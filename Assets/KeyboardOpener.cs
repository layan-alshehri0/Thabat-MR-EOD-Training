using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class KeyboardOpener : MonoBehaviour, IPointerClickHandler, ISelectHandler
{
    public GameObject keyboardPanel;   // اسحبي KeyboardPanel هنا
    public VirtualKeyboard virtualKeyboard; // (اختياري) لو عندك السكربت الثاني

    TMP_InputField input;

    void Awake()
    {
        input = GetComponent<TMP_InputField>();
        if (input) input.shouldHideMobileInput = true; // يمنع كيبورد النظام
    }

    // يشتغل في الماوس/اللمس/أشعة XR
    public void OnPointerClick(PointerEventData eventData) => Show();

    // يشتغل لما يصير للحقل Focus من نظام الـUI (أفضل في الـQuest)
    public void OnSelect(BaseEventData eventData) => Show();

    void Show()
    {
        if (keyboardPanel)
        {
            keyboardPanel.SetActive(true);
            keyboardPanel.transform.SetAsLastSibling(); // يخليه قدّام
        }

        // اربط الهدف للكيبورد الافتراضي تلقائياً
        if (virtualKeyboard && input)
            virtualKeyboard.SetTarget(input);

        Debug.Log("[KeyboardOpener] Open keyboard");
    }
}
