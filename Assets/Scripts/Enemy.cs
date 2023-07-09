using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[SelectionBase]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _vfx;
    [SerializeField] private Light2D _light;
    [SerializeField] private Vector3 _upVector = Vector3.up;
    [SerializeField, Min(0)] private float _detectionDistance = 1f;
    [SerializeField] private bool _patrol;
    [SerializeField] private bool _ignoreDay = false;

    private Player _player;
    private DayNightCycle _cycle;
    private NavMeshAgent _agent;

    private SpriteRenderer _renderer;
    private float _defaultLightIntensity;

    private bool _followPlayer = false;
    private Vector2 _startPosition;

    private void Awake()
    {
        _player = FindObjectOfType<Player>(true);
        _cycle = FindObjectOfType<DayNightCycle>(true);
        _renderer = _vfx.GetComponent<SpriteRenderer>();

        _cycle.Day += OnDay;
        _cycle.Night += OnNight;
        
        var gameStarter = FindObjectOfType<GameStarterWindow>();
        gameStarter.GameStarted += () =>
        {
            transform.position = _startPosition;
            _followPlayer = false;
            _agent.velocity = Vector3.zero;
            _agent.SetDestination(_startPosition);
        };
    }


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _defaultLightIntensity = _light.intensity;
        _startPosition = transform.position;
    }

    private const float ChangeDestinationTime = 1.5f;
    private float _time = 0f;

    private void Update()
    {
        if (_cycle.CurrentState == DayNightCycle.State.Day && !_ignoreDay) return;

        CalculateRotation();
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
        bool nearPlayer = distanceToPlayer <= _detectionDistance;

        if (!nearPlayer && !_followPlayer)
        {
            if (!_patrol)
                return;

            _time += Time.deltaTime;
            if (_time <= ChangeDestinationTime) return;

            _time = 0f;

            _agent.SetDestination(Random.insideUnitCircle + (Vector2) transform.position);

            return;
        }

        if (!_player.Detectable)
        {
            _followPlayer = false;
            return;
        }
        
        _followPlayer = true;
        _agent.SetDestination(_player.transform.position);
        _player.SetDistanceToNearestEnemy(distanceToPlayer, _detectionDistance);
    }

    private void CalculateRotation()
    {
        Vector2 velocity = _agent.velocity;

        bool flip = velocity.x > 0;
        float angle = Vector2.SignedAngle(_upVector, velocity);

        _renderer.flipX = flip;
        _vfx.rotation = Quaternion.Euler(0, 0, (flip ? angle + 180 : angle));
    }

    private void OnDay()
    {
        _followPlayer = false;
        
        DOTween
            .To(() => _light.intensity, x => _light.intensity = x, 0f, 1.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => _light.enabled = false);
    }

    private void OnNight()
    {
        _light.enabled = true;

        DOTween
            .To(() => _light.intensity, x => _light.intensity = x, _defaultLightIntensity, 1.5f)
            .SetEase(Ease.OutCubic);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(_cycle.CurrentState == DayNightCycle.State.Day) return;
        if(!col.collider.TryGetComponent<Player>(out var player)) return;
        if(!player.Detectable) return;

        player.GameOver();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionDistance);
    }
}