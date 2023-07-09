using System;
using UnityEngine;

public class GameOverWindow : MonoBehaviour, IWindow
{
    private void Awake()
    {
        var gameStarter = FindObjectOfType<GameStarterWindow>(true);
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