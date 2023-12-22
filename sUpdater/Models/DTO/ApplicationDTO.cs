using System.Collections.Generic;

namespace sUpdater.Models.DTO
{
    public class ApplicationDTO
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Version { get; private set; }
        public bool NoUpdate { get; private set; }
        public string WebsiteUrl { get; private set; }
        public string ReleaseNotesUrl { get; private set; }
        public List<DetectInfoDTO> Detectinfo { get; private set; }
        public List<InstallerDTO> Installers { get; private set; }
    }
}
