using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionBySizeAdjuster : MonoBehaviour
{
    [SerializeField] Vector2 _positionMultiplier;
    [SerializeField] RectTransform _target;
    Vector2 _prevSize;
    RectTransform _rectTransform;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _prevSize = _target.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        if(_target.sizeDelta != _prevSize)
        {
            _prevSize = _target.sizeDelta;
            print(_rectTransform.rect.width * _positionMultiplier.x);
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.sizeDelta.x * _positionMultiplier.x, _rectTransform.sizeDelta.y * _positionMultiplier.y);
        }
    }
}
