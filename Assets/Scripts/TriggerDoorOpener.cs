using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerDoorOpener : DoorOpener
{
    protected override bool MatchRule(Collider2D col)
    {
        return col.TryGetComponent<Player>(out _);
    }
}