using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private long health;
    [SerializeField] private long scoreValue; 
    [SerializeField] private long damage;
    [SerializeField] private GameObject explosion;
    private float _speed;
    private float _stopDistance;
    private float _rotationSpeed;
    private Transform _playerTransform;

    public void Init(int multiplier)
    {
        health *= multiplier;
        scoreValue *= multiplier;
        damage *= multiplier;
    }
    
    void Start()
    {
        var randomDirection = Random.Range(0, 100);
       
        _speed = Random.Range(0.3f, 1.0f);
        _stopDistance = 0.5f;
        _rotationSpeed = randomDirection < 50 ? Random.Range(70f, 200f) : Random.Range(-200f, -70f);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
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
            Player.Instance.TakeDamage(damage);
            DestroyEnemy();
        }
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
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
}
