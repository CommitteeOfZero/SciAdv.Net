using Newtonsoft.Json;
using PropertyChanged;

namespace ProjectAmadeus.Models
{
    [AddINotifyPropertyChangedInterface]
    public sealed class FontGenSettings
    {
        [JsonProperty("game.usrdir.system.mpk")]
        public string SystemMpkPath { get; set; }

        public string ExtraCharacters { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        public int BaselineOriginX { get; set; }
        public int BaselineOriginY { get; set; }
    }
}
