using UnityEngine;
using System.Collections;

public class WireCut : MonoBehaviour
{
    public bool isSafeWire;

    private bool alreadyCut = false;
    private static bool anyWireCut = false;

    [Header("Cut Animation")]
    public float shrinkSpeed = 8f;
    public float cutDuration = 0.15f;

    [Header("Effects")]
    public ParticleSystem cutSpark;
    public AudioSource cutSound;
    public AudioSource sparkSound;

    void OnEnable()
    {
        alreadyCut = false;
        anyWireCut = false;
    }

    public void CutWire()
    {
        if (alreadyCut) return;
        if (anyWireCut) return;

        alreadyCut = true;
        anyWireCut = true;

        if (cutSound != null)
            cutSound.Play();

        if (sparkSound != null)
            sparkSound.Play();

        if (cutSpark != null)
        {
            cutSpark.transform.position =
                transform.position + transform.up * 0.02f;

            cutSpark.Play();
        }

        StartCoroutine(CutAnimation());

        AdaptiveTrainingManager adaptive =
            FindObjectOfType<AdaptiveTrainingManager>();

        if (adaptive == null) return;

        if (isSafeWire)
            adaptive.RegisterCorrectCut();
        else
            adaptive.RegisterWrongCut();
    }

    IEnumerator CutAnimation()
    {
        Vector3 startScale = transform.localScale;
        float timer = 0f;

        while (timer < cutDuration)
        {
            timer += Time.deltaTime;

            transform.localScale =
                Vector3.Lerp(startScale, Vector3.zero, timer / cutDuration);

            yield return null;
        }

        gameObject.SetActive(false);
    }
}