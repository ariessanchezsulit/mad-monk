using UnityEngine;
using System.Collections;

public class GestureEffect : MonoBehaviour {
    public Color tint;

    public void UpdateColor()
    {
        var systems = GetComponentsInChildren<ParticleSystem>();

        foreach(var system in systems)
        {
            system.startColor = tint;
        }
    }
}
