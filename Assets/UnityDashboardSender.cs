using System;
using UnityEngine;
using NativeWebSocket;

[Serializable]
public class DashboardData
{
    public float vibration;
    public float speed;
    public string movement;
    public string unityStatus;
    public int score;
    public int warnings;
    public int dangerCount;
}

public class UnityDashboardSender : MonoBehaviour
{
    WebSocket websocket;

    public string dashboardServerUrl = "ws://192.168.4.2:8002/unity";
    public SmartGloveToHybridAI gloveReceiver;
    public HybridHandAI handAI;

    public float sendRate = 0.1f;
    float sendTimer = 0f;
    bool connected = false;

    async void Start()
    {
        Debug.Log("Connecting to Dashboard Server: " + dashboardServerUrl);

        websocket = new WebSocket(dashboardServerUrl);

        websocket.OnOpen += () =>
        {
            connected = true;
            Debug.Log("Connected to Dashboard Server");
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("Dashboard WebSocket Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            connected = false;
            Debug.LogWarning("Dashboard Server Disconnected");
        };

        await websocket.Connect();
    }

    void Update()
    {
        Debug.Log("Update running...");

#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif

        sendTimer += Time.deltaTime;

        if (sendTimer >= sendRate)
        {
            sendTimer = 0f;
            SendDashboardData();
        }
    }

    async void SendDashboardData()
    {
        Debug.Log("SendDashboardData called");
        if (!connected)
        {
            Debug.LogWarning("Dashboard not connected yet");
            return;
        }

        if (gloveReceiver == null)
        {
            Debug.LogError("Glove Receiver is missing");
            return;
        }

        if (handAI == null)
        {
            Debug.LogError("HybridHandAI is missing");
            return;
        }

        DashboardData data = new DashboardData
        {
            vibration = gloveReceiver.vibration,
            speed = gloveReceiver.speed,
            movement = gloveReceiver.movement,
            unityStatus = handAI.aiResult,
            score = handAI.score,
            warnings = handAI.warningsCount,
            dangerCount = handAI.dangerCount
        };

        string json = JsonUtility.ToJson(data);

        Debug.Log("Sending dashboard data: " + json);

        await websocket.SendText(json);
    }

    async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }
}