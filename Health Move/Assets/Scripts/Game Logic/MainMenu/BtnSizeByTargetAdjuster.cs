using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSizeByTargetAdjuster : MonoBehaviour
{
    [SerializeField] RectTransform _target;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] Vector2 _resizeDivider;

    Vector2 _position;
    BoxCollider _thisCollider;
    Vector2 _previousSize;

    private void Awake()
    {
        _thisCollider = GetComponent<BoxCollider>();
        _rectTransform = GetComponent<RectTransform>();
        _position = _rectTransform.position;
        _previousSize = _target.rect.size;
    }

    void Update()
    {
        Vector2 targetSize = new Vector2(_target.rect.size.x, _target.rect.size.y);
        if(_previousSize != targetSize)
        {
            print(_previousSize);
            _previousSize = targetSize;
            Vector2 newSize = new Vector2(targetSize.x / _resizeDivider.x, targetSize.y / _resizeDivider.y);
            _rectTransform.rect.Set(_position.x, _position.y, newSize.x, newSize.y);
            if( _thisCollider != null ) 
                _thisCollider.size = newSize;
        }
        _previousSize = targetSize;
    }
}
