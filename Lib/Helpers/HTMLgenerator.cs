using Lib.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Helpers
{
    public class HTMLgenerator
    {
        //znam nije DRY al ono za wpf je manje vise bilo prva verzija
        public async Task GenerateHtmlReportAsync()
        {
            string apiKey = "vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
            string apiUrl = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var radnici = JsonConvert.DeserializeObject<List<RadnikModel>>(response);

                radnici = ListMerger.SpojiRadnike(radnici);

                foreach (var radnik in radnici)
                {
                    //extra totalhours worked??
                    double round = radnik.HoursWorked.TotalHours;
                    radnik.TotalHoursWorked = Math.Round(round, 2);

                    // i dalje ekstra total hours
                    //double originalHours = radnik.HoursWorked.TotalHours;
                    //radnik.TotalHoursWorked = Math.Round(originalHours, 2);

                    
                    //Console.WriteLine($"Original Hours: {originalHours}, Rounded Hours: {radnik.TotalHoursWorked}");
                }

                radnici.Last().EmployeeName = "Ostali";

                string htmlContent = GenerateHtml(radnici);

               
                //File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "output.html"), htmlContent);
                File.WriteAllText(@"D:\\output.html", htmlContent);

            }
        }
        

        public string GenerateHtml(List<RadnikModel> radnici)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine("<html>");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.AppendLine("<title>Radnici Report</title>");
            htmlBuilder.AppendLine("<style>");
            htmlBuilder.AppendLine("tr.red { background-color: red; }");
            htmlBuilder.AppendLine("</style>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("<h1>Radnici Report</h1>");

            htmlBuilder.AppendLine("<table border='1'>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine("<th>Name</th>");
            htmlBuilder.AppendLine("<th>Total Hours Worked</th>");
            htmlBuilder.AppendLine("</tr>");

            foreach (var radnik in radnici)
            {
                // provera za radnike
                bool isLessThan100Hours = radnik.TotalHoursWorked < 100;

                
                string rowStyle = isLessThan100Hours ? "class='red'" : "";

                htmlBuilder.AppendLine($"<tr {rowStyle}>");
                htmlBuilder.AppendLine($"<td>{radnik.EmployeeName}</td>");
                htmlBuilder.AppendLine($"<td>{radnik.TotalHoursWorked}</td>");
                htmlBuilder.AppendLine("</tr>");
            }

            htmlBuilder.AppendLine("</table>");

            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");

            return htmlBuilder.ToString();
        }
    }

    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return TimeSpan.Parse((string)reader.Value);
            }
            throw new JsonException($"Unexpected token type '{reader.TokenType}' when parsing TimeSpan.");
        }

        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("c"));
        }
    }
}

        //// ovo je kopija sa stack overflowa i ona pravi problem
        //public async Task GenerateHtmlReportAsync()
        //{
        //    string apiKey = "vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
        //    string apiUrl = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code={apiKey}";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        var response = await client.GetStringAsync(apiUrl);

        //        // Use custom JsonConverter for TimeSpan serialization/deserialization
        //        var settings = new JsonSerializerSettings
        //        {
        //            Converters = { new TimeSpanConverter() },
        //        };

        //        var radnici = JsonConvert.DeserializeObject<List<RadnikModel>>(response, settings);

        //        radnici = ListMerger.SpojiRadnike(radnici);

        //        foreach (var radnik in radnici)
        //        {
        //            double originalHours = radnik.HoursWorked.TotalHours;

        //            // Assuming HoursWorked is a TimeSpan, you might want to round based on the total minutes
        //            double roundedHours = Math.Round(originalHours * 60) / 60;

        //            radnik.TotalHoursWorked = roundedHours;

        //            // Log original and rounded hours for debugging
        //            Console.WriteLine($"Original Hours: {originalHours}, Rounded Hours: {roundedHours}");
        //        }

        //        string htmlContent = GenerateHtml(radnici);

        //        // Save HTML
        //        // Use Path.Combine to ensure a correct path
        //        string outputPath = Path.Combine(Environment.CurrentDirectory, "output.html");
        //        File.WriteAllText(outputPath, htmlContent);
        //        Console.WriteLine($"HTML report saved to: {outputPath}");
        //    }
        //}