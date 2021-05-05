namespace sUpdater.Models
{
    public enum Arch { Any, x86, x64 };

    public class DetectInfo
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public Arch Arch { get; set; }
        public string RegKey { get; set; }
        public string RegValue { get; set; }
        public string ExePath { get; set; }
    }
}
