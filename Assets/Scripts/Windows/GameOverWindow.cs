using System;
using UnityEngine;

public class GameOverWindow : MonoBehaviour, IWindow
{
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