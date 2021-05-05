namespace sUpdater.Models
{
    public class Installer
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string DL { get; set; }
        public string LaunchArgs { get; set; }
    }
}
