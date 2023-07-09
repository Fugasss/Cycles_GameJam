using System;
using System.Collections;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public State CurrentState { get; private set; }= State.Day;

    public event Action Day;
    public event Action Night;

    [SerializeField, Min(0)] private float _dayTime;
    [SerializeField, Min(0)] private float _nightTime;

    private Coroutine _cycleRoutine;

    private void Start()
    {
        SetNight();
    }

    public void StartCycle()
    {
        StopCycle();

        _cycleRoutine = StartCoroutine(Cycle());
    }

    public void StopCycle()
    {
        if (_cycleRoutine != null)
            StopCoroutine(_cycleRoutine);
    }

    public void SetDay()
    {
        Day?.Invoke();
        CurrentState = State.Day;
    }

    public void SetNight()
    {
        Night?.Invoke();
        CurrentState = State.Night;
    }
    
    private IEnumerator Cycle()
    {
        while (true)
        {
            SetDay();
            yield return new WaitForSeconds(_dayTime);
            SetNight();
            yield return new WaitForSeconds(_nightTime);
        }
    }


    public enum State
    {
        Day,
        Night
    }
}