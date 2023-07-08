using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarterWindow : MonoBehaviour, IWindow
{
    [SerializeField] private DayNightCycle _cycle;
    
    public void StartGame()
    {
        gameObject.SetActive(false);
        _cycle.StartCycle();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
