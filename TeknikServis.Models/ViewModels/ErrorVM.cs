namespace TeknikServis.Models.ViewModels
{
    public class ErrorVM
    {
        public string Text { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int ErrorCode { get; set; }
    }
}
