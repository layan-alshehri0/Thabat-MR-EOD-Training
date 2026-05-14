// using UnityEngine;
// using UnityEngine.XR;

// public class TerrainDigVR : MonoBehaviour
// {
//     public Terrain terrain;
//     private TerrainData terrainData;

//     public float digRadius = 0.2f;     // نصف قطر الحفر بالمتر
//     public float digDepth = 0.001f;    // عمق الحفر
//     public float forwardOffset = 0.3f; // كم متر قدام المجرفة نحفر

//     public Transform shovelTip;
//     public XRNode controllerNode = XRNode.RightHand;


  

//     void Start()
//     {
//         terrainData = terrain.terrainData;
//     }

//     void Update()
//     {
//         InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
//         bool triggerPressed;
//         if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
//         {
//             // نحسب نقطة الحفر قدام المجرفة
//             Vector3 digPoint = shovelTip.position + shovelTip.forward * forwardOffset;

//             RaycastHit hit;
//             if (Physics.Raycast(digPoint, Vector3.down, out hit, 0.5f))
//             {
//                 if (hit.collider.gameObject == terrain.gameObject)
//                 {
//                     DigAtPoint(hit.point);
//                 }
//             }
//         }
//     }

//     void DigAtPoint(Vector3 point)
//     {
//         int xRes = terrainData.heightmapResolution;
//         int yRes = terrainData.heightmapResolution;

//         int x = (int)((point.x / terrainData.size.x) * xRes);
//         int y = (int)((point.z / terrainData.size.z) * yRes);

//         // نحسب كم نقطة لكل متر
//         float metersPerPoint = terrainData.size.x / (float)xRes;
//         int radius = Mathf.Max(1, Mathf.RoundToInt(digRadius / metersPerPoint)); // ضمان الحفر حتى لو صغير

//         int startX = Mathf.Clamp(x - radius, 0, xRes - 1);
//         int startY = Mathf.Clamp(y - radius, 0, yRes - 1);
//         int width = Mathf.Clamp(radius * 2, 1, xRes - startX);
//         int height = Mathf.Clamp(radius * 2, 1, yRes - startY);

//         float[,] heights = terrainData.GetHeights(startX, startY, width, height);

//         for (int i = 0; i < width; i++)
//         {
//             for (int j = 0; j < height; j++)
//             {
//                 heights[j, i] -= digDepth;
//                 if (heights[j, i] < 0) heights[j, i] = 0;
//             }
//         }

//         terrainData.SetHeights(startX, startY, heights);
      
//     }
// }


// using UnityEngine;
// using UnityEngine.XR;

// public class TerrainDigVR : MonoBehaviour
// {
//     public Terrain terrain;
//     private TerrainData terrainData;

//     public float digRadius = 0.2f;     // نصف قطر الحفر بالمتر
//     public float digDepth = 0.001f;    // عمق الحفر
//     public float forwardOffset = 0.3f; // كم متر قدام المجرفة نحفر

//     public Transform shovelTip;
//     public XRNode controllerNode = XRNode.RightHand;

//     public AudioSource audio;



//     void Start()
//     {
//         terrainData = terrain.terrainData;
//     }

//     void Update()
//     {
//         InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
//         bool triggerPressed;
//         if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
//         {
//             // نحسب نقطة الحفر قدام المجرفة
//             Vector3 digPoint = shovelTip.position + shovelTip.forward * forwardOffset;

//             RaycastHit hit;
//             if (Physics.Raycast(digPoint, Vector3.down, out hit, 0.5f))
//             {
//                 if (hit.collider.gameObject == terrain.gameObject)
//                 {
//                     DigAtPoint(hit.point);
                    
//                 }
//             }
//         }
//     }

//     void DigAtPoint(Vector3 point)
//     {
//         int xRes = terrainData.heightmapResolution;
//         int yRes = terrainData.heightmapResolution;

//         int x = (int)((point.x / terrainData.size.x) * xRes);
//         int y = (int)((point.z / terrainData.size.z) * yRes);

