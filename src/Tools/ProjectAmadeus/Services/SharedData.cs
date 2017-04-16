using ProjectAmadeus.Models;
using System.Collections.Immutable;

namespace ProjectAmadeus.Services
{
    public sealed class SharedData
    {
        public ImmutableArray<Language> Languages { get; set; }
    }
}
