using UnityEngine;

public class NightClosingDoor : Door
{
    [SerializeField] private bool _openDuringNight;

    private void Awake()
    {
        var cycle = FindObjectOfType<DayNightCycle>();

        if (_openDuringNight)
        {
            cycle.Day += Close;
            cycle.Night += Open;
        }
        else
        {
            cycle.Day += Open;
            cycle.Night += Close;
        }
    }
}