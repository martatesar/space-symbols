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

    bool Counting;

	void Update () 
    {
        if (Counting)
        {
            RemainingTime -= Time.deltaTime;

            if (RemainingTime < 0)
            {
                if (OutOfTimeEvent != null)
                {
                    OutOfTimeEvent();
                }
            }
        }
	}

    public void Start() 
    {
        StopTimer();
    }

    public void StartTimer() 
    {
        Counting = true;
        RemainingTime = StopTime;
    }

    public void StopTimer() 
    {
        Counting = false;
    }

    public void Switch() 
    {
        if (Counting)
        {
            StopTimer();
        }
        else 
        {
            StartTimer();
        }
    }
}
