using System.Collections;
using UnityEngine;

public class BarrierModule : ModuleBase
{
   [SerializeField] private ParticleSystem barrierParticles;
   [SerializeField] private CircleCollider2D barrierCollider;
   [SerializeField] private long barrierEnergy;
   [SerializeField] private long barrierMaxEnergy;
   [SerializeField] private int barrierRechargeTime;
   [SerializeField] private int barrierRegenerationAmount;
   [SerializeField] private int barrierRegenerationTickTimer;
   
   
   private bool _isRecharging = false;
   private bool _enabled = false;
   private void Start()
   {
      Upgrades[0] = 1;
      IsUnlocked = true;
      IsActive = true;
      if (IsUnlocked && IsActive)
      {
         barrierCollider.enabled = true;
         barrierParticles.Play();
         _enabled = true;
         if(Upgrades[0] == 1)
            StartCoroutine(Regen());
      }
      else
      {
         _enabled = false;
         barrierCollider.enabled = false;
         barrierParticles.Stop();
      }
   }

   void Update()
   {
      if (IsUnlocked && IsActive)
      {
         if (barrierEnergy == 0)
         {
               if (!_isRecharging)
               {
                  StopAllCoroutines();
                  barrierCollider.enabled = false;
                  barrierParticles.Stop();
                  _enabled = false;
                  _isRecharging = true;
                  Invoke("RechargeBarrier", barrierRechargeTime);
               }
         }
      
         if (barrierEnergy == barrierMaxEnergy && !_enabled)
         {
            barrierCollider.enabled = true;
            barrierParticles.Play();
            _enabled = true;
            StartCoroutine(Regen());
         }
      }
  
   }

   private void RechargeBarrier()
   {
      barrierEnergy = barrierMaxEnergy;
   }

   IEnumerator Regen()
   {
      while (true)
      {
         yield return new WaitForSeconds(barrierRegenerationTickTimer);
         AlterBarrierEnergy(1, false);
      }
   }
   
   public void AlterBarrierEnergy(long amount, bool isDamage)
   {
      if (isDamage)
      {
         barrierEnergy -= amount;
      }
      else
      {
         barrierEnergy += amount;
      }
      if (barrierEnergy > barrierMaxEnergy)
      {
         barrierEnergy = barrierMaxEnergy;
      }
      if (barrierEnergy <= 0)
      {
         barrierEnergy = 0;
      }
   }
}
