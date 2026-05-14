// TaskChecklist.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TaskChecklist : MonoBehaviour
{
    [Header("اسحبي التوقلز هنا بالترتيب")]
    [SerializeField] private List<Toggle> taskToggles = new List<Toggle>();

    [Header("زر الإكمال داخل لوحة المهام")]
    [SerializeField] private GameObject completeButton;

    [Header("اسم السين التالية (مثل ما هو في Build Settings)")]
    [SerializeField] private string nextSceneName = "Scene2";

    private void Awake()
    {
        // مخفي من البداية
        if (completeButton) completeButton.SetActive(false);

        // كل ما تغيّر أي Toggle، نعيد الحساب
        foreach (var t in taskToggles)
            if (t) t.onValueChanged.AddListener(_ => Recalculate());

        // تأكد من الحالة أول ما تشتغل اللوحة
        Recalculate();
    }

    private void Recalculate()
    {
        bool allDone = true;
        foreach (var t in taskToggles)
        {
            if (!t || !t.isOn)
            {
                allDone = false;
                break;
            }
        }

        if (completeButton) completeButton.SetActive(allDone);
    }

    // اربطيها بزر الإكمال (OnClick)
    public void GoNextScene()
    {
        // أمان: ما ينتقل إلا إذا الكل منجز
        foreach (var t in taskToggles)
            if (!t || !t.isOn) return;

        SceneManager.LoadScene(nextSceneName);
    }
}
