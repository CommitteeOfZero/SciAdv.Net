using Newtonsoft.Json;

namespace ProjectCtrlF
{
    public class Redirect
    {
        [JsonProperty("original_id")]
        public int OriginalId { get; set; }
        [JsonProperty("new_id")]
        public int NewId { get; set; }
    }
}
