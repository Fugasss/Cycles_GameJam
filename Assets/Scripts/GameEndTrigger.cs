using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameEndTrigger : MonoBehaviour
{
    private GameEndWindow _gameEndWindow ;

    private void Awake()
    {
        _gameEndWindow = GetComponent<GameEndWindow>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!col.TryGetComponent<Player>(out _)) return;
        
        _gameEndWindow.Show();
    }
}
