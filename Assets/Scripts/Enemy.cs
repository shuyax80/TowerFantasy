using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private long health;
    [SerializeField] private long scoreValue; 
    [SerializeField] private long damage;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Vector2 speedRange = new Vector2(0.3f, 1f);
    private float _speed;
    private float _rotationSpeed;

    private void Awake()
    {
        if (GetComponent<Collider2D>() != null)
        {
            return;
        }

        var enemyCollider = gameObject.AddComponent<CircleCollider2D>();
        enemyCollider.isTrigger = false;
    }

    public void Init(int multiplier)
    {
        health *= multiplier;
        scoreValue *= multiplier;
        damage *= multiplier;
    }
    
    void Start()
    {
        var randomDirection = Random.Range(0, 100);
       
        _speed = Random.Range(speedRange.x, speedRange.y);
        _rotationSpeed = randomDirection < 50 ? Random.Range(70f, 200f) : Random.Range(-200f, -70f);
    }

    private void Update()
    {
        transform.position += Vector3.down * (_speed * Time.deltaTime);
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage(damage);
            DestroyEnemy();
        }

        if (other.TryGetComponent<Base>(out var baseScript))
        {
            DestroyEnemy();
        }
    }

    public void GetDamage(long amount)
    {
        
        health -= amount;
        Debug.Log($"{gameObject.name} bullet hit {amount} current health {health}");
        if(health <= 0)
        {
            DestroyEnemy();
            GameManager.Instance.AddXp(scoreValue);
        }
        
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
        if(!explosion.IsUnityNull())
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
