using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequentialDisappear : MonoBehaviour
{
    [System.Serializable]
    public class Piece
    {
        public GameObject part;       // القطعة
        public float disappearTime = 0.5f; // زمن اختفاء القطعة (بالثواني)
    }

    [Header("الترتيب (من الأعلى للأسفل)")]
    public List<Piece> order = new List<Piece>();

    [Header("الزمن بين كل قطعة والثانية")]
    public float interval = 0.5f;

    [Header("بعد الانتهاء")]
    public CanvasGroup panel;           // لوحة التعليمات (اختياري)
    public bool hidePanelAfter = true;  // يخفي اللوحة بعد آخر قطعة

    public void StartSequence()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        foreach (var p in order)
        {
            if (p.part == null) continue;
            StartCoroutine(FadeAndHide(p.part, p.disappearTime));
            yield return new WaitForSeconds(interval);
        }

        // بعد ما تخلص كل القطع
        if (panel && hidePanelAfter)
        {
            panel.interactable = false;
            panel.blocksRaycasts = false;
            panel.alpha = 0f;
            panel.gameObject.SetActive(false);
        }
    }

    IEnumerator FadeAndHide(GameObject obj, float time)
    {
        float t = 0f;
        Vector3 start = obj.transform.localScale;
        Vector3 target = Vector3.zero;

        while (t < 1f)
        {
            t += Time.deltaTime / time;
            obj.transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }

        Destroy(obj);
    }
}