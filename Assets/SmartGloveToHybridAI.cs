using System;
using System.Text;
using UnityEngine;
using NativeWebSocket;

[Serializable]
public class SmartGloveData
{
    public float vibration;
    public float speed;
    public string movement;
}

public class SmartGloveToHybridAI : MonoBehaviour
{
    WebSocket websocket;

    public string esp32Url = "ws://192.168.4.1:81";
    public HybridHandAI handAI;

    public float vibration;
    public float speed;
    public string movement;

    bool isConnecting = false;
    float reconnectTimer = 0f;

    async void Start()
    {
        await ConnectToESP32();
    }

    async System.Threading.Tasks.Task ConnectToESP32()
    {
        if (isConnecting) return;

        isConnecting = true;

        Debug.Log("Connecting to ESP32: " + esp32Url);

        websocket = new WebSocket(esp32Url);

        websocket.OnOpen += () =>
        {
            Debug.Log("Connected to ESP32 Smart Glove");
            isConnecting = false;
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("ESP32 WebSocket Error: " + e);
            isConnecting = false;
        };

        websocket.OnClose += (e) =>
        {
            Debug.LogWarning("Disconnected from ESP32");
            isConnecting = false;
        };

        websocket.OnMessage += (bytes) =>
        {
            string json = Encoding.UTF8.GetString(bytes);
            Debug.Log("ESP32 Data: " + json);

            SmartGloveData data = JsonUtility.FromJson<SmartGloveData>(json);

            vibration = data.vibration;
            speed = data.speed;
            movement = data.movement;

            SendDataToHybridAI();
        };

        await websocket.Connect();
    }

    void SendDataToHybridAI()
    {
        if (handAI == null) return;

        handAI.movement = vibration;
        handAI.speed = speed;

        if (movement == "Stable")
            handAI.stability = 1.0f;
        else if (movement == "Moving")
            handAI.stability = 0.75f;
        else if (movement == "Shaking")
            handAI.stability = 0.35f;
    }

    async void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif

        if (websocket == null || websocket.State == WebSocketState.Closed)
        {
            reconnectTimer += Time.deltaTime;

            if (reconnectTimer >= 2f)
            {
                reconnectTimer = 0f;
                await ConnectToESP32();
            }
        }
    }

    async void OnApplicationQuit()
    {
        if (websocket != null)
            await websocket.Close();
    }
}