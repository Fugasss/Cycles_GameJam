using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HidePlace : MonoBehaviour
{
    
    private void Awake()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            var material = Instantiate( renderer.material);
            material.SetVector("_TimeOffset", new Vector4(Random.Range(-50f,50f), Random.Range(-50f,50f), 0,0));
            renderer.material = material;
        }
    }
    
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
