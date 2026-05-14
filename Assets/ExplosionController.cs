using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
    public ParticleSystem vfx;
    public AudioSource sfx;
    public AudioClip explosionClip;
    public Transform cameraRig;  // XR Origin or Camera parent
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.05f;

    public void Play()
    {
        if (vfx) vfx.Play();
        if (sfx && explosionClip) sfx.PlayOneShot(explosionClip);
        if (cameraRig) StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 start = cameraRig.localPosition;
        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            cameraRig.localPosition = start + Random.insideUnitSphere * shakeMagnitude;
            yield return null;
        }
        cameraRig.localPosition = start;
    }
}
