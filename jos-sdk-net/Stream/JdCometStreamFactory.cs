using System;

namespace Jd.Api.Stream
{
    public class JdCometStreamFactory
    {
        private Configuration configruation;

        public JdCometStreamFactory(Configuration configuration)
        {
            if (configuration == null)
            {
                throw new Exception("configuration is must not null!");
            }
            this.configruation = configuration;
        }

        public IJdCometStream GetInstance()
        {
            return new JdCometStreamImpl(configruation);
        }
    }
}
