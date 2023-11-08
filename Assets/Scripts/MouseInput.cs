using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public bool canInteract = true;
    public delegate void CanInteractChanged(bool newValue);
    public event CanInteractChanged OnCanInteractChanged;

    public void SetCanInteract(bool newValue)
    {
        canInteract = newValue;
        if (canInteract == false)
        {
            OnCanInteractChanged?.Invoke(canInteract);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen position to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (canInteract && Input.GetMouseButtonDown(0))
        {
            Debug.Log(mousePosition);
            if (mousePosition.x >= -3 && mousePosition.x <= -1)
            {
                if (mousePosition.y <= 3 && mousePosition.y >= 1)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 0);
                }
                else if (mousePosition.y <= 1 && mousePosition.y >= -1)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 3);
                }
                else if (mousePosition.y <= -1 && mousePosition.y >= -3)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 6);
                }
            }
            else if (mousePosition.x >= -1 && mousePosition.x <= 1)
            {
                if (mousePosition.y <= 3 && mousePosition.y >= 1)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 1);
                }
                else if (mousePosition.y <= 1 && mousePosition.y >= -1)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 4);
                }
                else if (mousePosition.y <= -1 && mousePosition.y >= -3)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 7);
                }
            }
            else if (mousePosition.x >= 1 && mousePosition.x <= 3)
            {
                if (mousePosition.y <= 3 && mousePosition.y >= 1)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 2);
                }
                else if (mousePosition.y <= 1 && mousePosition.y >= -1)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 5);
                }
                else if (mousePosition.y <= -1 && mousePosition.y >= -3)
                {
                    FindObjectOfType<GameManager>().PlacePP('O', 8);
                }
            }
        }
    }
}
