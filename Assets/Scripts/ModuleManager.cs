using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
   [SerializeField] private List<ModuleBase> modules = new List<ModuleBase>();
   public static ModuleManager Instance { get; set; }
   
   private int _upgradePoints = 0;
   private CannonModule _cannon;
   private BarrierModule _barrier;
   private ArmorModule _armor;
   private DistortionFieldModule _distortionField;
   
   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(this.gameObject);
         return;
      }
      Instance = this;
      _armor = GetComponent<ArmorModule>();
      _barrier = GetComponent<BarrierModule>();
      _distortionField = GetComponent<DistortionFieldModule>();
      _cannon = GetComponent<CannonModule>();
      modules.Add(_armor);
      modules.Add(_barrier);
      modules.Add(_distortionField);
      modules.Add(_cannon);
   }

   public void IncreaseUpgradePoints()
   {
      _upgradePoints++;
   }

   public int ReturnUpgradePoints()
   {
      return _upgradePoints;
   }
   
   public void SpendUpgradePoints()
   {
      _upgradePoints--;
   }

   public List<ModuleBase> ReturnModules()
   {
      return modules;
   }
   
   public long DamagePlayerBarriers(long amount)
   {
      return _barrier.AlterBarrierEnergy(amount, true);
   }

   public void FireCannonHit(GameObject target, long damage)
   {
      if (target.TryGetComponent<Enemy>(out var script))
      {
         _cannon.Shoot(script, damage);
      }
   }
}
