using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private PlayerDogController playerDog;

    private void Start()
    {
        playerDog.OnExceedSuspicion += HandleExceedSuspicion;
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetGame();
        }

        /* // Test
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            playerDog.TakeSuspicion(100);
        } */
    }

    private void ResetGame()
    {
        gameOverText.gameObject.SetActive(false);
    }

    private void HandleExceedSuspicion()
    {
        gameOverText.gameObject.SetActive(true);
    }
}
