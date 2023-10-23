using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("Page Management")]
    [Tooltip("The pages (Panels) managed by the UI Manager")]
    public List<UIPage> pages;
    [Tooltip("The index of the active page in the UI")]
    public int currentPage = 0;
    [Tooltip("The page (by index) switched to when the UI Manager starts up")]
    public int defaultPage = 0;

    [Header("Pause Settings")]
    [Tooltip("The index of the pause page in the pages list")]
    public int pausePageIndex = 1;

    private bool isPaused = false;

    // A list of all UI element classes
    private List<UIelement> UIelements;

    // The event system handling UI navigation
    [HideInInspector]
    public EventSystem eventSystem;
    [SerializeField]
    private PlayerController playerController;

    private void OnEnable()
    {
        SetupGameManagerUIManager();
    }

    private void SetupGameManagerUIManager()
    {
        if (GameManager.s_instance != null && GameManager.s_instance.UiManager == null)
        {
            GameManager.s_instance.UiManager = this;
        }
    }

    private void SetUpUIElements()
    {
        UIelements = FindObjectsOfType<UIelement>().ToList();
    }

    private void SetUpEventSystem()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogWarning("There is no event system in the scene but you are trying to use the UIManager. /n" +
                "All UI in Unity requires an Event System to run. /n" +
                "You can add one by right clicking in hierarchy then selecting UI->EventSystem.");
        }
    }

    private void SetUpInputManager()
    {
        if (playerController == null)
        {
            playerController = PlayerController.instance;
        }
        if (playerController == null)
        {
            Debug.LogWarning("The UIManager is missing a reference to an Player Controller, without it the UI can not pause");
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            GoToPage(defaultPage);
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            GoToPage(pausePageIndex);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void UpdateUI()
    {
        foreach (UIelement uiElement in UIelements)
        {
            uiElement.UpdateUI();
        }
    }

    private void Start()
    {
        SetUpInputManager();
        SetUpEventSystem();
        SetUpUIElements();
        InitilizeFirstPage();
        UpdateUI();
    }

    private void InitilizeFirstPage()
    {
        GoToPage(defaultPage);
    }

    private void Update()
    {
        CheckPauseInput();
    }

    private void CheckPauseInput()
    {
        if (playerController != null)
        {
            if (playerController.pauseButton == 1)
            {
                TogglePause();
                playerController.pauseButton = 0;
            }
        }
    }

    public void GoToPage(int pageIndex)
    {
        if (pageIndex < pages.Count && pages[pageIndex] != null)
        {
            SetActiveAllPages(false);
            pages[pageIndex].gameObject.SetActive(true);
            pages[pageIndex].SetSelectedUIToDefault();
        }
    }

    public void GoToPageByName(string pageName)
    {
        UIPage page = pages.Find(item => item.name == pageName);
        int pageIndex = pages.IndexOf(page);
        GoToPage(pageIndex);
    }

    public void SetActiveAllPages(bool activated)
    {
        if (pages != null)
        {
            foreach (UIPage page in pages)
            {
                if (page != null)
                    page.gameObject.SetActive(activated);
            }
        }
    }
}
