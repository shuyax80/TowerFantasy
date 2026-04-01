
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("Player stats")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private float range = 5;
    [SerializeField] private long damage;   
    [SerializeField] private long maxHealth;
    [SerializeField] private long currentHealth;
    [SerializeField] private float fireRate;
    
    
    private int _level = 1;
    
    public static Player Instance { get; private set; }
    private float _nextFireTime;
    private GameObject _target;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        _target = EnemySpawner.Instance.GetClosestEnemy(this.transform.position);
        if (!_target.IsUnityNull())
        {
            RotateToTarget(_target.transform.position);
            var distance = Vector3.Distance(transform.position, _target.transform.position);
            if (distance <= range && Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot()
    {
        muzzleFlash.Play();
        if (_target.TryGetComponent<Enemy>(out var script))
        {
            script.GetDamage(damage);
        }
    }
    
    private void RotateToTarget(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle -90f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void IncreaseLevel()
    {
        if (ModuleManager.Instance.ReturnModules().Any(x => x.Id == 1 && x.IsActive && x.IsUnlocked))
        {
            maxHealth += 10;
            currentHealth += 10;
        }
        _level++;
        range += 0.1f;
        damage += 2;
        maxHealth += 10;
        currentHealth += 10;
        fireRate -= 0.01f;
        ModuleManager.Instance.IncreaseUpgradePoints();
    }
    
    public void ModifyHealth(long quantity, bool isDamage)
    {
        if (isDamage)
        {
            var armor = ModuleManager.Instance.ReturnModules().ToList().OfType<ArmorModule>().FirstOrDefault();
            if(armor != null)
                currentHealth -=  quantity - armor.GetDamageReduction();
        }
        else
        {
            if (currentHealth < maxHealth)
                currentHealth += quantity;
            else
                currentHealth += 0;
        }
        UiManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }
}
