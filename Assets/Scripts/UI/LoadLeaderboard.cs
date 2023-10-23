using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LoadLeaderboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tableText;
    [SerializeField] private int _numberOfScoresToLoad = 10;

    void Start()
    {
        LoadTopScores(_numberOfScoresToLoad);
    }

    public void LoadScores()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("leaderboard").OrderByChild("time").ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            string keyValueText = "";

            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                foreach (DataSnapshot snapshot in args.Snapshot.Children)
                {
                    string playerName = snapshot.Child("name").Value.ToString();
                    float time = float.Parse(snapshot.Child("time").Value.ToString());

                    keyValueText += playerName + " : " + time.ToString("F2") + "s" + "\n";
                }

                _tableText.text = keyValueText;
            }
        };
    }

    public void LoadTopScores(int numberOfScoresToLoad)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("leaderboard").OrderByChild("time").LimitToFirst(numberOfScoresToLoad).ValueChanged += (object sender, ValueChangedEventArgs args) =>
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError(args.DatabaseError.Message);
                return;
            }

            string keyValueText = "";

            if (args.Snapshot != null && args.Snapshot.ChildrenCount > 0)
            {
                foreach (DataSnapshot snapshot in args.Snapshot.Children)
                {
                    string playerName = snapshot.Child("name").Value.ToString();
                    float time = float.Parse(snapshot.Child("time").Value.ToString());

                    keyValueText += playerName + " : " + time.ToString("F2") + "s" + "\n";
                }

                _tableText.text = keyValueText;
            }
        };
    }
}
