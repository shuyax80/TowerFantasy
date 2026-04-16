using System;
using UnityEngine;

public class DistortionFieldModule : ModuleBase
{
    [SerializeField] private ParticleSystem distortionFieldEffect;
    [SerializeField] private BoxCollider2D distortionFieldCollider;
    void Start()
    {
        IsUnlocked = true;   
        IsActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Distortion Field Triggered");
    }
}
