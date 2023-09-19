using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelection : MonoBehaviour
{
    public Button[] selectionOptions;
    public int currentSelection = 0;

    void OnEnable()
    {
        InputManager.onPrimaryDown += SelectOption;
        InputManager.onMove += ChangeSelection;
    }

    void OnDisable()
    {
        InputManager.onPrimaryDown -= SelectOption;
        InputManager.onMove -= ChangeSelection;
    }

    void ChangeSelection(Vector2 changeDirection)
    {
        if (changeDirection == Vector2.zero) return;

        if (changeDirection.y < 0)
        {
            currentSelection = (currentSelection + 1) % selectionOptions.Length;
        }
        else
        {
            currentSelection--;
            if (currentSelection < 0) currentSelection = selectionOptions.Length - 1;
        }
        

    }
    void SelectOption()
    {
        selectionOptions[currentSelection].onClick.Invoke();
    }
}
