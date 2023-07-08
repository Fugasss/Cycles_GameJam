using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class DayNightVolumeChanger : MonoBehaviour
{
    [SerializeField] private DayNightCycle _cycle;
    
    [SerializeField] private Volume _day;
    [SerializeField] private Volume _night;

    [SerializeField] private float _transitionDuration;
    

    private void Awake()
    {
        _cycle.Day += OnDay;
        _cycle.Night += OnNight;
    }

    private void OnDay()
    {
        ChangeVolumeWeight(_day, 1f);
        ChangeVolumeWeight(_night, 0f);
    }

    private void OnNight()
    {
        ChangeVolumeWeight(_day, 0f);
        ChangeVolumeWeight(_night, 1f);
    }

    private void ChangeVolumeWeight(Volume volume, float value)
    {
        DOTween.To(()=>volume.weight,x=>volume.weight = x, value, _transitionDuration).SetEase(Ease.InCubic);
    }
}
