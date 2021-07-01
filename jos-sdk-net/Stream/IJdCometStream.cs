using Jd.Api.Stream.Connect;
using Jd.Api.Stream.Message;

namespace Jd.Api.Stream
{
    public interface IJdCometStream
    {
        void SetConnectionListener(IConnectionLifeCycleListener connectionLifeCycleListener);
        void SetMessageListener(IJdCometMessageListener cometMessageListener);
        void Start();
        void SJd();
    }
}
