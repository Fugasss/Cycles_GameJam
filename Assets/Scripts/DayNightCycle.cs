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
        StartCycle();
    }

    public void StartCycle()
    {
        if (_cycleRoutine != null)
            StopCoroutine(_cycleRoutine);

        _cycleRoutine = StartCoroutine(Cycle());
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            Day?.Invoke();
            CurrentState = State.Day;
            yield return new WaitForSeconds(_dayTime);
            Night?.Invoke();
            CurrentState = State.Night;
            yield return new WaitForSeconds(_nightTime);
        }
    }


    public enum State
    {
        Day,
        Night
    }
}