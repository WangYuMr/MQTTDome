using MQTTDome.Interface;
using MQTTDomeMode;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MQTTServerDome
{
    public class ServerDome : IMQTTAction
    {
        private IMqttServer server;
        private readonly MQTTServerEntity model;
        public ServerDome(MQTTServerEntity mQTTServer) {
            model = mQTTServer;
        }

        public async Task StartAsync()
        {
            if (server == null||!server.IsStarted)
            {

                server = new MqttFactory().CreateMqttServer();
                MqttServerOptionsBuilder serverOptions = new MqttServerOptionsBuilder();
                //默认监听端口
                  //serverOptions.WithDefaultEndpointBoundIPAddress(IPAddress.Parse("0.0.0.0."));
                  serverOptions.WithDefaultEndpointPort(model.Port);
                //校验客户端信息
                serverOptions.WithConnectionValidator(client => {
                        string Account = client.Username;
                        string PassWord = client.Password;
                        string clientid = client.ClientId;
                        if (Account == "test" && PassWord == "1234")
                        {
                            client.ReasonCode = MqttConnectReasonCode.Success;
                            Console.WriteLine("校验成功");
                        }
                        else
                        {
                            client.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                            Console.WriteLine("校验失败");
                        }
                    });
                
                    //客户端发送消息监听
                    server.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MessageReceivedHandler);
                    //server.UseApplicationMessageReceivedHandler(args=>{
                    //    Console.WriteLine("===================================================");
                    //    Console.WriteLine("收到消息:");
                    //    Console.WriteLine($"客户端:{args.ClientId}");
                    //    Console.WriteLine($"主题:{args.ApplicationMessage.Topic}");
                    //    Console.WriteLine($"消息:{Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}");
                    //    Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
                    //    Console.WriteLine();
                    //});
                    //客户端连接事件
                    server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(ClientConnectedHandler);
                    //server.UseClientConnectedHandler(args =>
                    //{
                    //    Console.WriteLine($"{args.ClientId}此客户端已经连接到服务器");
                    //});
                    //客户端断开连接事件
                    server.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(ClientDisconnectedHandler);
                    //server.UseClientDisconnectedHandler(args => {
                    //    Console.WriteLine($"断开连接的客户端:{args.ClientId}");
                    //    Console.WriteLine($"断开连接类型:{args.DisconnectType.ToString()}");
                    //});

                    //客户端订阅主题事件
                     server.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(ClientSubscribedTopicHandler);
                    //客户端取消订阅主题事件
                    server.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(ClientUnsubscribedTopicHandler);
                    //服务器启动事件
                    server.StartedHandler = new MqttServerStartedHandlerDelegate(StartedHandler);
                    //服务器停止事件
                    server.StoppedHandler = new MqttServerStoppedHandlerDelegate(StoppedHandler);
                    //启动服务器
                    await server.StartAsync(serverOptions.Build());
            }
        }

        public async Task StopAsync()
        {
            if (server != null)
            {
                if (server.IsStarted)
                {
                    await server.StopAsync();
                    server.Dispose();
                }
            }
        }

        /// <summary>
        /// 客户端取消订阅主题
        /// </summary>
        /// <param name="obj"></param>
        private void ClientUnsubscribedTopicHandler(MqttServerClientUnsubscribedTopicEventArgs obj)
        {
            Console.WriteLine($"客户端:{obj.ClientId}");
            Console.WriteLine($"取消订阅主题:{obj.TopicFilter}");
        }

        /// <summary>
        /// 客户端订阅的主题
        /// </summary>
        /// <param name="obj"></param>
        private void ClientSubscribedTopicHandler(MqttServerClientSubscribedTopicEventArgs obj)
        {
            Console.WriteLine($"客户端:{obj.ClientId}");
            Console.WriteLine($"订阅主题:{obj.TopicFilter.Topic}");
        }

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="obj"></param>
        private void ClientDisconnectedHandler(MqttServerClientDisconnectedEventArgs obj)
        {
            Console.WriteLine($"断开连接的客户端:{obj.ClientId}");
            Console.WriteLine($"断开连接类型:{obj.DisconnectType.ToString()}"); 
        }

        /// <summary>
        /// 客户端连接到服务器事件
        /// </summary>
        /// <param name="obj"></param>
        private void ClientConnectedHandler(MqttServerClientConnectedEventArgs obj)
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// 收到各个客户端发送的消息
        /// </summary>
        /// <param name="obj"></param>
        private void MessageReceivedHandler(MqttApplicationMessageReceivedEventArgs obj)
        {
            Console.WriteLine("===================================================");
            Console.WriteLine("收到消息:");
            Console.WriteLine($"客户端:{obj.ClientId}");
            Console.WriteLine($"主题:{obj.ApplicationMessage.Topic}");
            Console.WriteLine($"消息:{Encoding.UTF8.GetString(obj.ApplicationMessage.Payload)}");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine();
        }


        /// <summary>
        /// MQTT启动服务器事件
        /// </summary>
        /// <param name="obj"></param>
        private void StartedHandler(EventArgs obj)
        {
            Console.WriteLine($"程序已经启动!监听端口为:{model.Port}");
        }

        /// <summary>
        /// MQTT服务器停止事件
        /// </summary>
        /// <param name="obj"></param>
        private void StoppedHandler(EventArgs obj)
        {
            Console.WriteLine("程序已经关闭");
        }
    }
}
