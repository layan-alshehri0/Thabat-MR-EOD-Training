using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject startPanel;         // اللوحة الأولى: البداية
    [SerializeField] GameObject gamePanel;          // اللوحة الثانية بعد Start
    [SerializeField] GameObject instructionPanel;   // اللوحة الثالثة بعد Next
    [SerializeField] GameObject resultPanel;        // لوحة النتائج
    [SerializeField] GameObject leaderboardPanel;   // لوحة المتصدرين

    [Header("Result UI")]
    [SerializeField] TMP_Text resultText;           // نص النجاح أو الفشل
    [SerializeField] TMP_Text timeText;             // نص الوقت

    [Header("Audio (اختياري)")]
    [SerializeField] AudioSource sfxSuccess;
    [SerializeField] AudioSource sfxFail;

    [Header("Game Logic (اختياري)")]
    [SerializeField] DefuseSequence sequence;       // سكربت تسلسل التفكيك
    [SerializeField] TimerUI timerUI;               // سكربت المؤقت

    void Awake()
    {
        // عند بداية المشهد، تظهر البداية فقط
        SafeSet(startPanel, true);
        SafeSet(gamePanel, false);
        SafeSet(instructionPanel, false);
        SafeSet(resultPanel, false);
        SafeSet(leaderboardPanel, false);
    }

    // زر Start → يفتح لوحة اللعب مباشرة
    public void OnPressStart()
    {
        SafeSet(startPanel, false);
        SafeSet(gamePanel, true);             // فتح اللعب
        SafeSet(instructionPanel, false);     // التعليمات مطفأة
        SafeSet(resultPanel, false);
        SafeSet(leaderboardPanel, false);
    }

    // زر Next في لوحة اللعب → يفتح التعليمات
    public void OnPressNext()
    {
        SafeSet(gamePanel, false);            // إغلاق اللعب
        SafeSet(instructionPanel, true);      // فتح التعليمات
        if (timerUI) timerUI.ResetTimer(startHidden: true);
    }

    // زر Next في لوحة التعليمات → يخفي التعليمات
    public void OnPressNextInstruction()
    {
        SafeSet(instructionPanel, false);     // إخفاء التعليمات
        // هنا يمكنك إضافة أي شيء آخر يبدأ بعد التعليمات
        // مثل بدء اللعبة أو تشغيل المؤقت
    }

    // نجاح المهمة
    public void ShowSuccess(float totalSeconds = -1f)
    {
        SafeSet(resultPanel, true);
        SafeSet(gamePanel, false);
        SafeSet(leaderboardPanel, false);

        if (resultText) resultText.text = "Success";
        if (timeText && totalSeconds >= 0f) timeText.text = $"Time: {totalSeconds:0.00}s";
        if (sfxSuccess) sfxSuccess.Play();
        if (timerUI) timerUI.ShowFinalTime();
    }

    // فشل فوري
    public void ShowFail()
    {
        SafeSet(resultPanel, true);
        SafeSet(gamePanel, false);
        SafeSet(leaderboardPanel, false);

        if (resultText) resultText.text = "Failed";
        if (timeText) timeText.text = "";
        if (sfxFail) sfxFail.Play();
    }

    // فشل بعد تأخير
    public void ShowFailAfter(float seconds)
    {
        StartCoroutine(CoShowFail(seconds));
    }

    public void ShowFailAfterFrom(AutoExplosion explosion, float extraDelay = 0f)
    {
        float d = explosion ? explosion.GetDurationPublic() : 0.5f;
        StartCoroutine(CoShowFail(d + Mathf.Max(0f, extraDelay)));
    }

    IEnumerator CoShowFail(float s)
    {
        yield return new WaitForSeconds(s);
        ShowFail();
    }

    // عرض لوحة المتصدرين
    public void ShowLeaderboardButton()
    {
        SafeSet(leaderboardPanel, true);
        SafeSet(resultPanel, false);
    }

    // إخفاء لوحة المتصدرين
    public void HideLeaderboard()
    {
        SafeSet(leaderboardPanel, false);
        SafeSet(resultPanel, true);
    }

    // إعادة اللعبة
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // دالة مساعدة لتفعيل/إلغاء تفعيل أي لوحة بأمان
    void SafeSet(GameObject go, bool state)
    {
        if (go && go.activeSelf != state) go.SetActive(state);
    }
}