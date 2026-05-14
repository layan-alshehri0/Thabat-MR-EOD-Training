using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardBuilder : MonoBehaviour
{
    [Header("Keyboard Settings")]
    public VirtualKeyboard virtualKeyboard;   // اربطي سكربت VirtualKeyboard
    public GameObject buttonPrefab;           // زر جاهز (يحتوي TextMeshProUGUI و Button)
    public RectTransform keyboardPanel;       // اللوحة اللي بتحط فيها الأزرار
    public int columns = 10;                  // عدد الأعمدة
    public float spacing = 5f;                // المسافة بين الأزرار

    [TextArea]
    public string keys = "QWERTYUIOPASDFGHJKLZXCVBNM"; // الحروف

    void Start()
    {
        BuildKeyboard();
    }

    void BuildKeyboard()
    {
        if (!buttonPrefab || !keyboardPanel) return;

        GridLayoutGroup grid = keyboardPanel.GetComponent<GridLayoutGroup>();
        if (!grid) grid = keyboardPanel.gameObject.AddComponent<GridLayoutGroup>();

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(spacing, spacing);

        // حجم الزر بحسب حجم اللوحة وعدد الأعمدة
        float cellW = (keyboardPanel.rect.width - (spacing * (columns - 1))) / columns;
        float cellH = cellW * 1.2f; // ارتفاع أكبر شوي
        grid.cellSize = new Vector2(cellW, cellH);

        foreach (char c in keys)
        {
            GameObject btnObj = Instantiate(buttonPrefab, keyboardPanel);
            TMP_Text txt = btnObj.GetComponentInChildren<TMP_Text>();
            txt.text = c.ToString();

            Button btn = btnObj.GetComponent<Button>();
            string letter = c.ToString();
            btn.onClick.AddListener(() => virtualKeyboard.Append(letter));
        }

        // زر المسافة
        GameObject spaceBtn = Instantiate(buttonPrefab, keyboardPanel);
        spaceBtn.GetComponentInChildren<TMP_Text>().text = "Space";
        spaceBtn.GetComponent<Button>().onClick.AddListener(() => virtualKeyboard.Space());

        // زر المسح
        GameObject backBtn = Instantiate(buttonPrefab, keyboardPanel);
        backBtn.GetComponentInChildren<TMP_Text>().text = "⌫";
        backBtn.GetComponent<Button>().onClick.AddListener(() => virtualKeyboard.Backspace());

        // زر الإدخال
        GameObject enterBtn = Instantiate(buttonPrefab, keyboardPanel);
        enterBtn.GetComponentInChildren<TMP_Text>().text = "Enter";
        enterBtn.GetComponent<Button>().onClick.AddListener(() => virtualKeyboard.Submit());
    }
}
