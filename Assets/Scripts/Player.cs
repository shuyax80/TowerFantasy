using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private float range = 5;
    [SerializeField] private long damage;   
    [SerializeField] private long maxHealth;
    [SerializeField] private long currentHealth;
    
    void Update()
    {
        
    }

    private void Shoot()
    {
        muzzleFlash.Play();
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
