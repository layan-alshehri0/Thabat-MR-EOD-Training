using UnityEngine;

public class FailSoundExplosion : MonoBehaviour
{
    [Header("مراجع")]
    public AudioSource failAudio;          // حط هنا AudioSource حق صوت الفشل
    public AutoExplosion explosion;        // حط هنا سكربت الانفجار

    private bool triggered = false;

    void Update()
    {
        if (!triggered && failAudio != null && failAudio.isPlaying)
        {
            triggered = true;
            if (explosion != null)
            {
                explosion.Explode();
            }
        }
    }
}