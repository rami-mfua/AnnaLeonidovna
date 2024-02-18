using Bashirov_16.Pages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Bashirov_16.Model.PostgreModel;

namespace Bashirov_16
{
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
            if (!(e.Content is Page page)) return;
            this.Title = $"Project by Bashirov - {page.Title}";

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

            Console.WriteLine("Users exported to users.txt");

            // Открываем файл в notepad
            System.Diagnostics.Process.Start("notepad.exe", filePath);
        }

        public void ExportUsersToTextFile(string filePath)
        {
            using (MySqlConnection connection = db.getConnection())
            {
                connection.Open();

                string query = "SELECT id, login, user_role, firstname, lastname, patronymic FROM users";
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
    }
}
