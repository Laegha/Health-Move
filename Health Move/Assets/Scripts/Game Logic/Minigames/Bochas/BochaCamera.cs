using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochaCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _bochaVirtualCamera;
    float bochaCameraFocusTime = 2.5f;

    public IEnumerator FocusBocha(Action callback)
    {
        CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        cinemachineBrain.ActiveVirtualCamera.Priority = 0;
        _bochaVirtualCamera.Priority = 1;
        
        while(cinemachineBrain.IsBlending) yield return null;
        yield return new WaitForSeconds(bochaCameraFocusTime);
        
        callback();
    }
}
