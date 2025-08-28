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

    // UI components
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameWinText;

    // Level data
    [SerializeField] private Transform spawnPoint;


    private void Start()
    {
        playerDog.OnExceedSuspicion += HandleExceedSuspicion;
        playerDog.OnDepositBiscuit += HandleDepositBiscuit;
        playerDog.OnPickupBiscuit += HandlePickupBiscuit;

        playerDog.transform.position = spawnPoint.position;
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

