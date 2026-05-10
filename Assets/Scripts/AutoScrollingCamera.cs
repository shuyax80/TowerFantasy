using UnityEngine;

public class AutoScrollingCamera : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private Vector2 scrollDirection = Vector2.up;
    [SerializeField] private bool isScrolling = true;

    public Vector2 Velocity
    {
        get
        {
            if (!isScrolling)
            {
                return Vector2.zero;
            }

            return scrollDirection.normalized * scrollSpeed;
        }
    }

    private void Update()
    {
        var velocity = Velocity;
        transform.position += new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime;
    }
}
