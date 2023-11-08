using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private Scene mainMenuScene;
    private Scene gameScene;

    [Header("Placement Array")]
    [SerializeField] private GameObject[] pp = new GameObject[9];
    private char[] usedPP = new char[9];

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
            pp = GameObject.FindGameObjectsWithTag("PiecePlacement");
        }
    }

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

        Debug.Log(usedPP[0] + " " + usedPP[1] + " " + usedPP[2] + " " + usedPP[3] + " " + usedPP[4] + " " + usedPP[5] + " " + usedPP[6] + " " + usedPP[7] + " " + usedPP[8]);

        if (XorO == 'O')
        {
            GameObject temp = Instantiate(O);
            temp.transform.position = pp[i].transform.position;
            FindObjectOfType<MouseInput>().SetCanInteract(false);
            Debug.Log("placing pp");
        }
        else
        {
            GameObject temp = Instantiate(X);
            temp.transform.position = pp[i].transform.position;
            FindObjectOfType<MouseInput>().SetCanInteract(true);
            Debug.Log("placing pp");
        }
    }

    public char[] GetUsedPP()
    {
        return usedPP;
    }

    public bool IsValidMove(int index)
    {
        if (usedPP[index] == '\0')
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWinner(char XorO, char[] board)
    {
        for (int i = 0; i < 3; i++)
        {
            //check vertical and horizontal lines
            if ((board[i] == XorO && board[i + 3] == XorO && board[i + 6] == XorO) || (board[i * 3] == XorO && board[i * 3 + 1] == XorO && board[i * 3 + 2] == XorO))
            {
                return true;
            }
        }
            //check diagonals
            if((board[0] == XorO && board[4] == XorO && board[8] == XorO) || (board[2] == XorO && board[4] == XorO && board[6] == XorO))
        {
            return true;
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
