
using UnityEngine;
public class DebugHotkeys : MonoBehaviour
{
    public GameLogic logic;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) logic?.Defuse();
        if (Input.GetKeyDown(KeyCode.F2)) logic?.Explode();
    }
}
