using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[SelectionBase]
public class Player : MonoBehaviour
{
    public bool Detectable { get; set; } = true;

    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private Transform _vfx;
    [SerializeField] private AudioSource _source;

    [SerializeField] private float _minPitch;
    [SerializeField] private float _maxPitch;

    private Rigidbody2D _rigidbody;

    private Vector2 _input;
    private Vector2 _velocity;
    private Vector2 _toMouseDirection;
    private SpriteRenderer _renderer;
    private Ventil _ventil;
    private GameOverWindow _gameOverWindow;

    private void Awake()
    {
        _gameOverWindow = FindObjectOfType<GameOverWindow>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = _vfx.GetComponent<SpriteRenderer>();
        _ventil = GetComponentInChildren<Ventil>();
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _input.Normalize();

        if (_input.sqrMagnitude != 0f)
        {
            if (!_source.isPlaying)
                _source.Play();
            _source.pitch = Mathf.MoveTowards(_source.pitch, Random.Range(_minPitch, _maxPitch), Time.deltaTime * 50f);
        }
        else
        {
            if (_source.isPlaying)
                _source.Stop();
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

    public void GameOver()
    {
        Debug.Log("Game Over");
        _gameOverWindow.Show();
    }
}