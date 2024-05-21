using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ButtonTestCode : MonoBehaviour
{
    IntPtr move;
    // Start is called before the first frame update
    void Start()
    {
        move = psmove_connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (psmove_poll(move) > 0)
        {
            uint buttons = psmove_get_buttons(move);
            print(buttons);
        }
    }


    [DllImport("libpsmoveapi")]
    private static extern IntPtr psmove_connect();

    [DllImport("libpsmoveapi")]
    private static extern uint psmove_poll(IntPtr move);

    [DllImport("libpsmoveapi")]
    private static extern uint psmove_get_buttons(IntPtr move);

}