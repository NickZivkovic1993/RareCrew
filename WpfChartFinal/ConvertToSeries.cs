using Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveCharts.Wpf;
using LiveCharts;
using Newtonsoft.Json.Linq;

namespace WpfChartFinal
{
    public class ConvertToSeries
    {
        
        public static async Task<SeriesCollection> Serija()
        {
            LoaderClass loaderInstance = new LoaderClass();
            List<RadnikModel> radnici = await loaderInstance.Loader();


            SeriesCollection serija = new SeriesCollection();

            foreach(var radnik in radnici)
            {
                serija.Add( new PieSeries
                    {
                        Title = radnik.EmployeeName,
                        Values = new ChartValues<double> { radnik.TotalHoursWorked },
                    });
            }

            PieChart pieChart = new PieChart
            {
                Series = serija,
            };

            return serija;

        }



    }
}
