using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class LevelLoadButton : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    public void LoadLevelByName(string name)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(name);
    }

    public void SavePlayerCount(int count)
    {
        PlayerPrefs.SetInt("PlayerCount", count);
        PlayerPrefs.Save();
    }
}
