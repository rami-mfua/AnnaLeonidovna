using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            UpdateCurrentDateTime();
        }

        // Свойство для привязки к текущей дате и времени
        private DateTime _currentDateTime;


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
            }
            else
            {
                ButtonBack.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }
    }
}
