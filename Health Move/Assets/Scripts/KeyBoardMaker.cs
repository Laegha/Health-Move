using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class KeyBoardMaker : MonoBehaviour
{
    [SerializeField] GameObject prefabKey;

    UnityEvent Event;

    char[] alfabetoEspañol = new char[]
    {
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
    'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's',
    't', 'u', 'v', 'w', 'x', 'y', 'z'
    };

    private void Start()
    {
        foreach(var character in alfabetoEspañol)
        {
            GameObject key = Instantiate(prefabKey, transform);
            TextMeshProUGUI text = key.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.text = character.ToString();
        }
    }
}
