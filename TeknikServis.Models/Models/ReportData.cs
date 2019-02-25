namespace TeknikServis.Models.Models
{
    public class ReportData
    {
    }

    public class DailyReport
    {
        public int completed { get; set; }
        public bool success { get; set; }
    }

    public class DailyProfitReport
    {
        public decimal completed { get; set; }
        public bool success { get; set; }
    }
}
