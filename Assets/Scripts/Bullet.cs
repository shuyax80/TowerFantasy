using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float LifeTimeSeconds = 5f;
    private const float HitDistance = 0.2f;

    private Transform _target;
    private long _damage;
    private float _speed;
    private bool _isInitialized;

    public void Setup(Transform target, long damage, float speed)
    {
        _target = target;
        _damage = damage;
        _speed = speed;
        _isInitialized = true;

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
        return _target == null;
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
        }
    }
}
