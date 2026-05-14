using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialDisassembly : MonoBehaviour
{
    [System.Serializable]
    public class Part
    {
        public GameObject obj;          
        public Vector3 moveOffset = new Vector3(0, 1, 0); 
        public float moveDuration = 1f; 
        public float waitAfter = 0.5f;  

        [Header("Optional Sound")]
        public AudioClip detachSound;   // صوت التفكيك الخاص بالقطعة
    }

    public List<Part> parts = new List<Part>();
    public AudioSource audioSource; // مصدر الصوت العام

    public void StartDisassembly()
    {
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        foreach (var p in parts)
        {
            if (p.obj != null)
                yield return StartCoroutine(MoveAndDisappear(p));
        }
    }

    IEnumerator MoveAndDisappear(Part p)
    {
        // 🎵 تشغيل الصوت
        if (audioSource != null && p.detachSound != null)
        {
            audioSource.PlayOneShot(p.detachSound);
        }

        Vector3 startPos = p.obj.transform.position;
        Vector3 targetPos = startPos + p.moveOffset;

        float t = 0;
        Renderer r = p.obj.GetComponent<Renderer>();
        List<Material> mats = new List<Material>();

        if (r != null)
            mats.AddRange(r.materials);

        while (t < 1)
        {
            t += Time.deltaTime / p.moveDuration;
            p.obj.transform.position = Vector3.Lerp(startPos, targetPos, t);

            foreach (var m in mats)
            {
                Color c = m.color;
                c.a = Mathf.Lerp(1f, 0f, t);
                m.color = c;
            }

            yield return null;
        }

        p.obj.SetActive(false);
        yield return new WaitForSeconds(p.waitAfter);
    }
}