//         // نحسب كم نقطة لكل متر
//         float metersPerPoint = terrainData.size.x / (float)xRes;
//         int radius = Mathf.Max(1, Mathf.RoundToInt(digRadius / metersPerPoint)); // ضمان الحفر حتى لو صغير

//         int startX = Mathf.Clamp(x - radius, 0, xRes - 1);
//         int startY = Mathf.Clamp(y - radius, 0, yRes - 1);
//         int width = Mathf.Clamp(radius * 2, 1, xRes - startX);
//         int height = Mathf.Clamp(radius * 2, 1, yRes - startY);

//         float[,] heights = terrainData.GetHeights(startX, startY, width, height);

//         for (int i = 0; i < width; i++)
//         {
//             for (int j = 0; j < height; j++)
//             {
//                 heights[j, i] -= digDepth;
//                 if (heights[j, i] < 0) heights[j, i] = 0;
//             }
//         }

//         terrainData.SetHeights(startX, startY, heights);

//         if (audio != null && !audio.isPlaying)
//             audio.Play();
//     }

// }

// using UnityEngine;
// using UnityEngine.XR;

// public class TerrainDigVR : MonoBehaviour
// {
//     public Terrain terrain;
//     private TerrainData terrainData;

//     public float digRadius = 0.2f;     // نصف قطر الحفر بالمتر
//     public float digDepth = 0.001f;    // عمق الحفر
//     public float forwardOffset = 0.3f; // كم متر قدام المجرفة نحفر

//     public Transform shovelTip;
//     public XRNode controllerNode = XRNode.RightHand;

//     void Start()
//     {
//         terrainData = terrain.terrainData;
//     }

//     void Update()
//     {
//         InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
//         bool triggerPressed;
//         if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
//         {
//             Vector3 digPoint = shovelTip.position + shovelTip.forward * forwardOffset;
//             RaycastHit hit;
//             if (Physics.Raycast(digPoint, Vector3.down, out hit, 0.5f, ~0))
//             {
//                 if (hit.collider.gameObject == terrain.gameObject)
//                 {
//                     DigAtPoint(hit.point);
//                 }
//             }
//         }
//     }

//     void DigAtPoint(Vector3 point)
//     {
//         int xRes = terrainData.heightmapResolution;
//         int yRes = terrainData.heightmapResolution;

//         int x = (int)((point.x / terrainData.size.x) * xRes);
//         int y = (int)((point.z / terrainData.size.z) * yRes);

//         float metersPerPoint = terrainData.size.x / (float)xRes;
//         int radius = Mathf.Max(1, Mathf.RoundToInt(digRadius / metersPerPoint));

//         int startX = Mathf.Clamp(x - radius, 0, xRes - 1);
//         int startY = Mathf.Clamp(y - radius, 0, yRes - 1);
//         int width = Mathf.Clamp(radius * 2, 1, xRes - startX);
//         int height = Mathf.Clamp(radius * 2, 1, yRes - startY);

//         float[,] heights = terrainData.GetHeights(startX, startY, width, height);

//         for (int i = 0; i < width; i++)
//         {
//             for (int j = 0; j < height; j++)
//             {
//                 heights[j, i] -= digDepth;
//                 if (heights[j, i] < 0) heights[j, i] = 0;
//             }
//         }

//         terrainData.SetHeights(startX, startY, heights);
//     }
// // }
// using UnityEngine;
// using UnityEngine.XR;

// public class TerrainDigVR : MonoBehaviour
// {
//     public Terrain terrain;
//     private TerrainData terrainData;

//     [Header("Dig Settings")]
//     [Tooltip("نصف قطر الحفر بالأمتار (صغير = 0.05 - 0.12)")]
//     public float digRadius = 0.08f;         // أصغر من قبل
//     [Tooltip("عمق الحفر لكل ضغطة")]
//     public float digDepthPerPress = 0.002f; // عمق لكل ضغطة
//     [Tooltip("مهلة بين الضغطات (ثانية)")]
//     public float minPressInterval = 0.15f;  // منع الحفر المتكرر في نفس الإطار

