using System.Windows;
using System;
using System.Windows;
using anotherGO.Helpers;
using Caliburn.Micro;
using Lib;
using System.Threading.Tasks;
using Lib.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.ComponentModel;
using Lib.Helpers;

namespace MainWindowNN
{
    
    public partial class MainWindow : Window
    {
        public RadnickiViewModel ViewModel { get; set; }

        public MainWindow()
        {
            
            InitializeComponent();
            ViewModel = new RadnickiViewModel();
            DataContext = ViewModel;
            Loaded += MainWindow_Loaded;

            //List<RadnikModel> Radnici = LoadDataAsync();

            workersListBox.ItemsSource = Radnici;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }



    }

    public class RadnickiViewModel : INotifyPropertyChanged
    {
        private List<RadnikModel> _radnici;

        public List<RadnikModel> Radnici
        {
            get { return _radnici; }
            set
            {
                _radnici = value;
                OnPropertyChanged(nameof(Radnici));
            }
        }

        public async Task<List<RadnikModel>> LoadDataAsync()
        {

            string apiKey = "vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
            string apiUrl = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var Radnici = JsonConvert.DeserializeObject<List<RadnikModel>>(response);

                Radnici = ListMerger.SpojiRadnike(Radnici);

                foreach (var radnik in Radnici)
                {
                    double round = radnik.HoursWorked.TotalHours;
                    radnik.TotalHoursWorked = Math.Round(round , 2);
                }

                //Radnici?.Sort((a, b) => b.HoursWorked.CompareTo(a.HoursWorked));

                //Radnici = new List<RadnikModel>(Radnici);

                return Radnici;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}



//https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==
//vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==