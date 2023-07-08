using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public bool Detectable { get; set; } = true;
    
    [SerializeField] private float _speed = 1.5f;
    [SerializeField] private Transform _vfx;
    
    
    private Rigidbody2D _rigidbody;

    private Vector2 _input;
    private Vector2 _velocity;
    private Vector2 _toMouseDirection;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = _vfx.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _input.Normalize();

        _toMouseDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        _rigidbody.velocity = _input * _speed;


        bool flip = Vector2.Dot(Vector2.left, _toMouseDirection) > 0; 
        float angle = Vector2.SignedAngle(Vector2.right, _toMouseDirection);

        _renderer.flipX = flip;
        _vfx.rotation = Quaternion.Euler(0, 0, (flip ? angle + 180: angle));
    }
}