using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Slider HealthBar;
    public PlayerBehaviourController Player;

    public void NotifyHealthPercent(float percent)
    {
        HealthBar.value = percent;
    }
}
