using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
    public event System.Action<float> TimeChangeEvent;
    public event System.Action OutOfTimeEvent;

    public float StopTime;

    float _remainingTime;
    public float RemainingTime
    {
        get { return _remainingTime; }
        private set
        {
            if (_remainingTime != value)
            {
                _remainingTime = value;
                if (TimeChangeEvent != null)
                    TimeChangeEvent(_remainingTime);
            }
        }
    }

    bool counting;

	void Update () 
    {
        if (counting)
        {
            RemainingTime -= Time.deltaTime;

            if (RemainingTime < 0)
            {
                if (OutOfTimeEvent != null)
                {
                    OutOfTimeEvent();
                }
                counting = false;
            }
        }
	}

    void Start() 
    {
        StopTimer();
    }

    public void StartTimer() 
    {
        counting = true;
        RemainingTime = StopTime;
    }

    public void StopTimer() 
    {
        counting = false;
    }

    public void Switch() 
    {
        if (counting)
        {
            StopTimer();
        }
        else 
        {
            StartTimer();
        }
    }
}
