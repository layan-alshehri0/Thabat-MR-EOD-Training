using UnityEngine;
using System;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    public event Action leaderboardUpdated;

    [System.Serializable]
    public class Entry
    {
        public string playerName;
        public float time;
    }

    public List<Entry> leaderboardEntries = new List<Entry>();

    // ⏱️ متغيرات التايمر
    float startTime;
    bool isRunning;

    void Awake()
    {
        Instance = this;
    }

    // 🟢 تبدأ التوقيت (لما ينفتح الغطا)
    public void StartRun()
    {
        startTime = Time.time;
        isRunning = true;
        Debug.Log("Timer Started");
    }

    // 🔴 توقف التوقيت وتحفظ النتيجة
    public void FinishRun()
    {
        if (!isRunning) return;

        float finalTime = Time.time - startTime;
        isRunning = false;

        AddEntry("Player", finalTime);

        Debug.Log("Final Time: " + finalTime);
    }

    // 🏆 إضافة النتيجة وترتيبها
    public void AddEntry(string name, float time)
    {
        leaderboardEntries.Add(new Entry
        {
            playerName = name,
            time = time
        });

        leaderboardEntries.Sort((a, b) => a.time.CompareTo(b.time));

        leaderboardUpdated?.Invoke();
    }
}