using System;
using System.Collections.Generic;
using System.Data;
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

namespace Bashirov_16.Pages
{
    /// <summary>
    /// Interaction logic for CalcPage.xaml
    /// </summary>
    public partial class CalcPage : Page
    {
        public CalcPage()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.Name)
            {
                case "CancelButton":
                    CalcTextBox.Text = "";
                    break;
                case "CommaBtn":
                    if (!CalcTextBox.Text.Contains('.'))
                    {
                        CalcTextBox.Text += ".";
                    }
                    break;
                case "ZeroBtn":
                    CalcTextBox.Text += "0";
                    break;
                case "OneBtn":
                    CalcTextBox.Text += "1";
                    break;
                case "TwoBtn":
                    CalcTextBox.Text += "2";
                    break;
                case "ThreeBtn":
                    CalcTextBox.Text += "3";
                    break;
                case "FourBtn":
                    CalcTextBox.Text += "4";
                    break;
                case "FiveBtn":
                    CalcTextBox.Text += "5";
                    break;
                case "SixBtn":
                    CalcTextBox.Text += "6";
                    break;
                case "SevenBtn":
                    CalcTextBox.Text += "7";
                    break;
                case "EightBtn":
                    CalcTextBox.Text += "8";
                    break;
                case "NineBtn":
                    CalcTextBox.Text += "9";
                    break;
                case "PlusBtn":
                    CalcTextBox.Text += "+";
                    break;
                case "MinusBtn":
                    CalcTextBox.Text += "-";
                    break;
                case "DivideBtn":
                    CalcTextBox.Text += ":";
                    break;
                case "MultiplyBtn":
                    CalcTextBox.Text += "x";
                    break;
                case "EqualBtn":
                    try
                    {
                        string result = new DataTable().Compute(CalcTextBox.Text, null).ToString();
                        CalcTextBox.Text = result;
                    }
                    catch (Exception ex)
                    {
                        CalcTextBox.Text = "Error";
                        MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case "LogButton":
                    double logInput;
                    if (double.TryParse(CalcTextBox.Text, out logInput))
                    {
                        CalcTextBox.Text = Math.Log(logInput).ToString();
                    }
                    else
                    {
                        CalcTextBox.Text = "Invalid input";
                    }
                    break;
                case "RadicalButton":
                    double radicalInput;
                    if (double.TryParse(CalcTextBox.Text, out radicalInput))
                    {
                        CalcTextBox.Text = Math.Sqrt(radicalInput).ToString();
                    }
                    else
                    {
                        CalcTextBox.Text = "Invalid input";
                    }
                    break;
                case "DegreeButton":
                    double degreeInput;
                    if (double.TryParse(CalcTextBox.Text, out degreeInput))
                    {
                        CalcTextBox.Text = Math.Pow(degreeInput, 2).ToString();
                    }
                    else
                    {
                        CalcTextBox.Text = "Invalid input";
                    }
                    break;
                case "AbsoluteButton":
                    double absInput;
                    if (double.TryParse(CalcTextBox.Text, out absInput))
                    {
                        CalcTextBox.Text = Math.Abs(absInput).ToString();
                    }
                    else
                    {
                        CalcTextBox.Text = "Invalid input";
                    }
                    break;
                default:
                    MessageBox.Show("Invalid Input!");
                    break;
            }
        }
    }
}
