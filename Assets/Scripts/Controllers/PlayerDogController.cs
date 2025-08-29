using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerDogController : MonoBehaviour
{
    // Components 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Transform dogCenter; // This is nice for some aesthetic things like doggy cam tracking

    // Movement
    [SerializeField] private float moveSpeed;

    // dashing
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldownDuration;
    [SerializeField] private float dashDuration;

    private float _dashTime;
    private float _dashCooldownTime;
    private Vector2 _dashDirection;
    private bool _dashing;

    // pushing
    [SerializeField] private float pushForce;

    private Vector2 _moveInput;

    // Suspicion meter
    // [SerializeField] private Slider suspicionBar;
    [SerializeField] private int maxSuspicion;
    [SerializeField] private int suspicionDecayInterval; // delay before suspicion starts decaying, in seconds
    [SerializeField] private int suspicionDecayRate; // amount suspicion drops each interval
    public int Suspicion;
    private float secondsUntilSuspicionDecay; // how long until suspicion meter can decay

    // Events
    public System.Action OnExceedSuspicion;
    public System.Action OnDepositBiscuit;
    public System.Action OnPickupBiscuit;

    // Biscuits
    [SerializeField] private GameObject carriedBiscuit;
    public bool CarryBiscuit;

    void Start()
    {
        /* suspicionBar.maxValue = maxSuspicion;
        suspicionBar.value = Suspicion; */
        secondsUntilSuspicionDecay = 0;
    }

    void Update()
    {
        UpdateMovement();
        DecaySuspicion();

        animator.SetBool(AnimationParameters.Dashing, _dashing);
        animator.SetBool(AnimationParameters.Running, _moveInput.sqrMagnitude > Mathf.Epsilon);
    }

    private void LateUpdate()
    {
        // suspicionBar.value = Suspicion;
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Conditions for dashing:
            // - doggo must to be dashing already
            // - cooldown needs to be greater than 0
            // - player must be pressing a directional key
            if (!_dashing && _dashCooldownTime <= 0 && _moveInput.sqrMagnitude > Mathf.Epsilon)
            {
                Dash();
            }
        }
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Tags.BreakableBox))
        {
            if (_dashing)
            {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(Tags.BreakableBox))
        {
            if (_dashing)
            {
                Destroy(other.gameObject);
            }
            else
            {
                var breakableBox = other.gameObject.GetComponent<BreakableBoxController>();
                breakableBox.Rb.AddForceAtPosition(_moveInput, other.contacts[0].point, ForceMode2D.Impulse);
            }
        }
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
                EatBiscuit();
                OnDepositBiscuit?.Invoke();
            }
        }
    }

    private void UpdateMovement()
    {
        if (_dashing)
        {
            rb.linearVelocity = dashSpeed * _dashDirection;

            if (_dashTime <= Mathf.Epsilon)
            {
                _dashing = false;
                _dashDirection = Vector2.zero;
                _dashTime = 0;
            }
            else
            {
                _dashTime -= Time.deltaTime;
                _dashCooldownTime = dashCooldownDuration;
            }
        }
        else
        {
            rb.linearVelocity = moveSpeed * _moveInput;
            _dashCooldownTime = Mathf.Max(_dashCooldownTime - Time.deltaTime, 0);
        }
    }

    private void Dash()
    {
        _dashing = true;
        _dashTime = dashDuration;
        _dashDirection = _moveInput;
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

    public void GetCaught()
    {
        OnExceedSuspicion?.Invoke();
    }

    private bool CheckWinCondition()
    {
        return CarryBiscuit;
    }

    private void EatBiscuit()
    {
        carriedBiscuit.gameObject.SetActive(false);
        CarryBiscuit = false;
    }

    private void TakeBiscuit()
    {
        carriedBiscuit.gameObject.SetActive(true);
        CarryBiscuit = true;

        PositionBiscuit(spriteRenderer.flipX);
        OnPickupBiscuit?.Invoke();
    }

    private void PositionBiscuit(bool flipped)
    {
        var direction = flipped ? 1 : -1;
        var localPosition = carriedBiscuit.transform.localPosition;
        localPosition.x = direction * Mathf.Abs(localPosition.x);
        carriedBiscuit.transform.localPosition = localPosition;
    }
}
