using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonFeedback : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Target")]
    public Image targetGraphic;              // صورة خلفية الزر (Image)
    
    [Header("Colors")]
    public Color normalColor = new Color(1f, 0.65f, 0.3f, 1f);   // #FFA64D تقريبًا
    public Color highlightedColor = new Color(1f, 0.75f, 0.45f, 1f); // أفتح شوي
    public Color pressedColor = new Color(0.8f, 0.48f, 0f, 1f);  // أغمق (#CC7A00 تقريبًا)
    public float fadeDuration = 0.08f;       // سرعة الانتقال بين الألوان

    [Header("Click Sound")]
    public AudioSource audioSource;          // AudioSource على نفس الزر أو طفل له
    public AudioClip clickClip;              // ملف الصوت (wav/mp3)
    [Range(0f,1f)] public float clickVolume = 1f;

    [Header("Optional: Press Scale Punch")]
    public bool useScalePunch = true;
    public float pressedScale = 0.93f;       // انكماش بسيط وقت الضغط
    public float scaleReturnSpeed = 20f;

    Vector3 _originalScale;
    Color _currentTargetColor;
    bool _isPressed;

    void Reset()
    {
        // محاولة تعبئة الحقول تلقائيًا
        targetGraphic = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        if (targetGraphic == null) targetGraphic = GetComponent<Image>();
        if (targetGraphic != null) targetGraphic.color = normalColor;

        _originalScale = transform.localScale;
        _currentTargetColor = normalColor;
    }

    void Update()
    {
        // مزج ناعم للّون
        if (targetGraphic != null)
        {
            targetGraphic.color = Color.Lerp(targetGraphic.color, _currentTargetColor, Time.deltaTime / Mathf.Max(0.0001f, fadeDuration));
        }

        // رجوع ملس للسكيل بعد رفع الضغط
        if (useScalePunch && !_isPressed)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, _originalScale, Time.deltaTime * scaleReturnSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isPressed) return;
        _currentTargetColor = highlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPressed) return;
        _currentTargetColor = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        _currentTargetColor = pressedColor;
        if (useScalePunch) transform.localScale = _originalScale * pressedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
        _currentTargetColor = highlightedColor; // يرجع لحالة hover لو كان المؤشر فوقه
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // تشغيل صوت الكليك
        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip, clickVolume);
    }
}
