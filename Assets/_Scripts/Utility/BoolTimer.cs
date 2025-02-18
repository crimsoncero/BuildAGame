
using System;
using UnityEngine;

[Serializable]
public struct BoolTimer
{
    public bool Value;
    public float TimeToReset;

    private float _timeLeftToReset;


    public BoolTimer(bool initValue, float timeToReset)
    {
        Value = initValue;
        TimeToReset = timeToReset;
        _timeLeftToReset = timeToReset;
    }

    public void SetTimer(float timeToReset)
    {
        if (timeToReset <= 0) return;

        _timeLeftToReset = timeToReset;
        
        Value = false;
    }
       
    public void SetTimer()
    {
        SetTimer(TimeToReset);
    }
    public void UpdateTimer(bool useScaledTime = true, bool useFixedTime = false)
    {
        // No need to update
        if (Value) return;

        // Timer ended, set to true
        if (_timeLeftToReset <= 0)
        {
            Value = true;
            return;
        }
        
        // Progress timer according to the last tick
        if (useFixedTime)
        {
            if (useScaledTime)
                _timeLeftToReset -= Time.fixedDeltaTime;
            else
                _timeLeftToReset -= Time.fixedUnscaledDeltaTime;
        }
        else
        {
            if (useScaledTime)
                _timeLeftToReset -= Time.deltaTime;
            else
                _timeLeftToReset -= Time.unscaledDeltaTime;
        }
       
    }

}
