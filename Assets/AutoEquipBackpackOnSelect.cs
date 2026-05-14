using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class AutoEquipBackpackOnSelect : MonoBehaviour
{
    // استخدميه لأي عنصر (شنطة/خوذة/جهاز)
    [Header("Attach to this Slot on XR Rig")]
    public Transform backpackSlot;              // اسحبي السلوْت المناسب (BackpackSlot/HelmetSlot/DeviceSlot)

    [Header("Checklist UI (اختياري)")]
    public GameObject checkmark;                // صورة ✅ (تكون OFF بالبداية)
    public Toggle checklistToggle;              // Toggle (يتحول isOn = true)

    [Header("Audio/VFX (اختياري)")]
    public AudioSource equipSfx;                // صوت التركيب
    public GameObject equipVfx;                 // مؤثر بصري (Prefab Particle) وقت الالتقاط

    [Header("Highlight on pick (اختياري)")]
    public Renderer targetRenderer;             // Renderer للعنصر (لو فاضي بيبحث عن أقرب Renderer)
    public Color flashColor = Color.yellow;     // لون الفلاش
    public float flashSeconds = 0.15f;          // مدة الفلاش
    public bool useEmission = true;             // يفعّل Emission بدلاً من تغيير Color

    [Header("Activation Mode")]
    public bool equipOnSelect = true;           // يلبس عند الضغط/الاختيار (Ray/Direct)
    public bool equipOnHover  = false;          // يلبس بمجرد اللمس (Hover)

    [Header("Trigger Fallback (اختياري)")]
    public bool alsoTriggerEnter = false;       // يلبس إذا دخل Collider بتاج معيّن
    public string triggerTag = "Hand";          // التاج (Hand/Head حسب رغبتك)

    // ---- داخلي ----
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable simple;
    Rigidbody rb;
    Collider[] cols;
    bool equipped;

    MaterialPropertyBlock mpb;
    static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    Color baseColor;
    bool hasBaseColor;

    void Awake()
    {
        grab   = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        simple = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        rb     = GetComponent<Rigidbody>();
        cols   = GetComponentsInChildren<Collider>(true);

        // خليه ثابت قبل اللبس (ما يطيح)
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }

        // حضّر Renderer للفلاش
        if (!targetRenderer) targetRenderer = GetComponentInChildren<Renderer>();
        if (targetRenderer)
        {
            mpb = new MaterialPropertyBlock();
            var mat = targetRenderer.sharedMaterial;
            if (mat && mat.HasProperty("_Color"))
            {
                baseColor = mat.color;
                hasBaseColor = true;
            }
        }
    }

    void OnEnable()
    {
        if (grab != null)
        {
            if (equipOnSelect) grab.selectEntered.AddListener(OnSelect);
            if (equipOnHover)  grab.hoverEntered.AddListener(OnHover);
        }
        if (simple != null)
        {
            if (equipOnSelect) simple.selectEntered.AddListener(OnSelect);
            if (equipOnHover)  simple.hoverEntered.AddListener(OnHover);
        }
    }

    void OnDisable()
    {
        if (grab != null)
        {
            if (equipOnSelect) grab.selectEntered.RemoveListener(OnSelect);
            if (equipOnHover)  grab.hoverEntered.RemoveListener(OnHover);
        }
        if (simple != null)
        {
            if (equipOnSelect) simple.selectEntered.RemoveListener(OnSelect);
            if (equipOnHover)  simple.hoverEntered.RemoveListener(OnHover);
        }
    }

    // للربط اليدوي من الـInspector إذا حبيتي
    public void EquipNow() => TryEquip();

    void OnSelect(SelectEnterEventArgs _) => TryEquip();
    void OnHover (HoverEnterEventArgs  _) => TryEquip();

    void OnTriggerEnter(Collider other)
    {
        if (!alsoTriggerEnter || equipped) return;
        if (other.CompareTag(triggerTag)) TryEquip();
    }

    void TryEquip()
    {
        if (equipped || backpackSlot == null) return;
        equipped = true;

        // قفل فيزياء وتفاعلات بعد اللبس
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }
        if (cols != null) foreach (var c in cols) c.enabled = false;
        if (grab)   grab.enabled = false;
        if (simple) simple.enabled = false;

        // التثبيت على السلوْت
        transform.SetParent(backpackSlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // UI
        if (checkmark)       checkmark.SetActive(true);
        if (checklistToggle) checklistToggle.isOn = true;

        // صوت/مؤثر
        if (equipSfx) equipSfx.Play();
        if (equipVfx) Instantiate(equipVfx, transform.position, transform.rotation);

        // فلاش لون/إيمشن
        if (targetRenderer) StartCoroutine(FlashHighlight());
    }

    System.Collections.IEnumerator FlashHighlight()
    {
        if (useEmission)
        {
            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(EmissionColorID, flashColor);
            targetRenderer.SetPropertyBlock(mpb);
        }
        else if (hasBaseColor)
        {
            var mats = targetRenderer.materials; // instance
            for (int i = 0; i < mats.Length; i++)
                if (mats[i].HasProperty("_Color")) mats[i].color = flashColor;
        }

        yield return new WaitForSeconds(flashSeconds);

        if (useEmission)
        {
            targetRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(EmissionColorID, Color.black);
            targetRenderer.SetPropertyBlock(mpb);
        }
        else if (hasBaseColor)
        {
            var mats = targetRenderer.materials;
            for (int i = 0; i < mats.Length; i++)
                if (mats[i].HasProperty("_Color")) mats[i].color = baseColor;
        }
    }
}
