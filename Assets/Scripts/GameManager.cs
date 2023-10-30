using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    public GameObject GameOverUI;
    public Slider HealthBar;
    public PlayerBehaviourController Player;

    private void Awake()
    {
        GameOverUI.SetActive(false);
    }

    public void NotifyHealthPercent(float percent)
    {
        HealthBar.value = percent;
    }

    public void PlayerDeath()
    {
        Debug.Log("Game Over!");
        GameOverUI.SetActive(true);
        Invoke(nameof(Restart), 2f);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
