using UnityEngine;

public class ModuleBase : MonoBehaviour
{
   public int Id { get; set; } = 0;
   public bool IsUnlocked { get; set; } = false;
   public bool IsActive { get; set; } = false;
   public int[] Upgrades { get; set; } = new int[8];

   public void Unlock()
   {
      IsUnlocked = true;
   }
   public void Activate()
   {
      IsActive = true;
   }
   public void ActivateUpgrade(int upgradeIndex)
   {
      Upgrades[upgradeIndex] = 1;
   }
}
