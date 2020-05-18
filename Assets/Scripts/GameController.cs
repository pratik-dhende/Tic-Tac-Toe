using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Button buttonPrefab;
    public GameObject board;
    public Text yourTurn;
    public bool isAIEnabled = false;
    public bool isFirstAI = false;
    public GameObject toggleAITurn;
    //public GameObject gameManager;

    int currentTurn = -1;
    bool isButtonPressed = false;
    bool isGameOver = false;
    int[,] boardMatrix = new int[3, 3];
    List<Button> buttons = new List<Button>();
    int firstTurn;
    bool isAITurn;
    bool gotUserFirstTurnChoice = false;
    bool is2PlayerMode = false;
    bool firstTurnDone = false;


    //= Getters ======================================================================================================================

    public int CurrentTurn
    {
        get
        {
            return currentTurn;
        }
        set
        {
            currentTurn = value;
        }
    }

    public int this[int r, int c]
    {
        get
        {
            return boardMatrix[r, c];
        }
        set
        {
            boardMatrix[r, c] = value;
        }
    } 

    public bool ButtonPressed
    {
        get
        {
            return isButtonPressed;
        }
        set
        {
            isButtonPressed = value;
        }
    }

    public int[,] BoardMatrix
    {
        get
        {
            return boardMatrix;
        }
    }

    public bool AIEnabled
    {
        get
        {
            return isAIEnabled;
        }
    }

    public bool AITurn
    {
        get
        {
            return isAITurn;
        }
        set
        {
            isAITurn = value;
        }
    }

    public List<Button> Buttons
    {
        get
        {
            return buttons;
        }
    }

    public int FirstTurn
    {
        get
        {
            return firstTurn;
        }
    }

    public bool twoPlayerMode
    {
        get
        {
            return is2PlayerMode;
        }
        set
        {
            is2PlayerMode = value;
        }
    }

//== Main Loop Functions =======================================================================================================

    private void Awake()
    {
        bool is2PlayerMode = GameManager.Instance.twoPlayerMode;

        if (!is2PlayerMode)
        {
            this.isAIEnabled = true;
            toggleAITurn.SetActive(true);
            this.isAITurn = this.isFirstAI;
        }

        Vector3 boardCentre = board.transform.position;
        initializeBoardMatrix(999);

        setButtonsOnBoard(boardCentre);

        setYourTurn();
        firstTurn = this.currentTurn;
    }

    private void Update()
    {
        if ((isButtonPressed || (isAITurn && isAIEnabled && gotUserFirstTurnChoice)) && !isGameOver)
        {


            gameOver();

            if (!isGameOver)
            {

                if (isFirstAI && firstTurnDone || !isFirstAI)
                {
                    currentTurn = (currentTurn == 0) ? 1 : 0;
                    setYourTurn();
                }

                if (isAIEnabled && isAITurn)
                {
                    itsAITurn();
                }
                else
                {
                    isButtonPressed = false;
                }

                firstTurnDone = true;
                if (firstTurnDone)
                {   
                    if (isAIEnabled)
                        setIsAIFirst();
                }
            }
        }
    }

//== Awake Fuctions =============================================================================================================

    void setButtonsOnBoard(Vector3 boardCentre)
    {
        int buttonNo = 0;
        for (int c = 1; c >= -1; c--)
        {
            for (int r = -1; r <= 1; r++)
            {
                Vector3 offset = new Vector3(r, c, 0) * (buttonPrefab.GetComponent<RectTransform>().sizeDelta.y) ;
                Button button = Instantiate(buttonPrefab, boardCentre + offset, Quaternion.identity, board.transform);
                buttons.Add(button);

                button.tag = buttonNo.ToString();
                buttonNo += 1;
            }
        }
    }

    void setYourTurn()
    {
        if (currentTurn == -1)
        {
            currentTurn = Mathf.RoundToInt(Random.value);
            yourTurn.text = (currentTurn == 1) ? "X" : "O";
        }
        else
        { 
            yourTurn.text = (currentTurn == 1) ? "X" : "O";
        }
    }

    void initializeBoardMatrix(int value)
    {
        for (int r = 0; r < boardMatrix.GetLength(0); r++)
        {
            for (int c = 0; c < boardMatrix.GetLength(1); c++)
            {
                boardMatrix[r, c] = value;
            }
        }
    }

    void printBoardMatrix()
    {
        for (int r = 0; r < this.boardMatrix.GetLength(0); r++)
        {
            string row = "";
            for (int c = 0; c < this.boardMatrix.GetLength(1); c++)
            {
                row += this.boardMatrix[r, c].ToString() + " ";
            }
            Debug.Log(row);
        }
    }

//== Update Functions ============================================================================================================

    bool checkTie()
    {
        if (!checkWin())
        {
            foreach (Button button in buttons)
            {
                if (button.IsInteractable())
                    return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    bool checkWin()
    {   
        for(int i = 0; i < 3; i++)
        {
            if (checkAxis(i, 0) || checkAxis(i, 1) || checkDiagonal(0) || checkDiagonal(1)) 
            {
                return true;
            }
        }
        return false;
    }

    bool gameOver()
    {
        if (checkTie())
        {
            yourTurn.text = "Tie";
            isGameOver = true;
            disableAllButtonInteractivity();
            return true;
        }
        if (checkWin())
        {
            yourTurn.text = ((this.currentTurn == 1) ? "X" : "O") + " Wins";
            isGameOver = true;
            disableAllButtonInteractivity();
            return true;
        }
        else
        {
            return false;
        }
    }

    void itsAITurn()
    { 
        MiniMax minimax = gameObject.GetComponent<MiniMax>();
        int[] tag_score = minimax.minimax(this.boardMatrix, 9, true, -999, 999);
        //Debug.Log("tag: " + tag_score[0] + ", score: " + tag_score[1]);
        buttons[tag_score[0]].GetComponent<ButtonScript>().setTextManually();
    }

    //== Utility Fuctions =====================================================================================================

    bool checkAxis(int startIndex, int axis)
    {
        int sumAxis = 0;
        for(int i = 0; i < 3; i++)
        {
            if(axis == 0)
                sumAxis += boardMatrix[i, startIndex];
            else if(axis == 1)
                sumAxis += boardMatrix[startIndex, i];
        }
        if (sumAxis == 3 || sumAxis == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool checkDiagonal(int axis)
    {
        int sumDiagonal = 0;
        int r = 0;
        int c = (axis == 1) ? 2 : 0;

        for (int i = 0; i < 3; i++)
        {
            sumDiagonal += boardMatrix[r, c];

            r += 1;
            c += (axis == 1) ? -1 : 1;
        }
        if (sumDiagonal == 3 || sumDiagonal == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void disableAllButtonInteractivity()
    {
        foreach (Button button in buttons)
        {
            if (button.IsInteractable())
            {
                button.interactable = false;
            }
        }
    }

    //== Toggle Fuctions =====================================================================================================
    public void setIsAIFirst()
    {
        if (!firstTurnDone)
        {
            isFirstAI = !isFirstAI;
            isAITurn = !isAITurn;

            GameObject toggleAITurn = GameObject.Find("AI Plays First");
            toggleAITurn.GetComponent<Toggle>().isOn = isFirstAI;

            FindObjectOfType<Toggle>().interactable = false;
            gotUserFirstTurnChoice = true;
        }
        else
        {
            FindObjectOfType<Toggle>().interactable = false;
        }
    }
}
