using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMax : MonoBehaviour
{
    GameController gameController;

    private void Awake()
    {
        gameController = gameObject.GetComponent<GameController>();

        //int[,] boardMatrix = new int[3, 3]
        //{
        //    { 0, 0, 1},
        //    { 1, 1, 0},
        //    { 0, 1, 0},
        //};

        //int[] tagScore = minimax(boardMatrix, 9, 1);
        //Debug.Log("tag: " + tagScore[0] + ", score: " + tagScore[1]);
    }

    int getBoardScore(int[,] boardMatrix, bool tie, bool maximizingPlayer)
    {
        if (tie)
        {
            return 0;
        }
        else
        {
            //if ((maximizingPlayer == 1) ? true : false)
            //{
            //    return -1;
            //}
            if (maximizingPlayer)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    int[,] setBoardMatrix(int tag, int value, int[,] boardMatrix)
    {
        int r = tag / 3;
        int c = tag % 3;

        int[,] updatedBoardMatrix = (int[,])boardMatrix.Clone();
        updatedBoardMatrix[r, c] = value;

        //Debug.Log(tag);
        //Debug.Log(value);
        //printBoardMatrix(updatedBoardMatrix);
        //Debug.Log("\n\n\n\n");

        return updatedBoardMatrix;
    }

    public int[] minimax(int[,] boardMatrix, int depth, bool maximizingPlayer, int alpha, int beta, int chosenMove = -1)
    {
        List<int> possibleMoves = toPossibleMoves(boardMatrix);

        if (checkWin(boardMatrix) || checkTie(boardMatrix))
        {
            int value = getBoardScore(boardMatrix, checkTie(boardMatrix), maximizingPlayer);
            int[] tagValue = new int[] { chosenMove, value };

            return tagValue;
        }

        if (maximizingPlayer)
        {
            int[] tagScore = new int[] { -1, -999 };
            int previousScore = tagScore[1];

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                int[] tagValue = minimax(setBoardMatrix(possibleMoves[i], (maximizingPlayer) ? 1 : 0, boardMatrix), depth - 1, false, alpha, beta, possibleMoves[i]);

                tagScore[1] = Mathf.Max(tagValue[1], tagScore[1]);
                alpha = Mathf.Max(tagValue[1], alpha);

                if (tagScore[1] > previousScore)
                {
                    tagScore[0] = possibleMoves[i];
                }

                if (alpha >= beta)
                {
                    break;
                }

                previousScore = tagScore[1];
            }

            return tagScore;
        }
        else
        {
            int[] tagScore = new int[] { -1, 999 };
            int previousScore = tagScore[1];

            for (int i = 0; i < possibleMoves.Count; i++)
            {
                int[] tagValue = minimax(setBoardMatrix(possibleMoves[i], (maximizingPlayer) ? 1 : 0, boardMatrix), depth - 1, true, alpha, beta ,possibleMoves[i]);

                tagScore[1] = Mathf.Min(new int[] { tagScore[1], tagValue[1] });
                beta = Mathf.Min(tagValue[1], beta);

                if (tagScore[1] < previousScore)
                {
                    tagScore[0] = possibleMoves[i];
                }

                if (beta <= alpha)
                {
                    break;
                }

                previousScore = tagScore[1];
            }

            return tagScore;
        }
    }

    //== Game Over Functions =========================================================================================================================

    bool checkTie(int[,] boardMatrix)
    {
        if (!checkWin(boardMatrix))
        {
            for (int i = 0; i < 3; i++)
            {
                if (sumAxis(i, 0, boardMatrix) >= 999)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    bool checkWin(int[,] boardMatrix)
    {
        for (int i = 0; i < 3; i++)
        {
            if (checkHorizontal(i, 0, boardMatrix) || checkHorizontal(i, 1, boardMatrix) || checkDiagonal(0, boardMatrix) || checkDiagonal(1, boardMatrix))
            {
                return true;
            }
        }
        return false;
    }

    //== Utility Fuctions =====================================================================================================

    bool checkHorizontal(int startIndex, int axis, int[,] boardMatrix)
    {
        int sumGivenAxis = sumAxis(startIndex, axis, boardMatrix);
        
        if (sumGivenAxis == 3 || sumGivenAxis == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    int sumAxis(int startIndex, int axis, int[,] boardMatrix)
    {
        int sumAxis = 0;
        for (int i = 0; i < 3; i++)
        {
            if (axis == 0)
                sumAxis += boardMatrix[i, startIndex];
            else if (axis == 1)
                sumAxis += boardMatrix[startIndex, i];
        }
        return sumAxis;
    }

    bool checkDiagonal(int axis, int[,] boardMatrix)
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

    void printBoardMatrix(int[,] boardMatrix)
    {
        for (int r = 0; r < boardMatrix.GetLength(0); r++)
        {
            string row = "";
            for (int c = 0; c < boardMatrix.GetLength(1); c++)
            {
                row += boardMatrix[r, c].ToString() + " ";
            }
            Debug.Log(row);
        }
    }
    List<int> toPossibleMoves(int[,] boardMatrix)
    {
        List<int> possibleMoves = new List<int>();

        for(int r = 0; r < 3; r++)
        {
            for(int c = 0; c < 3; c++)
            {
                if (boardMatrix[r, c] == 999)
                {
                    int tag = (3 * r) + c;
                    possibleMoves.Add(tag);
                }
            }
        }

        return possibleMoves;
    }
}
