using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BochasPlayer : MonoBehaviour
{
    public enum BochasThrowingMode
    {
        Bochador,
        Arrimador,
        Medio
    }

    public BochasThrowingMode throwingMode = BochasThrowingMode.Arrimador;
}
