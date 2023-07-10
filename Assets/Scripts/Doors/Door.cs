using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class Door : MonoBehaviour
{
    [SerializeField] private Transform _vfx;
    [SerializeField, Min(0f)] private float _closeTime = 1f;
    [SerializeField] private bool _startOpenState = false;
    [SerializeField] private Vector3 _openedPosition;
    [SerializeField] private Vector3 _closedPosition;
    
    public bool CurrentOpenState { get; private set; }

    private Tweener _closeTween;
    private Vector3 _startWorldPosition;

    protected virtual void Awake()
    {
        var renderer = _vfx.GetComponent<Renderer>();
        var material = Instantiate( renderer.material);
        material.SetVector("_TimeOffset", new Vector4(Random.Range(-50f,50f), Random.Range(-50f,50f), 0,0));
        renderer.material = material;
        
        var gameStarter = FindObjectOfType<GameStarterWindow>();
        gameStarter.GameStarted += () =>
        {
            CurrentOpenState = !_startOpenState;
            
            if (_startOpenState)
                Open();
            else
                Close();
        };
    }

    private void Start()
    {
        _startWorldPosition = transform.position;
    }

    [ContextMenu("Open")]
    public async void Open()
    {
        if (CurrentOpenState == true)
            return;

        _closeTween?.Kill();
        _closeTween = null;

        _closeTween = _vfx
            .DOLocalMove( _openedPosition, _closeTime);

        await _closeTween.AsyncWaitForCompletion();

        CurrentOpenState = true;
    }

    [ContextMenu("Close")]
    public async void Close()
    {
        if (CurrentOpenState == false)
            return;

        _closeTween?.Kill();
        _closeTween = null;
        
        _closeTween = _vfx
            .DOLocalMove( _closedPosition, _closeTime);

        await _closeTween.AsyncWaitForCompletion();

        CurrentOpenState = false;
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            _startWorldPosition = transform.position;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( _startWorldPosition + transform.rotation * _openedPosition, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( _startWorldPosition + transform.rotation *_closedPosition, 0.5f);
    }
    #endif
}