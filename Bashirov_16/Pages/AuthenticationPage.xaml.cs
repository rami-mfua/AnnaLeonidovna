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

namespace Bashirov_16.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationPage.xaml
    /// </summary>
    public partial class AuthenticationPage : Page
    {
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

            using (var db = new Bashirov_16_MarinichEntities())
            {
                var user = db.User
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == TextBoxLogin.Text && u.Password == PasswordBox.Password);

                if (user == null)
                {
                    MessageBox.Show("There is no such user", "Entries weren't found...", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("The user were found successfully!", "User were found", MessageBoxButton.OK, MessageBoxImage.Information);

                    switch (user.Role)
                    {
                        case "Страховой агент":
                            NavigationService?.Navigate(new CalcPage());
                            break;
                        case "Водитель":
                            NavigationService?.Navigate(new CalcPage());
                            break;
                        default:
                            NavigationService?.Navigate(new CalcPage());
                            break;
                    }
                }
            }
        }
    }
}
