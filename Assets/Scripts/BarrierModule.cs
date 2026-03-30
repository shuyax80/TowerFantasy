using System;
using UnityEngine;

public class BarrierModule : ModuleBase
{
   [SerializeField] private ParticleSystem barrierParticles;

   private void Start()
   {
      IsUnlocked = true;
      IsActive = true;
      if (IsUnlocked && IsActive)
      {
         barrierParticles.Play();
      }
      else
      {
         barrierParticles.Stop();
      }
   }
}
