using UnityEngine;

public class DefuseSounds : MonoBehaviour
{
    public AudioSource successSFX;
    public AudioSource failSFX;
    public AudioSource tickingSFX;

    public void PlaySuccess() { if (successSFX) successSFX.Play(); }
    public void PlayFail() { if (failSFX) failSFX.Play(); }
    public void PlayTicking(bool play)
    {
        if (!tickingSFX) return;
        if (play && !tickingSFX.isPlaying) tickingSFX.Play();
        if (!play && tickingSFX.isPlaying) tickingSFX.Stop();
    }
}