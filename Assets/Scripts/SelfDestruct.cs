using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float delay;
    void Start()
    {
        Destroy(this.gameObject, delay);
    }
}
