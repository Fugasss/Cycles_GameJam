using System;
using UnityEngine;

public class GameStarterWindow : MonoBehaviour, IWindow
{
    public event Action GameStarted;
    
    [SerializeField] private DayNightCycle _cycle;
    
    public void StartGame()
    {
        Time.timeScale = 1f;

        gameObject.SetActive(false);
        GameStarted?.Invoke();
        _cycle.StartCycle();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
