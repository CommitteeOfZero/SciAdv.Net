using Newtonsoft.Json;

namespace ProjectAmadeus.Models
{
    public sealed class Language
    {
        public string Name { get; set; }
        public int Code { get; set; }

        [JsonProperty("characters")]
        public string CharacterSet { get; set; }
    }
}
