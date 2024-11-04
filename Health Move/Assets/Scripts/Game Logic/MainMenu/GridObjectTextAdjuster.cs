using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridObjectTextAdjuster : MonoBehaviour
{
    float _prevSize;
    Transform _parent;
    TextMeshProUGUI _thisText;
    [SerializeField] int _fontScaler = 6;
    
    private void Start()
    {
        _parent = transform.parent;    
        _thisText = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if(_prevSize != _parent.localScale.x)
        {
            _prevSize = _parent.localScale.x;
            _thisText.fontSize = _parent.localScale.x / _fontScaler;
        }
    }
}
