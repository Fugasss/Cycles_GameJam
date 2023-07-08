using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _vfx;
    [SerializeField] private Vector3 _upVector = Vector3.up;
    [SerializeField,Min(0)] private float _detectionDistance = 1f;
    [SerializeField] private bool _patrol;

    private Player _player;
    private DayNightCycle _cycle;
    private NavMeshAgent _agent;

    private SpriteRenderer _renderer;


    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _cycle = FindObjectOfType<DayNightCycle>();
        _renderer = _vfx.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_cycle.CurrentState == DayNightCycle.State.Day) return;
        if (!_player.Detectable) return;
        if(Vector2.Distance(transform.position, _player.transform.position) > _detectionDistance) return;
        
        _agent.SetDestination(_player.transform.position);

        bool flip = _agent.velocity.x > 0;
        float angle = Vector2.SignedAngle(_upVector, _agent.velocity);
        
        _renderer.flipX = flip;
        _vfx.rotation = Quaternion.Euler(0, 0, (flip ? angle + 180: angle));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionDistance);
    }
}