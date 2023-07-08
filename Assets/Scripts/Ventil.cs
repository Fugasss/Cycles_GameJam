using UnityEngine;

public class Ventil : MonoBehaviour
{
    [SerializeField] private Transform _rotatingElement;

    private Vector3 _startPosition;
    

    private void Awake()
    {
        _startPosition = transform.localPosition;
    }

    public void FlipDirection(bool flip)
    {
        transform.localRotation = Quaternion.Euler(0, 0, flip ? 180f : 0f);
        transform.localPosition = flip ? -_startPosition : _startPosition;
    }

    public void SetSpeed(float speed)
    {
        _rotatingElement.Rotate(0, 0, speed * Time.deltaTime);
    }
}