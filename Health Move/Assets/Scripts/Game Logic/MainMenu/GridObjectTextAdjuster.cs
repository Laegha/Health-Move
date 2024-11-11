using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridObjectTextAdjuster : MonoBehaviour
{
    float _prevSize;
    [SerializeField] RectTransform _target;
    TextMeshProUGUI _thisText;
    [SerializeField] int _fontScaler = 6;
    
    private void Start()
    { 
        _thisText = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if(_prevSize != _target.rect.size.y)
        {
            _prevSize = _target.rect.size.y;
            _thisText.fontSize = _target.rect.size.y / _fontScaler;
        }
    }
}
