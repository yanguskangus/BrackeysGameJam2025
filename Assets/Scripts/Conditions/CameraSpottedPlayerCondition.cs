using System.Collections.Generic;
using UnityEngine;

public class CameraSpottedPlayerTrigger : MonoBehaviour
{
    [SerializeField] private StationaryDoggyCamController doggyCam;
    public System.Action<PlayerDogController> Trigger;

    private bool _triggered;

    private void Start()
    {
        doggyCam.sightlineController.OnNoticeDog += HandlePlayerNoticed;
    }

    private void HandlePlayerNoticed(PlayerDogController playerDog)
    {
        if (!_triggered && doggyCam.IsAlert)
        {
            Trigger?.Invoke(playerDog);
            _triggered = true;
        }
    }
}
