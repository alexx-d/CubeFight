using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.s_instance != null)
        {
            GameManager.s_instance.LevelCleared(collision.tag);
        }
    }
}