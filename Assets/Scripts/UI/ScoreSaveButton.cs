using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSaveButton : MonoBehaviour
{
    [SerializeField] private TMP_InputField _input;

    public void SaveScore()
    {
        GameManager.s_instance.SaveScoreToLeaderboard(_input.text);
        _input.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
