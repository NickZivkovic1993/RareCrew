using Lib.Helpers;
using Lib.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace WpfChartFinal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeAsync();

        }
        private async void InitializeAsync()
        {
            List<RadnikModel> radnici = await Loader();
            LoaderClass loaderInstance = new LoaderClass();

            SeriesCollection serija = new();

            foreach (var radnik in radnici)
            {
                serija.Add(new PieSeries
                {
                    Title = radnik.EmployeeName,
                    Values = new ChartValues<double> { radnik.TotalHoursWorked },
                });
            }

            // Use Dispatcher.Invoke to update UI elements from a background thread
            Dispatcher.Invoke(() =>
            {
                // Reference the PieChart defined in XAML using its name
                pieChart.Series = serija;
            });
        }

        
        public async Task<List<RadnikModel>> Loader()
        {
            //ovo treba da ide drugde u apsettings/json najverovatnije al za trenutne potrebe je dovoljno dobro
            string apiKey = "vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
            string apiUrl = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var radnici = JsonConvert.DeserializeObject<List<RadnikModel>>(response);

                radnici = ListMerger.SpojiRadnike(radnici);

                foreach (var radnik in radnici)
                {
                    double round = radnik.HoursWorked.TotalHours;
                    radnik.TotalHoursWorked = Math.Round(round, 2);
                }

                radnici.Last().EmployeeName = "Ostali";

               

                return radnici;


                //File.WriteAllText("output.html", htmlContent);
            }
        }

        

    }


}
