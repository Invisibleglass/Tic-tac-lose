using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBill2 : MonoBehaviour
{
    private MouseInput mouseInput;
    private GameManager gameManager;
    private char ai = 'X';
    private char user = 'O';

    // Start is called before the first frame update
    private void Start()
    {
        mouseInput = FindObjectOfType<MouseInput>();
        gameManager = GameManager.Instance;

        if (mouseInput != null)
        {
            mouseInput.OnCanInteractChanged += AIBillsTurn;
        }
    }

    private void AIBillsTurn(bool newValue)
    {
        if (!newValue)
        {
            int bestMove = Minimax(gameManager.GetUsedPP(), ai);
            Debug.Log(bestMove);
            gameManager.PlacePP(ai, bestMove);
            mouseInput.canInteract = true;
        }
    }

    private int Minimax(char[] board, char player)
    {
        int bestMove = -1;
        int bestScore = int.MinValue;

        var availPP = AvailPP(board);

        for (int i = 0; i < availPP.Length; i++)
        {
            int move = availPP[i];
            board[move] = player;

            // Check if the player (either AI or user) wins with this move
            if (gameManager.IsWinner(player, board))
            {
                board[move] = ' '; // Undo the move
                return move;
            }

            int score = -Evaluate(board, ai, user);
            board[move] = ' '; // Undo the move

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Evaluate(char[] board, char maximizingPlayer, char minimizingPlayer)
    {
        if (gameManager.IsWinner(maximizingPlayer, board))
        {
            return 10;
        }
        else if (gameManager.IsWinner(minimizingPlayer, board))
        {
            return -10;
        }
        else if (IsBoardFull(board))
        {
            return 0;
        }

        // Add additional evaluation logic here to prioritize center positions and blocking the opponent.
        int score = 0;

        // Prioritize center position
        if (board[4] == maximizingPlayer)
        {
            score += 2;
        }
        else if (board[4] == minimizingPlayer)
        {
            score -= 2;
        }
        else
        {
            score += 10;
        }

        // Check for possible winning moves by the opponent and prioritize blocking them.
        for (int i = 0; i < gameManager.WinningCombinations.GetLength(0); i++)
        {
            int countMaximizing = 0;
            int countMinimizing = 0;
            int emptyIndex = -1;

            for (int j = 0; j < 3; j++)
            {
                if (board[gameManager.WinningCombinations[i, j]] == maximizingPlayer)
                {
                    countMaximizing++;
                }
                else if (board[gameManager.WinningCombinations[i, j]] == minimizingPlayer)
                {
                    countMinimizing++;
                }
                else
                {
                    emptyIndex = gameManager.WinningCombinations[i, j];
                }
            }

            if (countMaximizing == 2 && countMinimizing == 0)
            {
                // Opponent has 2 in a row, prioritize blocking them.
                score -= 5;
            }

            if (countMinimizing == 2 && countMaximizing == 0)
            {
                // Opponent is close to winning, prioritize blocking them.
                score += 10;
            }
        }

        return score;
    }


    public int[] AvailPP(char[] board)
    {
        List<int> emptyIndices = new List<int>();

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != 'X' && board[i] != 'O')
            {
                emptyIndices.Add(i);
            }
        }

        return emptyIndices.ToArray();
    }

    private bool IsBoardFull(char[] board)
    {
        foreach (char cell in board)
        {
            if (cell == ' ')
            {
                return false; // The board is not full, as there's at least one empty spot.
            }
        }
        return true; // The board is full, as there are no empty spots.
    }
}
