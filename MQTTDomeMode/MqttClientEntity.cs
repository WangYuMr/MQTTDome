using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDomeMode
{
 public   class MqttClientEntity: MQTTBaseEntity
    {
        /// <summary>
        /// 连接地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
    }
}
