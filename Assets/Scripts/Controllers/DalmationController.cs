using UnityEngine;

public class DalmationController : EnemyController
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float sniffDelaySec;
    private bool _triggeredChase;

    private void Start()
    {
        sightlineController.OnNoticeDog += HandleNoticeDog;
        catchDogDetector.OnCaughtDog += HandleCaughtDog;

        sightlineSpriteRenderer.color = dangerColor;
    }

    private void Update()
    {
        if (IsAlert)
        {
            if (spriteRenderer.isVisible && !_triggeredChase)
            {
                // When the dalmation smells the player's dog with a treat
                // 1. Hide the sightline
                // 2. Stop moving the dog
                // 3. Prepare dog to chase
                var player = GameObject.FindGameObjectWithTag(Tags.Dog).GetComponent<PlayerDogController>();
                sightlineController.gameObject.SetActive(false);
                moveController.MoveState = MoveState.Stationary;
                StartCoroutine(ChaseDelay(player, sniffDelaySec));
                _triggeredChase = true;
            }
        }

        moveController.ProcessMoveState();
    }

    public void SetAlertMode()
    {
        IsAlert = true;
    }

    private void HandleCaughtDog(PlayerDogController dogController)
    {
        dogController.GetCaught();
    }

    private void HandleNoticeDog(PlayerDogController player)
    {
        StartCoroutine(ChaseDelay(player, chaseDelaySec));
    }
}
