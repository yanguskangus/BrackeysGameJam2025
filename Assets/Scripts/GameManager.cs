using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // NPCs and Units
    [SerializeField] private PlayerDogController playerDog;
    [SerializeField] private List<HumanController> men;
    [SerializeField] private List<DalmationController> dalmations;
    [SerializeField] private List<StationaryDoggyCamController> cameras;

    // UI components
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameWinText;

    // Level data
    [SerializeField] private Transform spawnPoint;

    // DEBUG
    [SerializeField] private bool dontSpawnBed;


    private void Start()
    {
        if (playerDog == null)
        {
            playerDog = GameObject.FindGameObjectWithTag(Tags.Dog).GetComponent<PlayerDogController>();
        }

        LoadDynamicContent<HumanController>(men, Tags.Man);
        LoadDynamicContent<DalmationController>(dalmations, Tags.Dalmation);
        LoadDynamicContent<StationaryDoggyCamController>(cameras, Tags.DoggyCam);

        playerDog.OnExceedSuspicion += HandleExceedSuspicion;
        playerDog.OnDepositBiscuit += HandleDepositBiscuit;
        playerDog.OnPickupBiscuit += HandlePickupBiscuit;

        if (!dontSpawnBed)
        {
            playerDog.transform.position = spawnPoint.position;
        }
    }

    private void LoadDynamicContent<T>(List<T> results, string tag) where T : MonoBehaviour
    {
        var objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objs)
        {
            var controller = obj.GetComponent<T>();
            if (!results.Contains(controller))
            {
                results.Add(controller);
            }
        }
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetLevel();
        }
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandlePickupBiscuit()
    {

        // TODO: these two are both just setting alert mode.  Maybe
        // it makes more sense to just use an enemycontroller list and call an abstract setalertmode
        foreach (var man in men)
        {
            man.SetAlertMode();
        }

        foreach (var dalmation in dalmations)
        {
            dalmation.SetAlertMode();
        }

        foreach (var camera in cameras)
        {
            camera.SetAlarmState(true);
        }
    }

    private void HandleDepositBiscuit()
    {
        gameWinText.gameObject.SetActive(true);
    }

    private void HandleExceedSuspicion()
    {
        gameOverText.gameObject.SetActive(true);
    }
}

