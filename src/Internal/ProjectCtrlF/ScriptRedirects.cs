using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProjectCtrlF
{
    public class ScriptRedirects
    {
        public ScriptRedirects()
        {
            Items = new List<Redirect>();
        }

        [JsonProperty("script_id")]
        public int ScriptId { get; set; }

        [JsonProperty("redirects")]
        public IList<Redirect> Items { get; set; }
    }
}
