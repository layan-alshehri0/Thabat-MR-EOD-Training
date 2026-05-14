using UnityEngine;

public class BombWire : MonoBehaviour
{
    public HybridHandAI handAI;

    [Header("Correct Wire")]
    public bool isSafeWire;

    private bool isCut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCut) return;
        if (handAI != null && handAI.missionEnded) return;

        if (other.CompareTag("Cutter"))
        {
            TryCut();
        }
    }

    void TryCut()
    {
        if (isCut) return;

        if (handAI == null)
        {
            Debug.Log("AI not assigned");
            return;
        }

        isCut = true;

        if (isSafeWire)
        {
            handAI.EndMissionSuccess("Correct wire cut - تم قص السلك الصحيح");
        }
        else
        {
            handAI.EndMissionFail("Wrong wire cut - تم قص السلك الخطأ");
        }

        gameObject.SetActive(false);
    }
}