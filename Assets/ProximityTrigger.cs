using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public UI_Manager uiManager;
    public string playerTag = "Player";
    public bool once = true;
    bool fired;

    void Reset()
    {
        // يضمن أن التريغر مضبوط بسرعة
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;

        var rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (once && fired) return;

        fired = true;
        if (uiManager)
            uiManager.SendMessage("ShowInstruction", SendMessageOptions.DontRequireReceiver);
    }
}