//     [Tooltip("مضاعف نعومة الحواف (1=نعم، أعلى=أنعم)")]
//     public float edgeSoftness = 1.5f;       // نعومة الحواف (falloff)

//     [Header("Aim Mode")]
//     public AimMode aimMode = AimMode.UserForward;

//     [Tooltip("حين يكون وضع ShovelTip نشط")]
//     public Transform shovelTip;
//     [Tooltip("كم متر قدام المجرفة نحفر")]
//     public float forwardOffsetShovel = 0.25f;

//     [Tooltip("حين يكون وضع UserForward نشط")]
//     public Transform userHead;              // كاميرا XR (الرأس)
//     [Tooltip("المسافة قدام المستخدم (أفقياً)")]
//     public float userForwardOffset = 0.8f;  // يحفر قدّام اللاعب
//     [Tooltip("ارتفاع بسيط لنقطة الراي كاست")]
//     public float userRaycastLift = 0.25f;

//     [Header("XR Input")]
//     public XRNode controllerNode = XRNode.RightHand;
//     [Range(0f,1f)]
//     public float triggerThreshold = 0.6f;   // عتبة اعتِبار الضغطة

//     [Header("SFX")]
//     public AudioSource audio;

//     // internal
//     private bool wasPressedLastFrame = false;
//     private float lastPressTime = -999f;

//     public enum AimMode { ShovelTip, UserForward }

//     void Start()
//     {
//         terrainData = terrain.terrainData;
//     }

//     void Update()
//     {
//         if (terrainData == null || terrain == null) return;

//         // قراءة الزناد كقيمة تناظرية (أفضل من triggerButton)
//         InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
//         if (!device.isValid) return;

//         float triggerValue = 0f;
//         device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);

//         bool pressedNow = triggerValue >= triggerThreshold;
//         bool isPressDown = pressedNow && !wasPressedLastFrame;

//         if (isPressDown && Time.time - lastPressTime >= minPressInterval)
//         {
//             // حدد مركز الحفر بناءً على الوضع المختار
//             Vector3 digOrigin;
//             Vector3 rayDir;
//             if (aimMode == AimMode.ShovelTip && shovelTip != null)
//             {
//                 digOrigin = shovelTip.position + shovelTip.forward * forwardOffsetShovel;
//                 rayDir = Vector3.down;
//             }
//             else
//             {
//                 Transform head = userHead != null ? userHead : Camera.main != null ? Camera.main.transform : null;
//                 if (head == null) { wasPressedLastFrame = pressedNow; return; }

//                 Vector3 fwdOnPlane = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
//                 if (fwdOnPlane.sqrMagnitude < 1e-4f) fwdOnPlane = head.forward; // احتياط

//                 digOrigin = head.position + fwdOnPlane * userForwardOffset;
//                 digOrigin.y += userRaycastLift; // نرفع قليلًا
//                 rayDir = Vector3.down;
//             }

//             // راي كاست نحو الأرض
//             if (Physics.Raycast(digOrigin, rayDir, out RaycastHit hit, 3f))
//             {
//                 if (hit.collider.gameObject == terrain.gameObject)
//                 {
//                     DigAtPoint(hit.point);
//                     if (audio != null) audio.Play();
//                     lastPressTime = Time.time;
//                 }
//             }
//         }

//         wasPressedLastFrame = pressedNow;
//     }

//     void DigAtPoint(Vector3 worldPoint)
//     {
//         int xRes = terrainData.heightmapResolution;
//         int yRes = terrainData.heightmapResolution;

//         // تحويل نقطة العالم إلى إحداثيات heightmap
//         Vector3 tPos = worldPoint - terrain.transform.position;
//         int cx = Mathf.RoundToInt((tPos.x / terrainData.size.x) * (xRes - 1));
//         int cy = Mathf.RoundToInt((tPos.z / terrainData.size.z) * (yRes - 1));

