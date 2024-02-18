using System;
using System.Collections.Generic;
using System.IO;
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
using Bashirov_16.Model;
using MySql.Data.MySqlClient;
using static Bashirov_16.Model.PostgreModel;
using static Bashirov_16.Helpers.Hashing;

namespace Bashirov_16.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {

        DB db = new DB();

        public AuthenticationPage()
        {
            InitializeComponent();
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password)) 
            {
                MessageBox.Show("You have left some fields blank!", "Blank fields!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "SELECT id, login, user_password, user_role, firstname, lastname, patronymic FROM users WHERE login = @Username AND user_password = @Password";
            db.openConnection();

            using (MySqlCommand command = new MySqlCommand(query, db.getConnection()))
            {
                command.Parameters.AddWithValue("@Username", TextBoxLogin.Text);
                command.Parameters.AddWithValue("@Password", PasswordBox.Password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashedPasswordFromDatabase = reader.GetString("user_password");
                        if (hashedPasswordFromDatabase == GetHashedPassword(PasswordBox.Password))
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

                            db.closeConnection();

                            switch (user.user_role)
                            {
                                case "admin":
                                    NavigationService?.Navigate(new CalcPage());
                                    break;
                                default:
                                    NavigationService?.Navigate(new CalcPage());
                                    break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Check your login or password!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                            PasswordBox.Password = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Access Denied! There is no such user!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                //int count = Convert.ToInt32(command.ExecuteScalar());

                //if (count == 1)
                //{
                //    db.closeConnection();
                //    MessageBox.Show("Авторизация прошла успешно!");
                    
                //    return;
                //}
                //else
                //{
                //    db.closeConnection();
                //    MessageBox.Show("Access Denied! There is no such user!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            NavigationService?.Navigate(new RegisterPage());
        }
    }
}
