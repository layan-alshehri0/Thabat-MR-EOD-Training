using UnityEngine;

public class WireCutterZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TryCut(other);
    }

    void TryCut(Collider other)
    {
        if (BombResult.Instance != null && BombResult.Instance.gameEnded)
            return;

        WireCut wire = other.GetComponent<WireCut>();

        if (wire == null)
            wire = other.GetComponentInParent<WireCut>();

        if (wire != null)
            wire.CutWire();
    }
}