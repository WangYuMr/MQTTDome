using MQTTDome.Interface;
using MQTTDomeMode;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTTClientDome
{
    public class ClientDome : IMQTTAction
    {
        private IMqttClient client;
        private readonly MqttClientEntity model;
        private IMqttClientOptions options;
        public ClientDome(MqttClientEntity mqttClientEntity) {
            model = mqttClientEntity;
        }

        public async Task StartAsync()
        {
            try
            {
                 client = new MqttFactory().CreateMqttClient();
                var build = new MqttClientOptionsBuilder()
                 //配置客户端Id
                .WithClientId(model.ClientId)
                //配置登录账号
                .WithCredentials(model.Account, model.PassWord)
                //配置服务器IP端口 这里得端口号是可空的
                .WithTcpServer(model.IP, model.Port)
                .WithCleanSession()
                // .WithTcpServer("www.baidu.com")这是一个例子
                ;

                 options = build.Build();
                //收到服务器发来消息
               client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MessageReceivedHandler);
                //client.UseApplicationMessageReceivedHandler(args=> {
                //    Console.WriteLine("===================================================");
                //    Console.WriteLine("收到消息:");
                //    Console.WriteLine($"主题:{args.ApplicationMessage.Topic}");
                //    Console.WriteLine($"消息:{Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}");
                //    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                //    Console.WriteLine();
                //});
                //连接成功 
            client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(ConnectedHandler);
                //client.UseConnectedHandler(args=> {
                //    Console.WriteLine("本客户端已连接成功");
                //    Console.WriteLine($"地址:{model.IP}");
                //    Console.WriteLine($"端口:{model.Port}");
                //    Console.WriteLine($"客户端:{model.ClientId}");
                //    Console.WriteLine($"账号:{model.Account}");
                //    Console.WriteLine();
                //    //第1种订阅方式
                //    client.SubscribeAsync("主题名称").GetAwaiter().GetResult();

                //    //第2种订阅方式
                //    List<MqttTopicFilter> Topics = new List<MqttTopicFilter>();
                //    Topics.Add(new MqttTopicFilter() { Topic = "主题名称A", QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce });
                //    Topics.Add(new MqttTopicFilter() { Topic = "主题名称B" });
                //    Topics.Add(new MqttTopicFilter() { Topic = "主题名称C" });
                //    client.SubscribeAsync(Topics.ToArray()).GetAwaiter().GetResult();

                //    //第3种订阅方式
                //    MqttClientSubscribeOptionsBuilder builder = new MqttClientSubscribeOptionsBuilder();
                //    builder.WithTopicFilter("AAA");
                //    client.SubscribeAsync(builder.Build()).GetAwaiter().GetResult();
                //});
                //断开连接 重连就写在此处
              client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(DisconnectedHandler);
                //client.UseDisconnectedHandler(args =>
                //{
                //    Console.WriteLine("本客户端已经断开连接");
                //    Console.WriteLine();
                //    try
                //    {
                //        client.ConnectAsync(options).GetAwaiter().GetResult();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("重连失败");
                //    }
                //});
                //连接
                await client.ConnectAsync(options);
            }
            catch (MqttConnectingFailedException) {
                Console.WriteLine("身份校验失败");
            }
            catch (Exception ex)
            {
                Console.WriteLine("出现异常");
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// 客户端断开连接后,如果需要重连在此处实现
        /// </summary>
        /// <param name="obj"></param>
        private async void DisconnectedHandler(MqttClientDisconnectedEventArgs obj)
        {
            Console.WriteLine("本客户端已经断开连接");
            Console.WriteLine();
            try
            {
              await   client.ConnectAsync(options);
            }
            catch (Exception)
            {
                Console.WriteLine("重连失败");
            }
        }

        /// <summary>
        /// 连接成功 在此处做订阅主题(Topic)操作
        /// </summary>
        /// <param name="obj"></param>
        private async  void ConnectedHandler(MqttClientConnectedEventArgs obj)
        {
            Console.WriteLine("本客户端已连接成功");
            Console.WriteLine($"地址:{model.IP}");
            Console.WriteLine($"端口:{model.Port}");
            Console.WriteLine($"客户端:{model.ClientId}");
            Console.WriteLine($"账号:{model.Account}");
            Console.WriteLine();
            //第1种订阅方式
            // client.SubscribeAsync("主题名称").GetAwaiter().GetResult();

            //第2种订阅方式
            List<MqttTopicFilter> Topics = new List<MqttTopicFilter>();
            Topics.Add(new MqttTopicFilter() { Topic = "主题名称A", QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce });
            Topics.Add(new MqttTopicFilter() { Topic = "主题名称B" });
            Topics.Add(new MqttTopicFilter() { Topic = "主题名称C" });
         await    client.SubscribeAsync(Topics.ToArray());

            //第3种订阅方式
            //MqttClientSubscribeOptionsBuilder builder = new MqttClientSubscribeOptionsBuilder();
            //builder.WithTopicFilter("AAA");
            //client.SubscribeAsync(builder.Build()).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 收到消息
        /// </summary>
        /// <param name="obj"></param>
        private void MessageReceivedHandler(MqttApplicationMessageReceivedEventArgs obj)
        {
            Console.WriteLine("===================================================");
            Console.WriteLine("收到消息:");
            Console.WriteLine($"主题:{obj.ApplicationMessage.Topic}");
            Console.WriteLine($"消息:{Encoding.UTF8.GetString(obj.ApplicationMessage.Payload)}");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine();
        }

        public async Task StopAsync()
        {
          await  client.DisconnectAsync();
        }
    }
}
