using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlertHumanController : MonoBehaviour
{
    [SerializeField] private HumanController humanController;
    [SerializeField] private CameraSpottedPlayerTrigger cameraSpottedPlayerTrigger;

    private void Start()
    {
        cameraSpottedPlayerTrigger.Trigger += HandleCameraSpottedPlayer;
    }

    private void HandleCameraSpottedPlayer(PlayerDogController player)
    {
        humanController.TriggerChase(player);
    }
}
