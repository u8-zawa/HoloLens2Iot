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

        // Unity Editor �Ń��C���X���b�h�ɖ߂��悤 SynchronizationContext.Current �L�^
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

        // ���b�Z�[�W��M���̏���
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

        // �ڑ�
        await mqttClient.ConnectAsync(options);

        // ���b�Z�[�W��M����w�� subscribe
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/co2").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/temp").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/gsr").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/ud").Build());

        // ���b�Z�[�W�̑��M���邽�߂̐ݒ�
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("prmn_iot/hello")
            .WithPayload("Hello HoloLens2")
            .WithExactlyOnceQoS()
            .Build();
        // ���b�Z�[�W�̑��M publish
        await mqttClient.PublishAsync(message);
    }

    async void LogMessage(object message, string payload = "")
    {
#if WINDOWS_UWP
 
        await Task.Run(() =>
        {
            // ���C���X���b�h�ɖ߂� UWP �̏ꍇ
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                Debug.Log(message);
 
                // GameObject�Ȃǂŕ`�悷��Ȃ炱�̒��ɏ��������� UWP �̏ꍇ
                
                // payload���ݒ肳��Ă��āA���g��SensorData�`����Json�̏ꍇ
                if (payload != "" && SensorData.IsSensorDataJson(payload))
                {
                    sensorData = SensorData.FromJson(payload);
                    Debug.Log(sensorData.ToJson());

                    MemorySensorData(sensorData);
                }
            }, true);
        });
#elif UNITY_EDITOR
        await Task.Run(() =>
        {

            SynchronizationContext.SetSynchronizationContext(context);

            // ���C���X���b�h�ɖ߂� Unty Editor �̏ꍇ
            context.Post((state) =>
            {
                // �P���Ƀ��O�ɏo�������Ȃ烁�C���X���b�h�ɖ߂���������
                Debug.Log(message);

                // GameObject�Ȃǂŕ`�悷��Ȃ炱�̒��ɏ���������

                // payload���ݒ肳��Ă��āA���g��SensorData�`����Json�̏ꍇ
                if (payload != "" && SensorData.IsSensorDataJson(payload))
                {
                    sensorData = SensorData.FromJson(payload);
                    Debug.Log(sensorData.ToJson());

                    MemorySensorData(sensorData);
                }
            }, null);
        });
#endif
    }

    public void GetSensorData(string sensor_name)
    {
        // ���b�Z�[�W�̑��M���邽�߂̐ݒ�
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("prmn_iot/get_sensor_data")
            .WithPayload(sensor_name)
            .WithExactlyOnceQoS()
            .Build();
        // ���b�Z�[�W�̑��M publish
        Task.Run(async () => {
            await mqttClient.PublishAsync(message);
        });
    }

    // SensorDataManager�����݂��Ă���΁ASensorData���i�[���ĕۑ�����
    private void MemorySensorData(SensorData data)
    {
        if( SensorDataManager.Instance != null)
        {
            SensorDataManager.Instance.UpdateData(data);
        }
    }
}