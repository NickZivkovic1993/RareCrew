using Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Helpers
{
    public class ListMerger
    {
        public static List<RadnikModel> SpojiRadnike(List<RadnikModel> Radnici)
        {
            //// sklopi sve radnike zajedno u po jedan model ,fala bogu pa sam koristio sam c# a ne ef-core
            //var spojeniRadnici = Radnici
            //    .GroupBy(e => e.EmployeeName)
            //    .Select(g => new RadnikModel
            //    {
            //        EmployeeName = g.Key,
            //        HoursWorked = TimeSpan.FromHours(g.Sum(e => e.HoursWorked.TotalHours))
            //    })
            //    .ToList();

            //return spojeniRadnici;

                var spojeniRadnici = Radnici
                .GroupBy(e => e.EmployeeName)
                .Select(g => new RadnikModel
                {
                    EmployeeName = g.Key,
                    HoursWorked = TimeSpan.FromHours(g.Sum(e => e.HoursWorked.TotalHours))
                })
                .ToList();

                return spojeniRadnici;
        }

    }
}
