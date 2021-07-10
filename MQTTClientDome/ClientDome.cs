using System;
using System.Text;
using System.Threading.Tasks;
using MQTTDome.Interface;
using MQTTDomeMode;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Exceptions;

namespace MQTTClientDome
{
    public class ClientDome : IMQTTAction
    {
        private IMqttClient client;
        private readonly MqttClientEntity model;
        public ClientDome(MqttClientEntity mqttClientEntity) {
            model = mqttClientEntity;
        }

        public async Task StartAsync()
        {
            try
            {
                client = new MqttFactory().CreateMqttClient();
                var build = new MqttClientOptionsBuilder()
                .WithClientId(model.ClientId)
                .WithCredentials(model.Account, model.PassWord)
                .WithTcpServer(model.IP, model.Port);
                IMqttClientOptions options = build.Build();
                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MessageReceivedHandler);
                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate(ConnectedHandler);
                client.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(DisconnectedHandler);
                await client.ConnectAsync(options);
            }
            catch (MqttConnectingFailedException ex) {
                Console.WriteLine("身份校验失败");
            }
            catch (Exception ex)
            {
                var name = ex.GetType().FullName;
                Console.WriteLine("出现异常");
                Console.WriteLine(ex.Message);
            }
        }

        private void DisconnectedHandler(MqttClientDisconnectedEventArgs obj)
        {
            Console.WriteLine("本客户端已经断开连接");
            Console.WriteLine();
        }

        /// <summary>
        /// 连接成功
        /// </summary>
        /// <param name="obj"></param>
        private void ConnectedHandler(MqttClientConnectedEventArgs obj)
        {
            Console.WriteLine("本客户端已连接成功");
            Console.WriteLine($"地址:{model.IP}");
            Console.WriteLine($"端口:{model.Port}");
            Console.WriteLine($"客户端:{model.ClientId}");
            Console.WriteLine($"账号:{model.Account}");
            Console.WriteLine();
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
