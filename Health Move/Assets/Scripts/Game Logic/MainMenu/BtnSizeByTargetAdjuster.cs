using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSizeByTargetAdjuster : MonoBehaviour
{
    [SerializeField] RectTransform _target;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] float _resizeDivider;

    BoxCollider _thisCollider;
    Vector2 _previousSize;

    private void Start()
    {
        _thisCollider = GetComponent<BoxCollider>();
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 targetSize = new Vector2(_target.rect.size.x, _target.rect.size.y);
        if(_previousSize != targetSize)
        {
            _previousSize = targetSize;
            Vector2 newSize = new Vector2(targetSize.x / _resizeDivider, targetSize.y);
            _rectTransform.rect.Set(_rectTransform.rect.position.x, _rectTransform.rect.position.y, newSize.x, newSize.y);
            _thisCollider.size = newSize;
        }
    }
}
