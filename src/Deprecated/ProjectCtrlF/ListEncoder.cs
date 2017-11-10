using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SciAdvNet.SC3Script;
using SciAdvNet.SC3Script.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace ProjectCtrlF
{
    public sealed class ListEncoder
    {
        private const string OutputFolderName = "Output";

        public ListEncoder()
        {
            Directory.CreateDirectory("Output");
        }

        public void EncodeLists(IEnumerable<ReplacementList> replacementLists)
        {
            JObject root = new JObject();

            var categories = new List<Category>();
            var deserializer = new CustomizedDeserializer();
            int id = 0;
            using (var stream = File.Create(Path.Combine(OutputFolderName, "stringtable.bin")))
            using (var writer = new BinaryWriter(stream))
            {
                int total = replacementLists.Sum(x => x.Items.Count);
                var stringTable = new int[total];
                writer.Write(new byte[total * 4]);

                foreach (var list in replacementLists)
                {
                    var category = new JObject();
                    foreach (var group in list.Items.GroupBy(x => x.ScriptId))
                    {
                        var scriptRedirects = new JObject();
                        foreach (var replacement in group)
                        {
                            var redirect = new Redirect { OriginalId = replacement.StringId, NewId = id };
                            scriptRedirects[replacement.StringId.ToString()] = id;

                            SC3String sc3String = deserializer.Deserialize(replacement.Text);
                            ImmutableArray<byte> bytes = sc3String.Encode(SC3Game.SteinsGateZero);

                            int offset = (int)stream.Position;
                            stringTable[id] = offset;

                            writer.Write(bytes.ToArray());
                            id++;
                        }

                        category[group.Key.ToString()] = scriptRedirects;
                    }

                    root[list.FriendlyName] = category;
                }

                writer.Seek(0, SeekOrigin.Begin);
                foreach (int value in stringTable)
                {
                    writer.Write(value);
                }

                string json = root.ToString();
                File.WriteAllText(Path.Combine(OutputFolderName, "stringredirection.json"), json);
            }
        }
    }

    public class Category
    {
        [JsonProperty("category")]
        public string Name { get; set; }
        [JsonProperty("elements")]
        public IList<ScriptRedirects> Elements { get; set; }
    }
}
