using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class FadingObject : MonoBehaviour
{
    [SerializeField] private Image[] _images;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Fade(float value, float duration = 1f, Action onComplete = null)
    {
        bool complete = false;
        foreach (var image in _images)
        {
            image
                .DOFade(value, duration)
                .OnComplete(() =>
                {
                    if(complete) return;
                    complete = true;
                    onComplete?.Invoke();
                });
        }
    }
}