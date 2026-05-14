using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public CanvasGroup introGroup;
    public GameObject startPanel;
    public AudioSource startupSound;

    public float showTime = 2f;
    public float fadeTime = 1f;

    void Start()
    {
        if (startPanel != null)
            startPanel.SetActive(false);

        if (startupSound != null)
            startupSound.Play();

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        introGroup.alpha = 1;

        yield return new WaitForSeconds(showTime);

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            introGroup.alpha = Mathf.Lerp(1, 0, t / fadeTime);
            yield return null;
        }

        if (startPanel != null)
            startPanel.SetActive(true);

        gameObject.SetActive(false);
    }
}