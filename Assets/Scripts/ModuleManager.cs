using UnityEngine;

public class ModuleManager : MonoBehaviour
{
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
