using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using MQTTnet;
using MQTTnet.Client;
using UnityEngine;

public class MqttController : MonoBehaviour
{
    private IMqttClient mqttClient;
    private IMqttClientOptions options;
    private SynchronizationContext context;
    private string host = "mqtt.beebotte.com";
    [SerializeField] private string channel_token;

    private SensorData sensorData;

    async void Start()
    {
        Debug.Log("Start");

        // Unity Editor でメインスレッドに戻れるよう SynchronizationContext.Current 記録
        context = SynchronizationContext.Current;

        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();

        options = new MqttClientOptionsBuilder()
            .WithTcpServer(host, 1883)
            .WithCredentials(channel_token)
            .Build();

        mqttClient.Connected += (s, e) =>
        {
            LogMessage("mqttClient.Connected");
        };

        mqttClient.Disconnected += async (s, e) =>
        {

            LogMessage("mqttClient.Disconnected");

            if (e.Exception == null)
            {
                LogMessage("Exception Disconnected");

                return;
            }

            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                LogMessage("Unexception disconnected");
            }, true);

            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await mqttClient.ConnectAsync(options);
            }
            catch
            {
                LogMessage("Reconnect failed.");
            }
        };

        // メッセージ受信時の処理
        mqttClient.ApplicationMessageReceived += (s, e) =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var qos = e.ApplicationMessage.QualityOfServiceLevel;
            var retain = e.ApplicationMessage.Retain;

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("ApplicationMessageReceived");
            stringBuilder.AppendLine($"Topic = {topic}");
            stringBuilder.AppendLine($"Payload = {payload}");
            stringBuilder.AppendLine($"QoS = {qos}");
            stringBuilder.AppendLine($"Retain = {retain}");
            LogMessage(stringBuilder, payload);
        };

        // 接続
        await mqttClient.ConnectAsync(options);

        // メッセージ受信する購読 subscribe
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/co2").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/temp").Build());

        // メッセージの送信するための設定
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("prmn_iot/hello")
            .WithPayload("Hello HoloLens2")
            .WithExactlyOnceQoS()
            .Build();
        // メッセージの送信 publish
        await mqttClient.PublishAsync(message);
    }

    async void LogMessage(object message, string payload = "")
    {
#if WINDOWS_UWP
 
        await Task.Run(() =>
        {
            // メインスレッドに戻す UWP の場合
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                Debug.Log(message);
 
                // GameObjectなどで描画するならこの中に処理を書く UWP の場合
                
                // payloadが設定されていて、中身がJsonの場合
                if (payload != "" && IsJson(payload))
                {
                    sensorData = SensorData.FromJson(payload);
                    Debug.Log(sensorData.ToString());
                }
            }, true);
        });
#elif UNITY_EDITOR
        await Task.Run(() =>
        {

            SynchronizationContext.SetSynchronizationContext(context);

            // メインスレッドに戻す Unty Editor の場合
            context.Post((state) =>
            {
                // 単純にログに出すだけならメインスレッドに戻さず書ける
                Debug.Log(message);

                // GameObjectなどで描画するならこの中に処理を書く

                // payloadが設定されていて、中身がJsonの場合
                if (payload != "" && IsJson(payload))
                {
                    sensorData = SensorData.FromJson(payload);
                    Debug.Log(sensorData.ToString());
                }
            }, null);
        });
#endif
    }

    public void GetSensorData(string sensor_name)
    {
        // メッセージの送信するための設定
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("prmn_iot/get_sensor_data")
            .WithPayload(sensor_name)
            .WithExactlyOnceQoS()
            .Build();
        // メッセージの送信 publish
        Task.Run(async () => {
            await mqttClient.PublishAsync(message);
        });
    }

    private Boolean IsJson(string data)
    {
        try
        {
            JsonUtility.FromJson<SensorData>(data);
        } catch 
        {
            return false;
        }
        return true;
    }
}