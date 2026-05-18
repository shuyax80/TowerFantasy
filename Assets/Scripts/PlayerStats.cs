using System;

[Serializable]
public class PlayerStats
{
    public long Damage => damage;
    public float FireRate => fireRate;
    public long CurrentHealth => currentHealth;
    public long MaxHealth => maxHealth;
    public long CurrentEnergy => currentEnergy;
    public long MaxEnergy => maxEnergy;
    public long EnergyConsumptionMovement => energyConsumptionMovement;
    public long EnergyConsumptionShoot => energyConsumptionShoot;
    public long EnergyConsumptionTick => energyConsumptionTick;
    public long DamageIncreasedBy => damageIncreasedBy;
    public long HealthIncreasedBy => healthIncreasedBy;
    public float FireRateIncreasedBy => fireRateIncreasedBy;
    public int Level => level;

    
    public void IncreaseLevel()
    {
        level++;
        skillPoints++;
    }

    public void MaxHealthAndEnergy()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
    }

    public void ModifyCurrentHealth(long quantity)
    {
        currentHealth = ClampMin(currentHealth - quantity, 0);
    }

    public void ModifyCurrentEnergy(long quantity)
    {
        currentEnergy = ClampMin(currentEnergy - quantity, 0);
    }

    private static long ClampMin(long value, long min)
    {
        return value < min ? min : value;
    }

    [UnityEngine.SerializeField] private long damage;
    [UnityEngine.SerializeField] private long maxHealth;
    [UnityEngine.SerializeField] private long currentHealth;
    [UnityEngine.SerializeField] private float fireRate;
    [UnityEngine.SerializeField]private long currentEnergy;
    [UnityEngine.SerializeField] private long maxEnergy;
    [UnityEngine.SerializeField] private long energyConsumptionMovement;
    [UnityEngine.SerializeField] private long energyConsumptionShoot;
    [UnityEngine.SerializeField] private long energyConsumptionTick;
    [UnityEngine.SerializeField] private long damageIncreasedBy;
    [UnityEngine.SerializeField] private long healthIncreasedBy;
    [UnityEngine.SerializeField] private float fireRateIncreasedBy;
    [UnityEngine.SerializeField] private int level = 1;
    [UnityEngine.SerializeField] private int skillPoints = 0;
}
