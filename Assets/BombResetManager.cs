using System.Collections.Generic;
using UnityEngine;

public class BombResetManager : MonoBehaviour
{
    public List<ResettablePart> parts = new List<ResettablePart>();

    public void ResetAll()
    {
        foreach (var p in parts)
            if (p) p.ResetPart();
    }
}