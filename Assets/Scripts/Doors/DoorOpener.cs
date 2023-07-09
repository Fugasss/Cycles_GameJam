using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[SelectionBase]
public abstract class DoorOpener : MonoBehaviour
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private bool _openDoor;

    protected abstract bool MatchRule(Collider2D col);

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (!MatchRule(col)) return;

        if (_openDoor)
        {
            foreach (var door in _doors)
            {
                door.Open();
            }
        }
        else
        {
            foreach (var door in _doors)
            {
                door.Close();
            }
        }
    }
}