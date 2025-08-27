using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // NPCs and Units
    [SerializeField] private PlayerDogController playerDog;

    // UI components
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameWinText;

    // Level data
    [SerializeField] private Transform spawnPoint;


    private void Start()
    {
        playerDog.OnExceedSuspicion += HandleExceedSuspicion;
        playerDog.OnDepositBiscuit += HandleDepositBiscuit;

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

    private void HandleDepositBiscuit()
    {
        gameWinText.gameObject.SetActive(true);
    }

    private void HandleExceedSuspicion()
    {
        gameOverText.gameObject.SetActive(true);
    }
}

