using System;
using System.Text;

using System.Threading.Tasks;
using System.Threading;

using MQTTnet;
using MQTTnet.Client;
using UnityEngine;
using UnityEngine.EventSystems;

public class MqttController : MonoBehaviour
{
    private IMqttClient mqttClient;
    private IMqttClientOptions options;
    private SynchronizationContext context;
    private string host = "mqtt.beebotte.com";
    [SerializeField] private string channel_token;

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
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("ApplicationMessageReceived");
            stringBuilder.AppendLine($"Topic = {e.ApplicationMessage.Topic}");
            stringBuilder.AppendLine($"Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            stringBuilder.AppendLine($"QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            stringBuilder.AppendLine($"Retain = {e.ApplicationMessage.Retain}");
            LogMessage(stringBuilder);
        };

        // �ڑ�
        await mqttClient.ConnectAsync(options);

        // ���b�Z�[�W��M����w�� subscribe
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/co2").Build());
        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("prmn_iot/temp").Build());

        // ���b�Z�[�W�̑��M���邽�߂̐ݒ�
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("prmn_iot/hello")
            .WithPayload("Hello HoloLens2")
            .WithExactlyOnceQoS()
            .Build();
        // ���b�Z�[�W�̑��M publish
        await mqttClient.PublishAsync(message);
    }

    async void LogMessage(object message)
    {
#if WINDOWS_UWP
 
        await Task.Run(() =>
        {
            // ���C���X���b�h�ɖ߂� UWP �̏ꍇ
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                Debug.Log(message);
 
                // GameObject�Ȃǂŕ`�悷��Ȃ炱�̒��ɏ��������� UWP �̏ꍇ
 
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

            }, null);
        });
#endif
    }

    public void getSensorData(string sensor_name)
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
}