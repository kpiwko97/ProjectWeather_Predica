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
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
namespace ProjectWeather
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Cities> cities = new List<Cities>();
        MatchCollection _cityCountFound;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void ParseHTML(string htmlToFind)
        {
            WebClient webPage = new WebClient();
            string _entireHTML = webPage.DownloadString("https://www.timeanddate.com/weather/?low=c");
            _cityCountFound = Regex.Matches(_entireHTML, htmlToFind, RegexOptions.Compiled);
        }

        private void SetNameOfCity(MatchCollection _cityCountFound, string _charToFindInHTML)
        {
            foreach (Match match in _cityCountFound)
            {
                string _city = match.Groups[1].Value;
                int _lastIndex = _city.IndexOf(_charToFindInHTML);

                cities.Add(new Cities() { _name = _city.Remove(0, _lastIndex + 1), });
            }
        }

        private void SetTemperature(MatchCollection _cityCountFound, string _charToFindInHTML)
        {
            int _index = 0;

            foreach (Match match in _cityCountFound)
            {

                int _temperatureToSet;
                string _city = match.Groups[1].Value;
                int _lastIndexCharToCut = _city.IndexOf(_charToFindInHTML);

                try
                {
                    _temperatureToSet = Convert.ToInt32(_city.Remove(0, _lastIndexCharToCut + 1));
                }
                catch
                {
                    string _charToFindInHTMLException = "i>";
                    int _lastIndexCharToCutException = _city.IndexOf(_charToFindInHTMLException);

                    _temperatureToSet = Convert.ToInt32(_city.Remove(0, _lastIndexCharToCutException + 2));

                    _index++;
                }

                cities[_index]._temperature = _temperatureToSet;
                _index++;

            }
        }

        private void PrintWeather(List<Cities> city1)
        {

            for (int _cityIndex = 0; _cityIndex < 50; _cityIndex++)
            {
                string _resultat = "";
                _resultat += (_cityIndex + 1).ToString() + ") In " + city1[_cityIndex]._name + " is " + city1[_cityIndex]._temperature + "°C.\r\n";
                WeatherShowLabel.Content += _resultat;
            }
        }

        private void SortByTemperature()
        {
            Cities city1 = new Cities();
            cities.Sort(city1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.timeanddate.com/weather/?low=c");
        }

        private void Refresher_Click(object sender, RoutedEventArgs e)
        {
            cities.Clear();
            WeatherShowLabel.Content = "";
            _cityCountFound = null;

            ParseHTML("<a href=\"/weather/(.*?)</a><span id");
            SetNameOfCity(_cityCountFound, ">");

            ParseHTML("<td class=rbi(.+?)&");
            SetTemperature(_cityCountFound, ">");

            SortByTemperature();

            PrintWeather(cities);
        }
    }
}
