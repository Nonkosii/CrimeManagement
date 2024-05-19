namespace CrimeManagement.Models
{
    public class Combined
    {
        public List<CriminalRecord> CriminalRecords { get; set; }

        public Suspect Suspect { get; set; }

        public int TotalOffenses { get; set; }
    }
}
