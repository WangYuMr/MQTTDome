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



## 当前我的项目安装的依赖如下

1. MQTTnet 3.0.16 必要
2. Microsoft.Extensions.Configuration 3.1.16
3. Microsoft.Extensions.Configuration.Abstractions 3.1.16
4. Microsoft.Extensions.Configuration.FileExtensions 3.1.16
5. Microsoft.Extensions.Configuration.Json 3.1.16
6. Newtonsoft.Json 13.0.1
7. Microsoft.Extensions.Options.ConfigurationExtensions 3.1.16