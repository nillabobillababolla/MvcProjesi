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

    public class SurveyReport
    {
        public string question { get; set; }
        public double point { get; set; }
    }

    public class TechReport
    {
        public string nameSurname { get; set; }
        public double point { get; set; }
    }
}
