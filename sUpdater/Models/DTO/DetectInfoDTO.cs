namespace sUpdater.Models.DTO
{
    public class DetectInfoDTO
    {
        public int Id { get; private set; }
        public Arch Arch { get; private set; }
        public string RegKey { get; private set; }
        public string RegValue { get; private set; }
        public string ExePath { get; private set; }
    }
}
