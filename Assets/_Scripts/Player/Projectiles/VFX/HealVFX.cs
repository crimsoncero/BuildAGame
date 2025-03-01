using System.Collections.Generic;
using UnityEngine;

public class HealVFX : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _healingEffects;

    public void Play()
    {
        foreach (var effect in _healingEffects)
        {
            effect.Play();
        }
    }
    
    public void Stop()
    {
        foreach (var effect in _healingEffects)
        {
            effect.Stop();
        }
    }
    
}
