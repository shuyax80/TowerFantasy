using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionEffect;
    
    private const float LifeTimeSeconds = 5f;
    private const float HitDistance = 0.2f;

    private Transform _target;
    private long _damage;
    private float _speed;
    private bool _isInitialized;
    private float _explosionRadius = 1f;
    private bool _explosionEnabled = false;

    public void Setup(Transform target, long damage, float speed, int[] upgradesToApply)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
        _isInitialized = true;
        _explosionEnabled = upgradesToApply[0] == 1 ? true : false;
        Destroy(gameObject, LifeTimeSeconds);
    }

    private void Update()
    {
        if (!_isInitialized || IsTargetInvalid())
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();

        if (HasReachedTarget())
        {
            HitTarget();
        }
    }

    private bool IsTargetInvalid()
    {
        return _target.IsUnityNull();
    }

    private void MoveTowardsTarget()
    {
        float step = _speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(
            transform.position,
            _target.position,
            step
        );
    }

    private bool HasReachedTarget()
    {
        return Vector2.Distance(transform.position, _target.position) < HitDistance;
    }

    private void HitTarget()
    {
        ApplyDamageToTarget();
        Destroy(gameObject);
    }

    private void ApplyDamageToTarget()
    {
        if (_target.TryGetComponent(out Enemy enemy))
        {
            enemy.GetDamage(_damage);
            if(_explosionEnabled)
                GenerateExplosion();
        } 
    }

    private void GenerateExplosion()
    {
        var explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        var main = explosion.main;
        var speed = main.startSpeed.constant; 
    
        if (speed > 0) 
        {
            main.startLifetime = _explosionRadius / speed;
        }
        else 
        {
            main.startLifetime = 0.5f; 
        }

        main.stopAction = ParticleSystemStopAction.Destroy;

        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true; 
        filter.SetLayerMask(LayerMask.GetMask("Enemy"));

        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, _explosionRadius, filter, results);

        foreach (var hit in results)
        {
            if (hit.TryGetComponent(out Enemy nearbyEnemy))
            {
                nearbyEnemy.GetDamage(_damage);
            }
        }
    }
}
