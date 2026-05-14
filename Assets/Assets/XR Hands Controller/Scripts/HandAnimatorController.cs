
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimatorController : MonoBehaviour
{
    [Header("Input (اسحبيها من Action Based Controller)")]
    public InputActionProperty pinchInput;   // عادة = Activate Action Value (Trigger)
    public InputActionProperty grabInput;    // عادة = Select Action Value (Grip)

    [Range(0f, 2f)] public float grabMultiplier = 1f;

    Animator anim;
    static readonly int HashPinch = Animator.StringToHash("Pinch");
    static readonly int HashGrab  = Animator.StringToHash("Grab");

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (pinchInput.action != null) pinchInput.action.Enable();
        if (grabInput.action  != null) grabInput.action.Enable();
    }

    void OnDisable()
    {
        if (pinchInput.action != null) pinchInput.action.Disable();
        if (grabInput.action  != null) grabInput.action.Disable();
    }

    void Update()
    {
        float pinch = (pinchInput.action != null) ? pinchInput.action.ReadValue<float>() : 0f;
        float grab  = (grabInput.action  != null) ? grabInput.action.ReadValue<float>()  : 0f;

        anim.SetFloat(HashPinch, pinch);
        anim.SetFloat(HashGrab,  grab * grabMultiplier);
    }
}
