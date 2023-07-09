using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<Player>(out var player)) return;
        
        player.GameOver(true);
    }
}
