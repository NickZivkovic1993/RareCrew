namespace anotherGO.Models
{
    [Serializable]
    public class RadnikModel
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }

        public DateTime StarTimeUtc { get; set; }

        public DateTime EndTimeUtc { get; set; }

        public string EntryNotes { get; set; }

        public bool DeletedOn { get; set; } = false;
    }
}
