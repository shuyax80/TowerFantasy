using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Vector2 screenPadding = Vector2.zero;
    
    [Header("Player stats")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private long damage;   
    [SerializeField] private long maxHealth;
    [SerializeField] private long currentHealth;
    [SerializeField] private float fireRate;
    
    [Header("Player stats increase")]
    [SerializeField] private long damageIncreasedBy = 2;
    [SerializeField] private long healthIncreasedBy = 20;
    [SerializeField] private float fireRateIncreasedBy = 0.01f;
    private int _level = 1;
    
    public static Player Instance { get; private set; }
    private float _nextFireTime;
    private GameObject _target;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _lastCameraPosition;
    private bool _hasCameraPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        Instance = this;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (gameplayCamera == null)
        {
            gameplayCamera = Camera.main;
        }

        ClampPositionToScreen();
        UpdateCameraPositionSnapshot();
    }

    private void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        ApplyCameraMovement();

        var movementInput = ReadMovementInput();
        var direction = new Vector3(movementInput.x, movementInput.y, 0f).normalized;

        transform.position += direction * (movementSpeed * Time.deltaTime);
        ClampPositionToScreen();
    }

    private void ApplyCameraMovement()
    {
        if (gameplayCamera.IsUnityNull())
        {
            return;
        }

        var cameraPosition = gameplayCamera.transform.position;
        if (!_hasCameraPosition)
        {
            _lastCameraPosition = cameraPosition;
            _hasCameraPosition = true;
            return;
        }

        var cameraDelta = cameraPosition - _lastCameraPosition;
        cameraDelta.z = 0f;

        transform.position += cameraDelta;
        _lastCameraPosition = cameraPosition;
    }

    private void UpdateCameraPositionSnapshot()
    {
        if (gameplayCamera.IsUnityNull())
        {
            return;
        }

        _lastCameraPosition = gameplayCamera.transform.position;
        _hasCameraPosition = true;
    }

    private static Vector2 ReadMovementInput()
    {
        var movementInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                movementInput.x -= 1f;
            }

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                movementInput.x += 1f;
            }

            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            {
                movementInput.y -= 1f;
            }

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            {
                movementInput.y += 1f;
            }
        }

        if (Gamepad.current != null)
        {
            movementInput += Gamepad.current.leftStick.ReadValue();
            movementInput += Gamepad.current.dpad.ReadValue();
        }

        return Vector2.ClampMagnitude(movementInput, 1f);
    }

    private void ClampPositionToScreen()
    {
        if (gameplayCamera.IsUnityNull())
        {
            return;
        }

        var position = transform.position;
        var cameraHeight = gameplayCamera.orthographicSize;
        var cameraWidth = cameraHeight * gameplayCamera.aspect;
        var cameraPosition = gameplayCamera.transform.position;
        var playerExtents = GetPlayerExtents();

        var minX = cameraPosition.x - cameraWidth + playerExtents.x + screenPadding.x;
        var maxX = cameraPosition.x + cameraWidth - playerExtents.x - screenPadding.x;
        var minY = cameraPosition.y - cameraHeight + playerExtents.y + screenPadding.y;
        var maxY = cameraPosition.y + cameraHeight - playerExtents.y - screenPadding.y;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = position;
    }

    private Vector2 GetPlayerExtents()
    {
        if (_spriteRenderer.IsUnityNull())
        {
            return Vector2.zero;
        }

        Bounds bounds = _spriteRenderer.bounds;
        return new Vector2(bounds.extents.x, bounds.extents.y);
    }

    private void Shoot()
    {
        muzzleFlash.Play();
    }

    public void IncreaseLevel()
    {
        _level++;
        damage += damageIncreasedBy;
        maxHealth += healthIncreasedBy;
        currentHealth += healthIncreasedBy;
        fireRate -= fireRateIncreasedBy;
    } 
    
    

    public void TakeDamage(long quantity)
    {
        currentHealth -= quantity;
        UiManager.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }
}
