namespace Lib.Models
{
    [Serializable]
    public class RadnikModel
    {
        // zakomentarisani propovi nisu korisni za resavanje problema pa nema potrebe da koristim resurse
       // public int Id { get; set; }
        public string EmployeeName { get; set; }

        public DateTime StarTimeUtc { get; set; }

        public DateTime EndTimeUtc { get; set; }


        public TimeSpan HoursWorked
        {
            get
            {
                return EndTimeUtc - StarTimeUtc;
            }
            set
            {
                
                EndTimeUtc = StarTimeUtc + value;
            }
        }

        public double TotalHoursWorked { get; set; }
        public double GetTotalHoursWorked()
        {
            return HoursWorked.TotalHours;
        }

        //public string EntryNotes { get; set; }

        //public bool DeletedOn { get; set; } = false;
    }
}
