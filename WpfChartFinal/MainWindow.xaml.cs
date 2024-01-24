using Lib.Helpers;
using Lib.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

/// Izvinjavam se za kranje neuredan kod bio sam malo pd stresom , mozete pogledati moje druge projekte na githubu da vidite kako zapravo kucam
/// jos jednom izvinite pogledao sam opet , i ja se jedva snalazim


namespace WpfChartFinal
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeAndSavePieChart();

        }


        private async void InitializeAndSavePieChart()
        {
            await InitializeAsync();
            await SavePieChartAsPngAsync();
        }
        private async Task InitializeAsync()
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

            Dispatcher.Invoke(() =>
            {
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


                
            }
        }

        private async Task SavePieChartAsPngAsync()
        {
            
            await SaveAsPngAsync(pieChart);
        }

        private async Task SaveAsPngAsync(UIElement visual)
        {
            await InitializeAsync();

            
            if (visual.IsMeasureValid && visual.IsArrangeValid &&
                visual.RenderSize.Width > 0 && visual.RenderSize.Height > 0)
            {
                
                var renderTargetBitmap = new RenderTargetBitmap(
                    (int)visual.RenderSize.Width,
                    (int)visual.RenderSize.Height,
                    96, 
                    96, 
                    PixelFormats.Pbgra32);

                
                renderTargetBitmap.Render(visual);

                //naravoucenije .... ne igraj se asyncom kad radis prvi put nesto....
                await Task.Delay(1000);

                await Dispatcher.BeginInvoke(new Action(() =>
                {
                    
                    var pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                    
                    string filePath = @"D:\PieChart.png";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        pngEncoder.Save(fileStream);
                    }
                }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            }
            else
            {
                MessageBox.Show("Nece da renderuje kako valje , nema png-a", "Error");
            }
        }



    }


}
