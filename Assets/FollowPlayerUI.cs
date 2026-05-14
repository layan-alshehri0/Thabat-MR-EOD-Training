using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 1.8f;
    public float heightOffset = -0.15f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (playerCamera == null) return;

        Vector3 targetPosition = playerCamera.position + playerCamera.forward * distance;
        targetPosition.y = playerCamera.position.y + heightOffset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
    }
}