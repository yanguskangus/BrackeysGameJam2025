using System.Collections;
using UnityEngine;

public class BreakableBoxController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D Rb;
    [SerializeField] private Collider2D boxCollider;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject fragmentParent;
    [SerializeField] private Animator animator;
    [SerializeField] private float brokenTime;

    private Vector3 _position;

    private void Start()
    {
        _position = transform.position;
    }

    public void BreakBox()
    {
        SetBoxState(false);
        animator.SetTrigger(AnimationParameters.BreakBox);
    }

    public void Respawn()
    {
        transform.position = _position;
        SetBoxState(true);
    }

    public void SetBoxState(bool functional)
    {
        Rb.simulated = functional;
        boxCollider.enabled = functional;
        spriteRenderer.enabled = functional;
        fragmentParent.SetActive(!functional);
    }

    public void OnBreakAnimationFinished()
    {
        fragmentParent.SetActive(false);
        StartCoroutine(RespawnBoxRoutine());
    }

    private IEnumerator RespawnBoxRoutine()
    {
        yield return new WaitForSeconds(brokenTime);
        Respawn();
    }
}
