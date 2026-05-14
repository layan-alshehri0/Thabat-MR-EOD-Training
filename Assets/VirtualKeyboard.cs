using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class VirtualKeyboard : MonoBehaviour
{
    [Header("Target")]
    public TMP_InputField targetInput;   // اسحبي NameInput هنا

    [Header("Optional")]
    public GameObject keyboardPanel;     // لو تبين تُظهري/تخفي اللوحة
    public UnityEvent onSubmit;          // ينفّذ عند Enter (اختياري)

    void Awake()
    {
        // منع الكيبورد النظامي (اختياري)
        if (targetInput) targetInput.shouldHideMobileInput = true;
    }

    public void SetTarget(TMP_InputField input)
    {
        targetInput = input;
    }

    public void Append(string s)
    {
        if (!targetInput) return;
        // أضِف النص في موضع المؤشر (يدعم الكتابة وسط النص)
        int pos = targetInput.caretPosition;
        string t = targetInput.text;
        targetInput.text = t.Insert(pos, s);
        targetInput.caretPosition = pos + s.Length;
        targetInput.selectionStringAnchorPosition = targetInput.caretPosition;
        targetInput.selectionStringFocusPosition  = targetInput.caretPosition;
        targetInput.ForceLabelUpdate();
    }

    public void Space()      => Append(" ");
    public void Dash()       => Append("-");
    public void Underscore() => Append("_");

    public void Backspace()
    {
        if (!targetInput) return;
        int pos = targetInput.caretPosition;
        if (pos <= 0) return;
        string t = targetInput.text;
        targetInput.text = t.Remove(pos - 1, 1);
        targetInput.caretPosition = pos - 1;
        targetInput.selectionStringAnchorPosition = targetInput.caretPosition;
        targetInput.selectionStringFocusPosition  = targetInput.caretPosition;
        targetInput.ForceLabelUpdate();
    }

    public void ClearAll()
    {
        if (!targetInput) return;
        targetInput.text = "";
        targetInput.caretPosition = 0;
        targetInput.ForceLabelUpdate();
    }

    public void Submit()
    {
        // مثلاً قفل الكيبورد واستدعِ حدث التثبيت
        if (keyboardPanel) keyboardPanel.SetActive(false);
        onSubmit?.Invoke();
    }

    public void ShowKeyboard(bool show)
    {
        if (keyboardPanel) keyboardPanel.SetActive(show);
    }
}
