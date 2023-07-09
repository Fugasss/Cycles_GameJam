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
    private Vector2 _toMouseDirection;
    private float _startCameraSize;
    private Vector2 _startPosition;
    
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private Ventil _ventil;
    private GameOverWindow _gameOverWindow;
    private GameEndWindow _gameEndWindow ;

    private DayNightCycle _cycle;

    private bool _gameOver = false;
    
    private void Awake()
    {
        _cycle = FindObjectOfType<DayNightCycle>(true);
        _gameOverWindow = FindObjectOfType<GameOverWindow>(true);
        _gameEndWindow = FindObjectOfType<GameEndWindow>(true);
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = _vfx.GetComponent<SpriteRenderer>();
        _ventil = GetComponentInChildren<Ventil>();

        _cycle.Day += OnDay;

        _gameOver = true;
        
        var gameStarter = FindObjectOfType<GameStarterWindow>();
        gameStarter.GameStarted += () =>
        {
            transform.position = _startPosition;
            _camera.m_Lens.OrthographicSize = _startCameraSize;
            _input = Vector2.zero;
            _toMouseDirection = Vector2.right;
            _gameOver = false;
            Detectable = true;
        };
    }

    private void Start()
    {
        _startPosition = transform.position;
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
        if(_gameOver) return; 
        
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
    
    private float _lastDistance = 1000f;

    public void SetDistanceToNearestEnemy(float distance, float detectionDistance)
    {
        if (!Detectable) return;
        
        if (_cycle.CurrentState == DayNightCycle.State.Night)
        {
            if (!_enemyNearnessSource.isPlaying)
            {
                _enemyNearnessSource.volume = 0f;
                _enemyNearnessSource.Play();
            }
        }
        
        if (distance < _lastDistance)
            _lastDistance = distance;
        
        if (_lastDistance <= detectionDistance)
        {
            float volume = (detectionDistance - _lastDistance) / detectionDistance;

            _enemyNearnessSource.volume = Mathf.MoveTowards(_enemyNearnessSource.volume, volume, 3f * Time.deltaTime);
            _camera.m_Lens.OrthographicSize = Mathf.MoveTowards(_camera.m_Lens.OrthographicSize,
                                                                _startCameraSize * (1 - .7f * volume),
                                                                1f * Time.deltaTime);
        }
    }

    public void GameOver(bool win = false)
    {
        _gameOver = true;
        Detectable = false;
        
        _enemyNearnessSource.Stop();
        _movementSoundSource.Stop();

        if (win)
            _gameEndWindow.Show();
        else
            _gameOverWindow.Show();
    }
}