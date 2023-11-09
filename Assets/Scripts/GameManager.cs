using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private Scene mainMenuScene;
    private Scene gameScene;

    private char ai = 'X';
    private char user = 'O';

    [Header("Placement Array")]
    [SerializeField] private GameObject[] pp = new GameObject[9];
    private char[] usedPP = new char[9] {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '};

    [Header("PiecePrefabs")]
    [SerializeField] private GameObject X;
    [SerializeField] private GameObject O;
    [SerializeField] private Text notValidText;

    protected override void Awake()
    {
        mainMenuScene = SceneManager.GetSceneByBuildIndex(0);
        gameScene = SceneManager.GetSceneByBuildIndex(1);

        if (mainMenuScene == SceneManager.GetActiveScene())
        {

        }
        if (gameScene == SceneManager.GetActiveScene())
        {
            
        }
    }

    public int[,] WinningCombinations = new int[8, 3]
    {
    {0, 1, 2}, // Top row
    {3, 4, 5}, // Middle row
    {6, 7, 8}, // Bottom row
    {0, 3, 6}, // Left column
    {1, 4, 7}, // Middle column
    {2, 5, 8}, // Right column
    {0, 4, 8}, // Diagonal from top-left to bottom-right
    {2, 4, 6}  // Diagonal from top-right to bottom-left
    };

    public void PlacePP(char XorO, int i)
    {
        if (!IsValidMove(i))
        {
            if (XorO == 'O')
            {
                StartCoroutine(NotValidMove());
            }
            return;
        }

        usedPP[i] = XorO;

        if (XorO == 'O')
        {
            GameObject temp = Instantiate(O);
            temp.transform.position = pp[i].transform.position;
            if (IsWinner(user, usedPP))
            {
                SceneManager.LoadScene("WinScene");
            }
            else if (FindObjectOfType<AIBill2>().AvailPP(usedPP).Length == 0)
            {
                SceneManager.LoadScene("TieScene");
            }
            else
            {
                FindObjectOfType<MouseInput>().SetCanInteract(false);
                Debug.Log("placing pp");
            }
        }
        else
        {
            GameObject temp = Instantiate(X);
            temp.transform.position = pp[i].transform.position;
            if (IsWinner(ai, usedPP))
            {
                SceneManager.LoadScene("AiWinScene");
            }
            else if (FindObjectOfType<AIBill2>().AvailPP(usedPP).Length == 0)
            {
                SceneManager.LoadScene("TieScene");
            }
            else
            {
                FindObjectOfType<MouseInput>().SetCanInteract(true);
                Debug.Log("placing pp");
            }
        }
    }

    public char[] GetUsedPP()
    {
        return usedPP;
    }

    public bool IsValidMove(int index)
    {
        if (usedPP[index] == ' ')
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWinner(char player, char[] board)
    {
        for (int i = 0; i < 8; i++)
        {
            int a = WinningCombinations[i, 0];
            int b = WinningCombinations[i, 1];
            int c = WinningCombinations[i, 2];

            if (board[a] == player && board[b] == player && board[c] == player)
            {
                return true;
            }
        }

        return false;
    }
    IEnumerator NotValidMove()
    {
        notValidText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        notValidText.gameObject.SetActive(false);
    }
}
