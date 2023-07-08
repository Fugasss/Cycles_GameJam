using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class DoorOpener : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private bool _openDoor;

    protected abstract bool MatchRule(Collider2D col);
    
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(!MatchRule(col)) 
            return;
        
        if(_openDoor) _door.Open();
        else _door.Close();
        
    }
}