# C#MQTT使用说明

1. 本次演示使用到的库为:**MQTTNET**
2. 开发环境:***.NET F*ramework 4.5**以上 或 **.Net Core 3.1**以上
3. MQTTNET 其实不太吃.Net版本，这里为了演示用的**.Net Core 3.1**
4. 因为MQTTNET 是一个标准库，这就意味着你.Net Core版本会使用了，**.NET Framework 4.5**

## 1.安装MQTTNet

### 两种方式

#### Shell

```shell
Install-Package MQTTNET
```

#### 可视化安装

1. 鼠标右击你的项目——>
2. **管理NuGet程序包**
3. 再NuGet包页面切换到**浏览**
4. 搜索MQTTNet



## 2.当前我的项目安装的依赖如下

1. MQTTnet 3.0.16 必要
2. Microsoft.Extensions.Configuration 3.1.16
3. Microsoft.Extensions.Configuration.Abstractions 3.1.16
4. Microsoft.Extensions.Configuration.FileExtensions 3.1.16
5. Microsoft.Extensions.Configuration.Json 3.1.16
6. Newtonsoft.Json 13.0.1
7. Microsoft.Extensions.Options.ConfigurationExtensions 3.1.16

## 3.如何使用

### 1.我想启动MQTT服务器,我该怎么做?

```c#
 IMqttServer server = new MqttFactory().CreateMqttServer();
 MqttServerOptionsBuilder serverOptions = new MqttServerOptionsBuilder();
 serverOptions.WithDefaultEndpointPort(8080);
 await server.StartAsync(serverOptions.Build());
```

### 2.我想知道MQTT服务器是否启动成功,我该怎么做?

```c#
server.StartedHandler = new MqttServerStartedHandlerDelegate(StartedHandler);
/// <summary>
/// MQTT启动服务器事件
/// </summary>
/// <param name="obj"></param>
private void StartedHandler(EventArgs obj)
{
  Console.WriteLine($"程序已经启动!监听端口为:{model.Port}");
}
```

### 3.我想知道MQTT服务器是否停止允许,我该怎么做?

```c#
 server.StoppedHandler = new MqttServerStoppedHandlerDelegate(StoppedHandler);
  /// <summary>
  /// MQTT服务器停止事件
  /// </summary>
  /// <param name="obj"></param>
  private void StoppedHandler(EventArgs obj)
  {
    Console.WriteLine("程序已经关闭");
  }
```

### 4.我想知道有那些设备连接上了服务器,我该怎么做?

```c#
//客户端连接事件
server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(ClientConnectedHandler);
/// <summary>
/// 客户端连接到服务器事件
/// </summary>
/// <param name="obj"></param>
private void ClientConnectedHandler(MqttServerClientConnectedEventArgs obj)
{
  throw new NotImplementedException();
}
```

```c#
   server.UseClientConnectedHandler(args =>
   {
   		Console.WriteLine($"{args.ClientId}此客户端已经连接到服务器");
   });
```

### 5.我想知道有那些设备断开连接,我该怎么做?

```C#
   //客户端断开连接事件
  server.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(ClientDisconnectedHandler);
  private void ClientDisconnectedHandler(MqttServerClientDisconnectedEventArgs obj)
  {
     Console.WriteLine($"断开连接的客户端:{obj.ClientId}");
     Console.WriteLine($"断开连接类型:{obj.DisconnectType.ToString()}"); 
  }
```

```c#
 server.UseClientDisconnectedHandler(args => {
    Console.WriteLine($"断开连接的客户端:{args.ClientId}");
    Console.WriteLine($"断开连接类型:{args.DisconnectType.ToString()}");
 });
```



### 6.我想监听消息,我该怎么做?

```C#
 server.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(MessageReceivedHandler);
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
```

```C#
  server.UseApplicationMessageReceivedHandler(args=>{
       Console.WriteLine("===================================================");
       Console.WriteLine("收到消息:");
       Console.WriteLine($"客户端:{args.ClientId}");
       Console.WriteLine($"主题:{args.ApplicationMessage.Topic}");
       Console.WriteLine($"消息:{Encoding.UTF8.GetString(args.ApplicationMessage.Payload)}");
       Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++");
       Console.WriteLine();
  });
```

### 7.我想连接的客户端校验身份信息后才允许收发消息,我该怎么做?

```c#
  IMqttServer server = new MqttFactory().CreateMqttServer();
  MqttServerOptionsBuilder serverOptions = new MqttServerOptionsBuilder();
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
 await server.StartAsync(serverOptions.Build());
```

### 8.我想知道客户端订阅主题与取消订阅主题,我该怎么做?

#### 1.订阅

```C#
  //客户端订阅主题事件
server.ClientSubscribedTopicHandler = new MqttServerClientSubscribedHandlerDelegate(ClientSubscribedTopicHandler);
 /// <summary>
 /// 客户端订阅的主题
 /// </summary>
 /// <param name="obj"></param>
 private void ClientSubscribedTopicHandler(MqttServerClientSubscribedTopicEventArgs obj)
 {
   Console.WriteLine($"客户端:{obj.ClientId}");
   Console.WriteLine($"订阅主题:{obj.TopicFilter.Topic}");
 }
```

#### 2.取消订阅

```c#
  //客户端取消订阅主题事件
 server.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(ClientUnsubscribedTopicHandler);
 /// <summary>
 /// 客户端取消订阅主题
 /// </summary>
 /// <param name="obj"></param>
 private void ClientUnsubscribedTopicHandler(MqttServerClientUnsubscribedTopicEventArgs obj)
 {
   Console.WriteLine($"客户端:{obj.ClientId}");
   Console.WriteLine($"取消订阅主题:{obj.TopicFilter}");
 }
```

### 9.我想关闭MQTT服务器,我该怎么做?

```c#
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
```

