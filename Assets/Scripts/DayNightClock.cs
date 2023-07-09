using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DayNightClock : MonoBehaviour
{
    [SerializeField] private RectTransform _clockVFX;

    private void Awake()
    {
        var cycle = FindObjectOfType<DayNightCycle>();

        cycle.Day += OnDay;
        cycle.Night += OnNight;
    }

    private void OnDay()
    {
        _clockVFX.DOLocalRotate(new Vector3(0, 0, 180f), 1f).SetEase(Ease.InOutBack);
    }

    private void OnNight()
    {
        _clockVFX.DOLocalRotate(new Vector3(0, 0, 0f), 1f).SetEase(Ease.InOutBack);
    }
}