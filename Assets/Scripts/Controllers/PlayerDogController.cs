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
    [SerializeField] private int suspicionDecayInterval; // delay before suspicion starts decaying, in seconds
    [SerializeField] private int suspicionDecayRate; // amount suspicion drops each interval
    public int Suspicion;
    private float secondsUntilSuspicionDecay; // how long until suspicion meter can decay

    // Events
    public System.Action OnExceedSuspicion;
    public System.Action OnWin;

    // Animations
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Biscuits
    [SerializeField] private GameObject carriedBiscuit;
    [SerializeField] private bool carryingBiscuit;

    void Start()
    {
        suspicionBar.maxValue = maxSuspicion;
        suspicionBar.value = Suspicion;
        secondsUntilSuspicionDecay = 0;
    }

    void Update()
    {
        UpdateMovement();
        DecaySuspicion();
    }

    private void LateUpdate()
    {
        suspicionBar.value = Suspicion;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_moveInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (_moveInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        PositionBiscuit(spriteRenderer.flipX);
        animator.SetBool(AnimationParameters.Running, _moveInput.sqrMagnitude > Mathf.Epsilon);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Biscuit))
        {
            TakeBiscuit();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag(Tags.Goal))
        {
            if (CheckWinCondition())
            {
                OnWin?.Invoke();
            }
        }
    }

    private void UpdateMovement()
    {
        rb.linearVelocity = moveSpeed * _moveInput;
    }

    private void DecaySuspicion()
    {
        secondsUntilSuspicionDecay -= Time.deltaTime;
        if (secondsUntilSuspicionDecay <= 0)
        {
            secondsUntilSuspicionDecay = suspicionDecayInterval;
            Suspicion = Mathf.Max(0, Suspicion - suspicionDecayRate);
        }
    }

    public void TakeSuspicion(int damage)
    {
        // Reset the suspicion decay delay
        secondsUntilSuspicionDecay = suspicionDecayInterval;

        Suspicion = Mathf.Min(Suspicion + damage, maxSuspicion);

        if (Suspicion >= maxSuspicion)
        {
            OnExceedSuspicion?.Invoke();
        }
    }

    private bool CheckWinCondition()
    {
        return carryingBiscuit;
    }

    private void TakeBiscuit()
    {
        carriedBiscuit.gameObject.SetActive(true);
        carryingBiscuit = true;

        PositionBiscuit(spriteRenderer.flipX);
    }

    private void PositionBiscuit(bool flipped)
    {
        var direction = flipped ? 1 : -1;
        var localPosition = carriedBiscuit.transform.localPosition;

        localPosition.x = direction * Mathf.Abs(localPosition.x);
        carriedBiscuit.transform.localPosition = localPosition;
    }
}
