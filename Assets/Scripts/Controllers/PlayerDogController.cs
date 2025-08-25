using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerDogController : MonoBehaviour
{
    // Movement
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    private Vector2 _moveInput;

    // Suspicion meter
    [SerializeField] private Slider suspicionBar;
    [SerializeField] private int maxSuspicion;
    public int Suspicion;

    // Events
    public System.Action OnExceedSuspicion;

    // Biscuits
    public GameObject CarriedBiscuit;

    void Start()
    {
        suspicionBar.maxValue = maxSuspicion;
        suspicionBar.value = Suspicion;
    }

    void Update()
    {
        UpdateMovement();
    }

    private void LateUpdate()
    {
        suspicionBar.value = Suspicion;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Biscuit))
        {
            TakeBiscuit();
            Destroy(other.gameObject);
        }
    }

    private void UpdateMovement()
    {
        rb.linearVelocity = moveSpeed * _moveInput;
    }

    public void TakeSuspicion(int damage)
    {
        Suspicion = Mathf.Min(Suspicion + damage, maxSuspicion);

        if (Suspicion >= maxSuspicion)
        {
            OnExceedSuspicion?.Invoke();
        }
    }

    private void TakeBiscuit()
    {
        CarriedBiscuit.gameObject.SetActive(true);
    }
}
