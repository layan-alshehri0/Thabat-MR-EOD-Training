using UnityEngine;

public class bomb : MonoBehaviour
{
    public Transform bomb1;        // القنبلة
    public AudioSource beepSound; // صوت البيب
    public float maxDistance = 10f;  // أبعد مسافة للكشف
    private float nextBeepTime = 0f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, bomb1.position);

        if (distance <= maxDistance)
        {
            // نحسب الفترة بين الأصوات: كل ما قرب، البيب أسرع
            float beepInterval = Mathf.Lerp(0.1f, 1f, distance / maxDistance);

            if (Time.time >= nextBeepTime)
            {
                beepSound.PlayOneShot(beepSound.clip);
                nextBeepTime = Time.time + beepInterval;
            }
        }
    }
}