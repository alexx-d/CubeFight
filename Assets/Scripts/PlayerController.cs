using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 30f;

    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody;

    public float pauseButton = 0;

    public void OnMove(InputAction.CallbackContext callbackContext)
    {
        _moveInput = callbackContext.ReadValue<Vector2>();
    }

    public void GetPauseInput(InputAction.CallbackContext callbackContext)
    {
        pauseButton = callbackContext.ReadValue<float>();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = transform.forward * _moveInput.x;
        _rigidbody.AddForce(moveDirection * _moveSpeed);

        var impulse = (-_moveInput.x * _rotateSpeed * Mathf.Deg2Rad);
        _rigidbody.AddTorque(impulse, ForceMode2D.Impulse);
    }
}