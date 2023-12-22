using sUpdater.Models.DTO;

namespace sUpdater.Models
{
    public enum Arch { Any, x86, x64 };

    public class DetectInfo
    {
        public Arch Arch { get; private set; }
        public string RegKey { get; private set; }
        public string RegValue { get; private set; }
        public string ExePath { get; private set; }

        public DetectInfo(DetectInfoDTO detectInfoDTO)
        {
            Arch = detectInfoDTO.Arch;
            RegKey = detectInfoDTO.RegKey;
            RegValue = detectInfoDTO.RegValue;
            ExePath = detectInfoDTO.ExePath;
        }
    }
}
