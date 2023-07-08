using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool CurrentOpenState { get; private set; }

    [SerializeField] private Transform _vfx;
    [SerializeField, Min(0)] private float _closeTime = 1f;
    [SerializeField] private bool _startOpenState = false;
    [SerializeField] private Vector3 _openedPosition;
    [SerializeField] private Vector3 _closedPosition;

    private Tweener _closeTween;
    private BoxCollider2D _collider;
    private Vector3 _startWorldPosition;

    private void Awake()
    {
        _collider = _vfx.GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        CurrentOpenState = _startOpenState;
        _startWorldPosition = transform.position;

        if (_startOpenState)
            Open();
        else
            Close();
    }

    [ContextMenu("Open")]
    public async void Open()
    {
        if (CurrentOpenState == true)
            return;

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

        _closeTween = _vfx
            .DOLocalMove( _closedPosition, _closeTime);

        await _closeTween.AsyncWaitForCompletion();

        CurrentOpenState = false;
    }

    // private static Vector2 DirectionToVector2(Direction direction)
    // {
    //     return direction switch
    //     {
    //         Direction.Up => Vector2.up,
    //         Direction.Down => Vector2.down,
    //         Direction.Left => Vector2.left,
    //         Direction.Right => Vector2.right,
    //         _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    //     };
    // }
    //
    // private static Direction GetOppositeDirection(Direction direction)
    // {
    //     return direction switch
    //     {
    //         Direction.Up => Direction.Down,
    //         Direction.Down => Direction.Up,
    //         Direction.Left => Direction.Right,
    //         Direction.Right => Direction.Left,
    //         _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    //     };
    // }
    // public enum Direction
    // {
    //     Up,
    //     Down,
    //     Left,
    //     Right
    // }

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            _startWorldPosition = transform.position;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( _startWorldPosition + _openedPosition, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( _startWorldPosition + _closedPosition, 0.5f);
    }
}