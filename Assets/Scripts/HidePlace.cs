using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HidePlace : MonoBehaviour
{
    
    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<Player>(out var player)) return;
        player.Detectable = false;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if(!col.TryGetComponent<Player>(out var player)) return;
        player.Detectable = true;
    }
}
