using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class Player : MonoBehaviour
{
    public bool Detectable { get; set; } = true;

    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private Transform _vfx;
    [SerializeField] private AudioSource _movementSoundSource;
    [SerializeField] private AudioSource _enemyNearnessSource;
    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;


    private Vector2 _input;
    private Vector2 _velocity;
    private Vector2 _toMouseDirection;
    private float _startCameraSize;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private Ventil _ventil;
    private GameOverWindow _gameOverWindow;
    private DayNightCycle _cycle;

    private void Awake()
    {
        _cycle = FindObjectOfType<DayNightCycle>();
        _gameOverWindow = FindObjectOfType<GameOverWindow>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = _vfx.GetComponent<SpriteRenderer>();
        _ventil = GetComponentInChildren<Ventil>();

        _cycle.Day += OnDay;
    }

    private void Start()
    {
        _startCameraSize = _camera.m_Lens.OrthographicSize;
    }

    private void OnDay()
    {
        _enemyNearnessSource.Stop();
        _lastDistance = 1000f;
        DOTween
            .To(() => _camera.m_Lens.OrthographicSize, (x) => _camera.m_Lens.OrthographicSize = x, _startCameraSize,
                .5f)
            .SetEase(Ease.InCubic);
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _input.Normalize();

        if (_input.sqrMagnitude != 0f)
        {
            if (!_movementSoundSource.isPlaying)
                _movementSoundSource.Play();
            _movementSoundSource.pitch = Mathf.MoveTowards(_movementSoundSource.pitch,
                Random.Range(_minPitch, _maxPitch), Time.deltaTime * 50f);
        }
        else
        {
            if (_movementSoundSource.isPlaying)
                _movementSoundSource.Stop();
        }

        _toMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _rigidbody.velocity = _input * _speed;

        bool flip = Vector2.Dot(Vector2.left, _toMouseDirection) > 0;
        float angle = Vector2.SignedAngle(Vector2.right, _toMouseDirection);

        float ventilSpeed = (flip ? -1 : 1) * (25f + _input.magnitude * 250f);
        _ventil.SetSpeed(ventilSpeed);
        _ventil.FlipDirection(flip);

        _renderer.flipX = flip;
        _vfx.rotation = Quaternion.Euler(0, 0, (flip ? angle + 180 : angle));
    }

    private const float SoundBeginDistance = 3f;
    private float _lastDistance = 1000f;

    public void SetDistanceToNearestEnemy(float distance)
    {
        if (!Detectable) return;

        if (distance < _lastDistance)
            _lastDistance = distance;

        if (_lastDistance <= SoundBeginDistance)
        {
            float volume = (SoundBeginDistance - _lastDistance) / SoundBeginDistance;

            _enemyNearnessSource.volume = Mathf.MoveTowards(_enemyNearnessSource.volume, volume, 3f * Time.deltaTime);
            _camera.m_Lens.OrthographicSize = Mathf.MoveTowards(_camera.m_Lens.OrthographicSize,
                                                                _startCameraSize * (1 - .7f * volume),
                                                                1f * Time.deltaTime);
        }

        if (_cycle.CurrentState == DayNightCycle.State.Day) return;
        if (_enemyNearnessSource.isPlaying) return;

        _enemyNearnessSource.volume = 0f;
        _enemyNearnessSource.Play();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        _gameOverWindow.Show();
    }
}