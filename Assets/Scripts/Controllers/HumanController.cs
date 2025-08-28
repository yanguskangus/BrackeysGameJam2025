using System;
using System.Collections;
using UnityEngine;

public class HumanController : EnemyController
{
    private void Start()
    {
        sightlineController.OnNoticeDog += HandleNoticeDog;
        catchDogDetector.OnCaughtDog += HandleCaughtDog;

        if (IsAlert)
        {
            sightlineSpriteRenderer.color = dangerColor;
        }
        else
        {
            sightlineSpriteRenderer.color = passiveColor;
        }
    }

    private void Update()
    {
        moveController.ProcessMoveState();
    }

    public void SetAlertMode()
    {
        IsAlert = true;
        sightlineSpriteRenderer.color = dangerColor;
    }

    private void HandleCaughtDog(PlayerDogController dogController)
    {
        if (IsAlert)
        {
            dogController.GetCaught();
        }
    }

    private void HandleNoticeDog(PlayerDogController player)
    {
        if (IsAlert)
        {
            StartCoroutine(ChaseDelay(player, chaseDelaySec));
        }
    }
}
