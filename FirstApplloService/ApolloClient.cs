using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace FirstApplloService
{
    class ApolloClient
    {
        MqttClient client;
        string MQTT_BROKER_ADDRESS = "localhost";
        int MQTT_BROKER_PORT = 61613;
        string password = "password";
        string account = "admin";
        /// <summary>
        /// 连接appache apollo代理服务
        /// </summary>
        public void LinkTCP()
        {
            //创建客户端实例
            //apache apollo连接的服务器地址和选择的协议有关，协议是tcp的话，那么端口号就是61613,
            //apollo.xml配置连接代理服务器的地址是:  <connector id="tcp" bind="tcp://0.0.0.0:61613" connection_limit="2000"/>
            //< connector id = "tls" bind = "tls://0.0.0.0:61614" connection_limit = "2000" />    
            //< connector id = "ws"  bind = "ws://0.0.0.0:61623"  connection_limit = "2000" />        
            //< connector id = "wss" bind = "wss://0.0.0.0:61624" connection_limit = "2000" />
     
            client = new MqttClient(MQTT_BROKER_ADDRESS,MQTT_BROKER_PORT,false,MqttSslProtocols.TLSv1_0,null,null); 


            // 注册消息接收处理事件，还可以注册消息订阅成功、取消订阅成功、与服务器断开等事件处理函数
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            // 注册消息订阅成功事件
            client.MqttMsgPublished += client_MqttMsgPublishedReceived;

            //生成客户端ID并连接服务器
            string clientId = "192.168.66.110";//Guid.NewGuid().ToString();
            client.Connect(clientId,account,password);
            Console.WriteLine("连接成功");
            Console.WriteLine(client.IsConnected);

        }
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //处理接收到的消息
            string msg = System.Text.Encoding.Default.GetString(e.Message);
            Console.WriteLine("收到消息" + msg );
        }

        void client_MqttMsgPublishedReceived(object sender, MqttMsgPublishedEventArgs e)
        {
            //处理接收到的消息
            
            Console.WriteLine("发布成功：消息的MessageId=" + e.MessageId);
        }
        /// <summary>
        /// 消息发布
        /// </summary>
        public void Broker_Public(string value = "Hello word!")
        {
            string strValue = value;

            // publish a message on "home" topic with QoS 2
            client.Publish("home", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,true);//retain参数表示是否持久化存在
            //Console.WriteLine("发布成功!");
        }
        /// <summary>
        /// 订阅主题
        /// </summary>
        public void Broker_Subscribe()
        {
            // 订阅主题"home" 消息质量为 2 
            client.Subscribe(new string[] { "home" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            Console.WriteLine("订阅成功!");
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Broker_Disconnect()
        {
            //检查是否连接
            if(client.IsConnected)
            {
                client.Disconnect();
                Console.WriteLine(":关闭连接");
            }           
        }
    }
}
