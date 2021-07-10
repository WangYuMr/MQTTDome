using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDomeMode
{
    public   class MQTTConfigEntity
    {
       /// <summary>
       /// 服务器模式?
       /// </summary>
        public bool IsServerMode { get; set; }
        /// <summary>
        /// 服务端实体
        /// </summary>
        public MQTTServerEntity Server { get; set; }
        /// <summary>
        /// 客户端实体
        /// </summary>
        public MqttClientEntity Client { get; set; }

    }
}
