using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NightTimeZone : MonoBehaviour
{
    private DayNightCycle _cycle;
    
    private void Awake()
    {
        _cycle = FindObjectOfType<DayNightCycle>(true);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<Player>(out _)) return;
        
        _cycle.StopCycle();
        _cycle.SetNight();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.TryGetComponent<Player>(out _)) return;

        _cycle.StartCycle();
    }

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}