//         // كم متر لكل نقطة واحدة
//         float metersPerPointX = terrainData.size.x / (float)(xRes - 1);
//         float metersPerPointY = terrainData.size.z / (float)(yRes - 1);
//         float metersPerPoint = Mathf.Max(metersPerPointX, metersPerPointY);

//         // نصف القطر بالنقاط (على الأقل 1)
//         int rPx = Mathf.Max(1, Mathf.RoundToInt(digRadius / metersPerPoint));

//         int startX = Mathf.Clamp(cx - rPx, 0, xRes - 1);
//         int startY = Mathf.Clamp(cy - rPx, 0, yRes - 1);
//         int endX   = Mathf.Clamp(cx + rPx, 0, xRes - 1);
//         int endY   = Mathf.Clamp(cy + rPx, 0, yRes - 1);

//         int w = endX - startX + 1;
//         int h = endY - startY + 1;

//         float[,] heights = terrainData.GetHeights(startX, startY, w, h);

//         // حفر بدائرة مع falloff ناعم
//         for (int j = 0; j < h; j++)
//         {
//             for (int i = 0; i < w; i++)
//             {
//                 int hx = startX + i;
//                 int hy = startY + j;

//                 float dx = (hx - cx);
//                 float dy = (hy - cy);
//                 float dist = Mathf.Sqrt(dx * dx + dy * dy);

//                 if (dist <= rPx)
//                 {
//                     // نسبة القرب من المركز (0 عند الحافة، 1 عند المركز)
//                     float t = 1f - Mathf.Clamp01(dist / rPx);
//                     // falloff ناعم (قوة أس)
//                     float fall = Mathf.Pow(t, edgeSoftness);

//                     heights[j, i] = Mathf.Max(0f, heights[j, i] - digDepthPerPress * fall);
//                 }
//             }
//         }

//         terrainData.SetHeights(startX, startY, heights);
//     }

//     // رسومات مساعدة في المشهد (اختياري)
//     void OnDrawGizmosSelected()
//     {
//         if (aimMode == AimMode.ShovelTip && shovelTip != null)
//         {
//             Gizmos.color = Color.yellow;
//             Gizmos.DrawLine(shovelTip.position, shovelTip.position + shovelTip.forward * forwardOffsetShovel);
//         }
//         else if (aimMode == AimMode.UserForward)
//         {
//             Transform head = userHead != null ? userHead : Camera.main != null ? Camera.main.transform : null;
//             if (head != null)
//             {
//                 Vector3 fwdOnPlane = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
//                 Vector3 p = head.position + fwdOnPlane * userForwardOffset;
//                 p.y += userRaycastLift;
//                 Gizmos.color = Color.cyan;
//                 Gizmos.DrawLine(head.position, p);
//                 Gizmos.DrawSphere(p, 0.02f);
//             }
//         }
//     }
// }
using UnityEngine;
using UnityEngine.XR;

public class TerrainDigVR : MonoBehaviour
{
    public Terrain terrain;
    private TerrainData terrainData;

    [Header("Dig Settings")]
    public float digRadius = 0.08f;         
    public float digDepthPerPress = 0.002f; 
    public float minPressInterval = 0.15f;  
    public float edgeSoftness = 1.5f;       

    [Header("Aim Mode")]
    public AimMode aimMode = AimMode.ShovelTip;

    [Tooltip("حين يكون وضع ShovelTip نشط")]
    public Transform shovelTip;
    [Tooltip("كم متر قدام المجرفة نحفر")]
    public float forwardOffsetShovel = 0.0f;

    [Tooltip("حين يكون وضع UserForward نشط")]
    public Transform userHead;              
    public float userForwardOffset = 0.8f;  
    public float userRaycastLift = 0.25f;   

    [Header("XR Input")]
    public XRNode controllerNode = XRNode.RightHand;
    [Range(0f,1f)]
    public float triggerThreshold = 0.6f;   

    [Header("FX")]
    public AudioSource Audio;
    public GameObject digDustPrefab;   // سحبي هنا الـPrefab حق الغبار

    // internal
    private bool wasPressedLastFrame = false;
    private float lastPressTime = -999f;

