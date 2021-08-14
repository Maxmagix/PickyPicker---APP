using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock
{
    private float _timerStart;

    public void Reset()
    {
        _timerStart = Time.time;
    }

    public float ElapsedTime()
    {
        float elapsedTime = Time.time - _timerStart;

        return elapsedTime;
    }
}