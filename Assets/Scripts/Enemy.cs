using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private long health;
    [SerializeField] private long scoreValue; 
    [SerializeField] private long damage;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float bounceForce = 3f;
    
    private float _speed;
    private float _stopDistance;
    private float _rotationSpeed;
    private Transform _playerTransform;
    private Vector3 _spawnPosition;
    private bool _isBouncing;

    public void Init(int multiplier)
    {
        health *= multiplier;
        scoreValue *= multiplier;
        damage *= multiplier;
    }
    
    void Start()
    {
        var randomDirection = Random.Range(0, 100);
        _spawnPosition = transform.position;    
        _speed = Random.Range(0.3f, 1.0f);
        _stopDistance = 0.5f;
        _rotationSpeed = randomDirection < 50 ? Random.Range(70f, 200f) : Random.Range(-200f, -70f);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (_isBouncing)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _spawnPosition, 
                (_speed * bounceForce) * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, _spawnPosition) < 0.1f)
            {
                _isBouncing = false;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }
    
    
    
    private void MoveTowardsPlayer()
    {
        var distance = Vector2.Distance(transform.position, _playerTransform.position);

        if (distance > _stopDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                _playerTransform.position, 
                _speed * Time.deltaTime
            );
        }
        else
        {
            Player.Instance.ModifyHealth(damage, true);
            DestroyEnemy();
        }
    }
    public void GetDamage(long amount)
    {
        health -= amount;
        if(health <= 0)
          DestroyEnemy();
        GameManager.Instance.AddXp(scoreValue);
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
        if(!explosion.IsUnityNull())
            Instantiate(explosion, transform.position, Quaternion.identity);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6) 
        {
            if (!_isBouncing) 
            {
                _isBouncing = true;
                Vector2 directionToSpawn = (_spawnPosition - transform.position).normalized;
                transform.position += (Vector3)directionToSpawn * 0.2f; 
                ModuleManager.Instance.DamagePlayerBarriers(damage);
            }
        }
    }
}
