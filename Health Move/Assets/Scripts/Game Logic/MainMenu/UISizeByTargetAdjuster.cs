using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISizeByTargetAdjuster : MonoBehaviour
{
    [SerializeField] RectTransform _target;
    RectTransform _rectTransform;
    [SerializeField] Vector2 _resizeDivider;

    BoxCollider _thisCollider;
    Vector2 _previousSize;

    private void Awake()
    {
        _thisCollider = GetComponent<BoxCollider>();
        _rectTransform = GetComponent<RectTransform>();
        _previousSize = _target.rect.size;
    }

    void Update()
    {
        Vector2 targetSize = new Vector2(_target.rect.size.x, _target.rect.size.y);
        if (_previousSize != targetSize)
        {
            _previousSize = targetSize;
            Vector2 newSize = new Vector2(targetSize.x / _resizeDivider.x, targetSize.y / _resizeDivider.y);
            _rectTransform.sizeDelta = newSize;
            if( _thisCollider != null ) 
                _thisCollider.size = newSize;
        }
    }
}
