using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPage : MonoBehaviour
{
    [Tooltip("The default UI to have selected when opening this page")]
    public GameObject defaultSelected;

    public void SetSelectedUIToDefault()
    {
        if (GameManager.s_instance != null && GameManager.s_instance.UiManager != null && defaultSelected != null)
        {
            GameManager.s_instance.UiManager.eventSystem.SetSelectedGameObject(null);
            GameManager.s_instance.UiManager.eventSystem.SetSelectedGameObject(defaultSelected);
        }

    }
}
