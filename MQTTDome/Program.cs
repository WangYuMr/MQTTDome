using System;
using System.IO;
using System.Threading;
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
            ManualResetEvent exitEvent = new ManualResetEvent(false);
            Thread thread = new Thread(()=> {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "MQTTConfig.json");
                IConfigurationBuilder config = new ConfigurationBuilder();
                config = config.AddJsonFile(jsonPath);
                var root = config.Build();
                var entity = root.Get<MQTTConfigEntity>();
                IMQTTAction mqtt;
                if (entity.IsServerMode)
                {
                    mqtt = new ServerDome(entity.Server);
                }
                else
                {
                    mqtt = new ClientDome(entity.Client);
                }
                mqtt.StartAsync().GetAwaiter().GetResult();
            });
            thread.IsBackground = true;
            thread.Start();
            Console.WriteLine("按Control+C关闭程序");
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };
            exitEvent.WaitOne();
            Console.WriteLine("程序已经退出");
        }
    }
}
