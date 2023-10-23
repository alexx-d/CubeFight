using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _startingPoints;
    [SerializeField] private List<Color> _playerColors;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private TextMeshProUGUI _timerText;

    private float _timer = 0;
    private bool _isCounting = false;


    public UIManager UiManager = null;
    public TextMeshProUGUI PlayerWinText;
    public TextMeshProUGUI PlayerWinScore;
    public int GameVictoryPageIndex = 0;
    public static GameManager s_instance = null;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount");

        InstantiatePlayers(playerCount);
        StartCounter();
    }

    public void LevelCleared(string player)
    {
        if (UiManager != null)
        {
            StopCounter();
            Time.timeScale = 0;

            UiManager.GoToPage(GameVictoryPageIndex);

            if (PlayerWinText != null && PlayerWinScore != null)
            {
                PlayerWinText.text = $"{player} Wins";
                PlayerWinScore.text = _timer.ToString("F2") + "s";
            }
        }
    }
    public void SaveScoreToLeaderboard(string playerName)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("leaderboard").Child(playerName).Child("name").SetValueAsync(playerName);
        reference.Child("leaderboard").Child(playerName).Child("time").SetValueAsync(_timer);
    }

    public void StartCounter()
    {
        _isCounting = true;
        StartCoroutine(Count());
    }

    public void StopCounter()
    {
        _isCounting = false;
    }

    private IEnumerator Count()
    {
        while (_isCounting)
        {
            _timer += Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }
    }

    private void UpdateTimerText()
    {
        if (_timerText != null)
        {
            _timerText.text = _timer.ToString("F2") + "s";
        }
    }

    private void InstantiatePlayers(int playerCount)
    {
        for (int i = 1; i <= playerCount; i++)
        {
            var player = PlayerInput.Instantiate(prefab: _playerPrefab, controlScheme: $"Keyboard{i}", pairWithDevice: Keyboard.current);
            player.tag = $"Player {i}";

            player.GetComponent<Renderer>().material.color = _playerColors[i - 1];
            player.transform.parent.position = _startingPoints[i - 1].position;
        }

        if (playerCount == 3)
        {
            GameObject player3 = GameObject.FindWithTag("Player 3");
            Camera childCamera = player3.transform.parent.Find("Camera").GetComponent<Camera>();
            childCamera.rect = new Rect(0f, 0f, 1f, 0.5f);
        }
    }
}
