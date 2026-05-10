using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private GameObject bulletExplosionPrefab;
    private long _damage;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(long damage)
    {
        _damage = damage;
    }

    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<Enemy>(out var enemy))
        {
            return;
        }

        SpawnExplosion();
        enemy.GetDamage(_damage);
        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        if (bulletExplosionPrefab == null)
        {
            return;
        }

        var explosion = Instantiate(bulletExplosionPrefab);
        explosion.transform.position = transform.position;

        foreach (var particleSystem in explosion.GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Play();
        }
    }
}
