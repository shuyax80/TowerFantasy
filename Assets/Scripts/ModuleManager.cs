using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ModuleManager : MonoBehaviour
{
   [SerializeField] private List<ModuleBase> _modules = new List<ModuleBase>();
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
      _modules.Add(armor);
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
}