    public enum AimMode { ShovelTip, UserForward }

    void Start()
    {
        terrainData = terrain.terrainData;
    }

    void Update()
    {
        if (terrainData == null || terrain == null) return;

        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (!device.isValid) return;

        float triggerValue = 0f;
        device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);

        bool pressedNow = triggerValue >= triggerThreshold;
        bool isPressDown = pressedNow && !wasPressedLastFrame;

        if (isPressDown && Time.time - lastPressTime >= minPressInterval)
        {
            Vector3 digOrigin;
            Vector3 rayDir;

            if (aimMode == AimMode.ShovelTip && shovelTip != null)
            {
                digOrigin = shovelTip.position + shovelTip.forward * forwardOffsetShovel;
                rayDir = Vector3.down;
            }
            else
            {
                Transform head = userHead != null ? userHead : Camera.main != null ? Camera.main.transform : null;
                if (head == null) { wasPressedLastFrame = pressedNow; return; }

                Vector3 fwdOnPlane = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
                if (fwdOnPlane.sqrMagnitude < 1e-4f) fwdOnPlane = head.forward;

                digOrigin = head.position + fwdOnPlane * userForwardOffset;
                digOrigin.y += userRaycastLift; 
                rayDir = Vector3.down;
            }

            if (Physics.Raycast(digOrigin, rayDir, out RaycastHit hit, 3f))
{
    if (hit.collider.gameObject == terrain.gameObject)
    {
        // 🔹 شرط إضافي: لازم المجرفة قريبة من مكان الحفر
        if (aimMode == AimMode.ShovelTip && shovelTip != null)
        {
            float dist = Vector3.Distance(shovelTip.position, hit.point);
            if (dist > 0.15f)  // إذا أبعد من 15 سم = لا يحفر ولا يطلع غبار
                return;
        }

        DigAtPoint(hit.point);

        if (Audio != null) Audio.Play();

        if (digDustPrefab != null)
        {
            GameObject dust = Instantiate(digDustPrefab, hit.point, Quaternion.identity);
            Destroy(dust, 3f);
        }

        lastPressTime = Time.time;
    }
}

        }

        wasPressedLastFrame = pressedNow;
    }

    void DigAtPoint(Vector3 worldPoint)
    {
        int xRes = terrainData.heightmapResolution;
        int yRes = terrainData.heightmapResolution;

        Vector3 tPos = worldPoint - terrain.transform.position;
        int cx = Mathf.RoundToInt((tPos.x / terrainData.size.x) * (xRes - 1));
        int cy = Mathf.RoundToInt((tPos.z / terrainData.size.z) * (yRes - 1));

        float metersPerPointX = terrainData.size.x / (float)(xRes - 1);
        float metersPerPointY = terrainData.size.z / (float)(yRes - 1);
        float metersPerPoint = Mathf.Max(metersPerPointX, metersPerPointY);

        int rPx = Mathf.Max(1, Mathf.RoundToInt(digRadius / metersPerPoint));

        int startX = Mathf.Clamp(cx - rPx, 0, xRes - 1);
        int startY = Mathf.Clamp(cy - rPx, 0, yRes - 1);
        int endX   = Mathf.Clamp(cx + rPx, 0, xRes - 1);
        int endY   = Mathf.Clamp(cy + rPx, 0, yRes - 1);

        int w = endX - startX + 1;
        int h = endY - startY + 1;

        float[,] heights = terrainData.GetHeights(startX, startY, w, h);

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                int hx = startX + i;
                int hy = startY + j;

                float dx = (hx - cx);
                float dy = (hy - cy);
                float dist = Mathf.Sqrt(dx * dx + dy * dy);

                if (dist <= rPx)
                {
                    float t = 1f - Mathf.Clamp01(dist / rPx);
                    float fall = Mathf.Pow(t, edgeSoftness);

                    heights[j, i] = Mathf.Max(0f, heights[j, i] - digDepthPerPress * fall);
                }
            }
        }

        terrainData.SetHeights(startX, startY, heights);
    }
}
