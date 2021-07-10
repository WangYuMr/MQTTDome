using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using MQTTClientDome;
using MQTTDome.Interface;
using MQTTDomeMode;
using MQTTServerDome;

namespace MQTTDome
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonPath=Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "MQTTConfig.json");
            IConfigurationBuilder config = new ConfigurationBuilder();
            config = config.AddJsonFile(jsonPath);
            var root=config.Build();
            var entity=  root.Get<MQTTConfigEntity>();
            IMQTTAction mqtt;
            if (entity.IsServerMode)
            {
                mqtt =   new ServerDome(entity.Server);
            }
            else {
                mqtt = new ClientDome(entity.Client);
            }
            mqtt.StartAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}
