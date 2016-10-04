using SciAdvNet.SC3;

namespace ProjectAmadeus
{
    public interface ITab
    {
        string FilePath { get; set; }
        SC3Module Module { get; }
    }
}
