using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : EnemyController
{
    protected void Awake()
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

    protected void HandleCaughtDog(PlayerDogController dogController)
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
            TriggerChase(player);
        }
    }

    public void TriggerChase(PlayerDogController player)
    {
        if (moveController.MoveState != MoveState.Chase)
        {
            StartCoroutine(ChaseDelay(player, chaseDelaySec));
        }
    }
}
