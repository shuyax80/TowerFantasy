using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
   [SerializeField] private List<ModuleBase> modules = new List<ModuleBase>();
   public static ModuleManager Instance { get; set; }
   
   private int _upgradePoints = 0;
   
   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(this.gameObject);
         return;
      }
      Instance = this;
      var armor = GetComponent<ArmorModule>();
      var barrier = GetComponent<BarrierModule>();
      var distortionField = GetComponent<DistortionFieldModule>();
      modules.Add(armor);
      modules.Add(barrier);
      modules.Add(distortionField);
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
      var barrier = modules.Find(x => x.GetType() == typeof(BarrierModule)).GetComponent<BarrierModule>();
      return barrier.AlterBarrierEnergy(amount, true);
      
   }
}
