using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private long health;
    [SerializeField] private long scoreValue;
    [SerializeField] private GameObject explosion;
    private float _speed;
    private float _stopDistance;
    private float _rotationSpeed;
    private Transform _playerTransform;
    
    void Start()
    {
        _speed = Random.Range(0.3f, 1.0f);
        _stopDistance = 0.5f;
        _rotationSpeed = Random.Range(70f, 200f);
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

       
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    public void GetDamage(long amount)
    {
        health -= amount;
        if(health <= 0)
            Destroy(this.gameObject);
        if(!explosion.IsUnityNull())
            Instantiate(explosion, transform.position, Quaternion.identity);
        UiManager.Instance.AddScore(scoreValue);
    }
}
