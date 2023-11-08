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
        public int index;
        public int score;
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
        if(!newValue)
        {
            int bestMove = BestPP(gameManager.GetUsedPP());
            Debug.Log(bestMove);
            gameManager.PlacePP(ai, bestMove);
            mouseInput.canInteract = true;
        }
    }

    private int SimulatePP(char[] usedPP, char player)
    {
        int possibleMoves = 0;
        for (int i = 0; i < usedPP.Length; i++)
        {
            if(usedPP[i] == '\0')
            {
                possibleMoves++;
            }    
        }

        if (gameManager.IsWinner(ai, usedPP) == true)
        {
            //AIBill win statement
            return -1;
        }
        else if (gameManager.IsWinner(user, usedPP) == true)
        {
            //Player win statement
            return +1;
        }
        else if (possibleMoves == 0)
        {
            return 0;
        }

        int bestScore = (player == user) ? int.MinValue : int.MaxValue;

        for (int i = 0; i < usedPP.Length; i++)
        {
            if (usedPP[i] == '\0')
            {
                usedPP[i] = player;
                int score = SimulatePP(usedPP,(player == user) ? ai:user);
                usedPP[i] = '\0';

                if (player == user)
                {
                    bestScore = Mathf.Min(bestScore, score);
                }
                else
                {
                    bestScore = Mathf.Max(bestScore, score);
                }
            }
        }
        return bestScore;
    }

    private int BestPP(char[] usedPP)
    {
        int bestScore = -1;
        int bestPos = -1;
        int score;

        for (int i = 0; i < usedPP.Length; i++)
        {
            if (usedPP[i] == '\0')
            {
                usedPP[i] = ai;
                score = SimulatePP(usedPP, ai);
                usedPP[i] = '\0';

                if (bestScore < score)
                {
                    bestScore = score;
                    bestPos = i;
                }
            }
        }
        return bestPos;
    }

}
