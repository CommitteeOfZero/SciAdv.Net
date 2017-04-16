using System.Threading.Tasks;

namespace ProjectAmadeus
{
    public interface IAsyncShutdown
    {
        Task ShutdownAsync();
    }
}
