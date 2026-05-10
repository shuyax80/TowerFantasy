using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 2f;

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

        enemy.GetDamage(_damage);
        Destroy(gameObject);
    }
}
