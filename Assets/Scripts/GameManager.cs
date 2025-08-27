using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;

    // NPCs and Units
    [SerializeField] private PlayerDogController playerDog;

    // Biscuits and spawns
    [SerializeField] private int biscuitCount;

    // UI components
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameWinText;
    [SerializeField] private TMP_Text biscuitCountText;

    private void Start()
    {
        playerDog.OnExceedSuspicion += HandleExceedSuspicion;
        playerDog.OnDepositBiscuit += HandleDepositBiscuit;
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
        gameWinText.gameObject.SetActive(false);
    }

    private void HandleDepositBiscuit()
    {
        // gameWinText.gameObject.SetActive(true);
        biscuitCount++;
        biscuitCountText.text = biscuitCount.ToString();
    }

    private void HandleExceedSuspicion()
    {
        gameOverText.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        var bottomLeft = grid.center.position;
        bottomLeft.x -= 0.5f * grid.columns * grid.cellSize;
        bottomLeft.y -= 0.5f * grid.rows * grid.cellSize;
        bottomLeft.x += grid.cellSize * 0.5f;
        bottomLeft.y += grid.cellSize * 0.5f;

        for (int row = 0; row < grid.rows; row++)
        {
            var cellPosition = bottomLeft;
            cellPosition.y = bottomLeft.y + row * grid.cellSize;
            for (int col = 0; col < grid.columns; col++)
            {
                cellPosition.x = bottomLeft.y + col * grid.cellSize;
                Gizmos.DrawWireCube(cellPosition, new Vector2(grid.cellSize, grid.cellSize));
            }
        }
    }
}

[System.Serializable]
public struct Grid
{
    public Transform center;
    public int rows;
    public int columns;
    public float cellSize;
}
