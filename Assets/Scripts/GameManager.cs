using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // NPCs and Units
    [SerializeField] private PlayerDogController playerDog;

    // UI components
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameWinText;

    // Level data
    [SerializeField] private int currentLevel;
    [SerializeField] private List<Transform> spawnPoints;


    private void Start()
    {
        playerDog.OnExceedSuspicion += HandleExceedSuspicion;
        playerDog.OnDepositBiscuit += HandleDepositBiscuit;
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetLevel();
        }

        /* // Test
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            playerDog.TakeSuspicion(100);
        } */
    }

    private void ResetLevel()
    {
        gameOverText.gameObject.SetActive(false);
        gameWinText.gameObject.SetActive(false);

        playerDog.transform.position = spawnPoints[currentLevel].position;
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

