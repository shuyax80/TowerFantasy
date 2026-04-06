using System.Collections;
using UnityEngine;

public class ArmorModule : ModuleBase
{
    [SerializeField] private int regenTickTimer;
    [SerializeField] private int regenAmount; 
    [SerializeField] private int damageReductionAmount;
    private int _damageReduction;
    private void Awake()
    {
        Id = 1;
        IsUnlocked = false;
        IsActive = false;
        Upgrades[0] = 0;
        Upgrades[1] = 0;
    }

    private void Start()
    {
        if (this.IsUnlocked && this.IsActive)
        {
            if (Upgrades[0] == 1)
            {
                StartCoroutine(Regen());
            }

            if (Upgrades[1] == 1)
            {
                _damageReduction = damageReductionAmount;
            }
        }
    }

    public int GetDamageReduction()
    {
        return _damageReduction;
    }

    IEnumerator Regen()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenTickTimer);
            Player.Instance.ModifyHealth(regenAmount, false);
        }
    }
}
