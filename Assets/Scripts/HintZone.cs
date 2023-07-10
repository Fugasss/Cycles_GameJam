using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HintZone : MonoBehaviour
{
    [SerializeField] private FadingObject _hintObject;
    [SerializeField] private Door _door;

    [SerializeField, Min(0f)] private float _hintDelay = 1f;

    private bool _inZone = false;
    private float _time = 0f;

    private void Update()
    {
        if (!_inZone) return;
        if (_hintObject.gameObject.activeInHierarchy) return;
        _time += Time.deltaTime;

        if (_time < _hintDelay) return;
        _hintObject.gameObject.SetActive(true);
        _hintObject.Fade(1f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<Player>(out _)) return;
        if (_door.CurrentOpenState) return;

        _inZone = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.TryGetComponent<Player>(out _)) return;
        _inZone = false;
        _time = 0f;
        _hintObject.Fade(0f, 1f, () => _hintObject.gameObject.SetActive(false));
    }

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}