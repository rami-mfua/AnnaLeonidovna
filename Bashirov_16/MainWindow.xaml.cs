using Bashirov_16.Pages;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

using static Bashirov_16.Helpers.Hashing;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using static Bashirov_16.Model.PostgreModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;

namespace Bashirov_16
{

    public class CustomExcelFormat
    {
        [Column("login")]
        public string LOGIN { get; set; }
        [Column("password")]
        public string PASSWORD { get; set; }
        [Column("role")]
        public string ROLE { get; set; }
        [Column("firstname")]
        public string FIRSTNAME { get; set; }
        [Column("lastname")]
        public string LASTNAME { get; set; }
        [Column("patronymic")]
        public string PATRONYMIC { get; set; }
    }

        /// <summary>
        /// Логика взаимодействия для MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += MainWindow_Loaded;
            //UpdateCurrentDateTime();

            //SizeToContent = SizeToContent.WidthAndHeight;
            //WindowState = WindowState.Maximized;
        }

        // Свойство для привязки к текущей дате и времени
        private DateTime _currentDateTime;
        DB db = new DB();

        public DateTime CurrentDateTime
        {
            get
            {
                return _currentDateTime;
            }
            set
            {
                if (_currentDateTime != value)
                {
                    _currentDateTime = value;
                    //OnPropertyChanged(nameof(CurrentDateTime));
                }
            }
        }

        // Метод, который будет обновлять текущую дату и время
        private void UpdateCurrentDateTime()
        {
            CurrentDateTime = DateTime.Now;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание таймера, который будет обновлять дату и время каждую секунду
            Timer timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Вызов метода для обновления текущей даты и времени
            Dispatcher.Invoke(UpdateCurrentDateTime);
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is DocumentFormat.OpenXml.Spreadsheet.Page page)) return;
            this.Title = $"Project by Bashirov - {page.LocalName}";

            if (page is Pages.AuthenticationPage)
            {
                ButtonBack.Visibility = Visibility.Hidden;
                ExportButton.Visibility = Visibility.Hidden;
            }
            else
            {
                ButtonBack.Visibility = Visibility.Visible;
                ExportButton.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "C:\\Users\\Рамиль Баширов\\OneDrive\\users.txt";

            ExportUsersToTextFile(filePath);

            MessageBox.Show("Users exported to users.txt", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            // Открываем файл в notepad
            System.Diagnostics.Process.Start("notepad.exe", filePath);
        }

        public void ExportUsersToTextFile(string filePath)
        {
            using (MySqlConnection connection = db.getConnection())
            {
                connection.Open();

                string query = "SELECT id, login, user_password, user_role, firstname, lastname, patronymic FROM users";
                MySqlCommand command = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        while (reader.Read())
                        {
                            UserCustom user = new UserCustom()
                            {
                                id = reader.GetInt32("id"),
                                login = reader.GetString("login"),
                                user_password = reader.GetString("user_password"),
                                user_role = reader.GetString("user_role"),
                                firstname = reader.IsDBNull(4) ? string.Empty : reader.GetString("firstname"),
                                lastname = reader.IsDBNull(5) ? null : reader.GetString("lastname"),
                                patronymic = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };

                            string userLine = $"ID: {user.id}," +
                                $" Login: {user.login}," +
                                $" Role: {user.user_role}," +
                                $" Firstname: {user.firstname}," +
                                $" Lastname: {user.lastname}," +
                                $" Patronymic: {user.patronymic}";
                            writer.WriteLine(userLine);
                        }
                    }
                }
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
            };

            if (!fileDialog.ShowDialog().HasValue)
            {
                return;
            }

            var filePath = fileDialog.FileName;

            ExcelParser(filePath);

        }


        private void ExcelParser(string xlsx_file_path)
        {
            List<CustomExcelFormat> items = new List<CustomExcelFormat>();

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(xlsx_file_path, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();

                foreach (Row row in sheetData.Elements<Row>().Skip(1)) // skipping header row
                {
                    CustomExcelFormat item = new CustomExcelFormat();
                    int cellIndex = 1;

                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        string cellValue = cell.CellValue.Text;

                        switch (cellIndex)
                        {
                            case 1:
                                item.LOGIN = cellValue;
                                break;
                            case 2:
                                item.PASSWORD = cellValue;
                                break;
                            case 3:
                                item.ROLE = cellValue;
                                break;
                            case 4:
                                item.FIRSTNAME = cellValue;
                                break;
                            case 5:
                                item.LASTNAME = cellValue;
                                break;
                            case 6:
                                item.PATRONYMIC = cellValue;
                                break;
                        }

                        cellIndex++;
                    }

                    items.Add(item);
                }
            }

            InsertIntoXLSX(items);
        }

        private void InsertIntoXLSX(List<CustomExcelFormat> ls)
        {
            using (MySqlConnection connection = db.getConnection())
            {
                foreach (CustomExcelFormat item in ls)
                {
                    try
                    {
                        // Открытие подключения
                        connection.Open();

                        // Создание SQL-запроса для вставки новой записи пользователя
                        string query = "INSERT INTO users (login, user_password, user_role, firstname, lastname, patronymic) VALUES (@Login, @Password, @Role, @Firstname, @Lastname, @Patronymic)";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Login", item.LOGIN);
                        command.Parameters.AddWithValue("@Password", GetHashedPassword(item.PASSWORD));
                        command.Parameters.AddWithValue("@Role", item.ROLE);
                        command.Parameters.AddWithValue("@Firstname", item.FIRSTNAME);
                        command.Parameters.AddWithValue("@Lastname", item.LASTNAME);
                        command.Parameters.AddWithValue("@Patronymic", item.PATRONYMIC);

                        // Выполнение запроса
                        int rowsAffected = command.ExecuteNonQuery();

                        // Проверка успешности регистрации
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Exported to database successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            connection.Close();
                        }
                        else
                        {
                            MessageBox.Show("Exporting to database failed. Please try again.", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred during exporting: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
