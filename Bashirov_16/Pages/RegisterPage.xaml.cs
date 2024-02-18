using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;
using static Bashirov_16.Model.PostgreModel;
using static Bashirov_16.Helpers.Hashing;

namespace Bashirov_16.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            NavigationService?.Navigate(new AuthenticationPage());
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string password = PasswordBox.Password;
            string reEnteredPassword = RePasswordBox.Password;
            string role = RoleBox.SelectedItem?.ToString();
            string firstname = "";
            string lastname = "";
            string patronymic = "";
            string fullname = FullNameBox.Text;

            if (!string.IsNullOrEmpty(fullname))
            {
                string[] nameParts = fullname.Split(' ');

                firstname = nameParts[0];
                lastname = nameParts.Length > 1 ? nameParts[1] : "";
                patronymic = nameParts.Length > 2 ? nameParts[2] : "";
            }

            DB db = new DB();

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(reEnteredPassword) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка совпадения паролей
            if (password != reEnteredPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создание подключения к базе данных
            using (MySqlConnection connection = db.getConnection())
            {
                try
                {
                    // Открытие подключения
                    connection.Open();

                    // Создание SQL-запроса для вставки новой записи пользователя
                    string query = "INSERT INTO users (login, user_password, user_role, firstname, lastname, patronymic) VALUES (@Login, @Password, @Role, @Firstname, @Lastname, @Patronymic)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", GetHashedPassword(password));
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Firstname", firstname);
                    command.Parameters.AddWithValue("@Lastname", lastname);
                    command.Parameters.AddWithValue("@Patronymic", patronymic);

                    // Выполнение запроса
                    int rowsAffected = command.ExecuteNonQuery();

                    // Проверка успешности регистрации
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Registration successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        connection.Close();
                        NavigationService?.Navigate(new AuthenticationPage());
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred during registration: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
