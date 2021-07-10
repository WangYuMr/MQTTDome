using System;
using System.Threading.Tasks;

namespace MQTTDome.Interface
{
    public interface IMQTTAction
    {
        public Task StartAsync();
        public Task StopAsync();
    }
}
