using Unity.VisualScripting;
using UnityEngine;

public class CannonModule : ModuleBase
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 10f;

    public void Shoot(Enemy target, long damage)
    {
        GameObject bulletGameObject = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Transform targetTransform = target.transform;

        if (bulletGameObject.TryGetComponent(out Bullet bullet))
        {
            bullet.Setup(targetTransform, damage, bulletSpeed, Upgrades);
        }
    }
}
