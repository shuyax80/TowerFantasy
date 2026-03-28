using System;
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
    
    [Header("Range circle setting")]
    [SerializeField] private int segments = 50; 
    [SerializeField]private LineRenderer lineRenderer;
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
       
        lineRenderer.useWorldSpace = false; 
    }

    private void Start()
    {
        DrawRangeCircle();
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
        DrawRangeCircle();
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
        _level++;
        range += 0.1f;
        damage += 2;
        maxHealth += 10;
        currentHealth += 10;
        fireRate -= 0.01f;
    } 
    
    public void DrawRangeCircle()
    {
        var realRange = range;
        var parentScale = transform.lossyScale.x;
        var localRadius = realRange / parentScale;

        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false; 

        var angle = 0f;
        for (var i = 0; i < (segments + 1); i++)
        {
            var x = Mathf.Sin(Mathf.Deg2Rad * angle) * localRadius;
            var y = Mathf.Cos(Mathf.Deg2Rad * angle) * localRadius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += (360f / segments);
        }
    }

    public void TakeDamage(long quantity)
    {
        currentHealth -= quantity;
        UiManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }
}
