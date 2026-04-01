using UnityEngine;

public class BarrierModule : ModuleBase
{
   [SerializeField] private ParticleSystem barrierParticles;
   [SerializeField] private CircleCollider2D barrierCollider;
   [SerializeField] private long barrierEnergy;
   [SerializeField] private long barrierMaxEnergy;
   [SerializeField] private int barrierRechargeTime;
   
   private bool _isRecharging = false;
   private bool _enabled = false;
   private void Start()
   {
      IsUnlocked = true;
      IsActive = true;
      if (IsUnlocked && IsActive)
      {
         barrierCollider.enabled = true;
         barrierParticles.Play();
         _enabled = true;
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
         }
      }
  
   }

   private void RechargeBarrier()
   {
      barrierEnergy = barrierMaxEnergy;
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
