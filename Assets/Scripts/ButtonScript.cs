using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    GameController gameController;
    public Button button;

    //private void Awake()
    //{
    //    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    //}

    public void setButtonText()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        int currentTurn = gameController.CurrentTurn;

        gameObject.GetComponentInChildren<Text>().text = (currentTurn == 1) ? "X" : "O";

        int c = int.Parse(gameObject.tag);
        int r = c / 3;
        c = c % 3;

        setGameControllerBoardMatrix(r, c, currentTurn);

        if (gameController.AIEnabled)
            gameController.AITurn = true;

        gameController.ButtonPressed = true;
        button.interactable = false;
    }

    public void setTextManually()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        int currentTurn = gameController.CurrentTurn;

        gameObject.GetComponentInChildren<Text>().text = (currentTurn == 1) ? "X" : "O";

        int c = int.Parse(gameObject.tag);
        int r = c / 3;
        c = c % 3;

        setGameControllerBoardMatrix(r, c, currentTurn);

        gameController.AITurn = false;
        gameController.ButtonPressed = true;
        button.interactable = false;
    }

    public void loadSinglePlayerMode()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.twoPlayerMode = false;
    }

    public void load2PlayerMode()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameManager.Instance.twoPlayerMode = true;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void exit()
    {
        Application.Quit();
    }

    // ==== Utility Functions ========================================================================================================

    void setGameControllerBoardMatrix(int r, int c, int currentTurn)
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (gameController.isAIEnabled)
        {
            if (!gameController.isFirstAI)
            {
                if (gameController.FirstTurn == 1)
                {
                    gameController[r, c] = (currentTurn + 1) % 2;
                }
                else
                {
                    gameController[r, c] = currentTurn;
                }
            }
            else
            {
                if (gameController.FirstTurn == 0)
                {
                    gameController[r, c] = (currentTurn + 1) % 2;
                }
                else
                {
                    gameController[r, c] = currentTurn;
                }
            }
        }
        else
        {
            gameController[r, c] = currentTurn;
        }

    }
}
