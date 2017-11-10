using Newtonsoft.Json.Linq;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

namespace SC3Tools
{
    internal class Configuration
    {
        private const string DefaultConfigResourceName = "config.json";

        public string DefaultEditorName { get; private set; }
        public ImmutableDictionary<string, (string path, string args)> Editors { get; private set; }
        public ImmutableDictionary<string, string> Languages { get; private set; }

        public static Configuration Parse(Stream stream)
        {
            Debug.Assert(stream != null);
            using (var reader = new StreamReader(stream))
            {
                return Parse(reader.ReadToEnd());
            }
        }

        public static Configuration Parse(string jsonString)
        {
            var configuration = new Configuration();
            var root = JObject.Parse(jsonString);

            void ReadEditorsSection()
            {
                string defaultEditorStr = (string)root["defaultEditor"];
                var editors = ImmutableDictionary.CreateBuilder<string, (string, string)>();
                foreach (JProperty editor in root["editors"].Children())
                {
                    string path = (string)editor.Value["path"];
                    string args = (string)editor.Value["args"];

                    editors[editor.Name] = (path, args);
                    if (editor.Name.Equals(defaultEditorStr, StringComparison.OrdinalIgnoreCase))
                    {
                        configuration.DefaultEditorName = editor.Name;
                    }
                }

                configuration.Editors = editors.ToImmutable();
            }

            void ReadLanguagesSection()
            {
                var languages = ImmutableDictionary.CreateBuilder<string, string>();
                foreach (JProperty lang in root["languages"].AsJEnumerable())
                {
                    languages[lang.Name] = lang.Value.Value<string>();
                }

                configuration.Languages = languages.ToImmutable();
            }

            ReadEditorsSection();
            ReadLanguagesSection();
            return configuration;
        }

        public static Stream GetDefaultConfigStream()
        {
            var assembly = typeof(Configuration).Assembly;
            string fullResourceName = $"{typeof(Configuration).Namespace}.{DefaultConfigResourceName}";
            return assembly.GetManifestResourceStream(fullResourceName);
        }
    }
}
