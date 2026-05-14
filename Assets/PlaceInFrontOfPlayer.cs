using UnityEngine;

public class PlaceInFrontOfPlayer : MonoBehaviour
{
    public Transform playerCamera;
    public float distance = 1.5f;
    public float heightOffset = 0f;

    public void PlaceNow()
    {
        if (playerCamera == null) return;

        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        transform.position =
            playerCamera.position +
            forward * distance +
            Vector3.up * heightOffset;

        transform.rotation =
            Quaternion.LookRotation(forward);
    }
}