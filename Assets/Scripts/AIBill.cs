using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBill : MonoBehaviour
{
    private MouseInput mouseInput;
    private GameManager gameManager;
    private char ai = 'X';
    private char user = 'O';

    private class Move
    {
        public int index = 0;
        public int score = 0;
    }

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
            int bestMove = SimulatePP(gameManager.GetUsedPP(), ai);
            Debug.Log(bestMove);
            gameManager.PlacePP(ai, bestMove);
            mouseInput.canInteract = true;
        }
    }

    // returns the index of availible Piece Placements
    private int[] AvailPP(char[] board)
    {
        List<int> emptyIndexies = new List<int>();

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != 'X' && board[i] != 'O')
            {
                emptyIndexies.Add(i);
            }
        }

        return emptyIndexies.ToArray();
    }

    private int SimulatePP(char[] usedPP, char player)
    {
        //availible spots
        var availPP = AvailPP(usedPP);

        if (gameManager.IsWinner(ai, usedPP) == true)
        {
            //AIBill win statement
            return +10;
        }
        else if (gameManager.IsWinner(user, usedPP) == true)
        {
            //Player win statement
            return -10;
        }
        else if (availPP.Length == 0)
        {
            return 0;
        }

        // Create a Move array with the same length as availPP
        Move[] moves = new Move[availPP.Length];

        int bestMove = -1; // Initialize outside of the loop with an invalid value
        int bestScore;

        if (player == ai)
        {
            bestScore = -10000;
        }
        else
        {
            bestScore = 10000;
        }

        // Loop through the available Piece Placements
        for (int i = 0; i < availPP.Length; i++)
        {
            // Check if the move is valid
            if (gameManager.IsValidMove(availPP[i]))
            {
                //stores the index of that spot
                moves[i] = new Move();
                moves[i].index = usedPP[availPP[i]];

                //sets the empty spot to the current player
                usedPP[availPP[i]] = player;

                moves[i].score = SimulatePP(usedPP, (player == ai) ? user : ai);

                //reset spot to empty
                usedPP[availPP[i]] = ' ';

                if ((player == ai && moves[i].score > bestScore) || (player == user && moves[i].score < bestScore))
                {
                    bestScore = moves[i].score;
                    bestMove = availPP[i]; // Store the valid move
                }
            }
        }
        return bestMove;
    }
}
