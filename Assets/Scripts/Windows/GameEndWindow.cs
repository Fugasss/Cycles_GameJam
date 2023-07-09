using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndWindow : MonoBehaviour, IWindow
{
    private void Awake()
    {
        var gameStarter = FindObjectOfType<GameStarterWindow>();
        gameStarter.GameStarted += () =>
        {
            gameObject.SetActive(false);
        };
    }
    
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.1f;
    }
}
