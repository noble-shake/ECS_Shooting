using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum StageState
{ 
    IDLE,
    ACCEL,
    SCROLL,
    STOPPING,
}


public class Stage01Scrolling : StageScript
{
    public StageState state;
    public List<ScrollObject> ScrollObjects;
    public float ScrollSpeed;

    public Vector3 scrollInit;
    public Vector3 scrollEnd;


    private bool scrollPlaying;
    private IEnumerator scrolling;

    private void Start()
    {
        state = StageState.SCROLL;
        if (ScrollObjects == null) ScrollObjects = new List<ScrollObject>();

        foreach (ScrollObject scrollObject in ScrollObjects) 
        {
            scrollObject.ScrollSpeed = ScrollSpeed;
            scrollObject.scrollInit = scrollInit;
            scrollObject.scrollEnd = scrollEnd;
        }
    }

    private void SetSpeed(float Speed = 0)
    {
        foreach (ScrollObject scrollObject in ScrollObjects)
        {
            scrollObject.ScrollSpeed = ScrollSpeed;
        }
    }

    public IEnumerator AccelSpeed(float _limit)
    {

        while (ScrollSpeed < _limit)
        {
            ScrollSpeed += Time.deltaTime;
            if (ScrollSpeed > _limit) ScrollSpeed = _limit;
            SetSpeed(ScrollSpeed);
            yield return 0;
        }

        ScrollUpdate(StageState.SCROLL, _limit);
        yield return 1;
    }

    public IEnumerator BreakSpeed()
    {

        while (ScrollSpeed > 0)
        {
            ScrollSpeed -= Time.deltaTime;
            if (ScrollSpeed < 0) ScrollSpeed = 0f;
            SetSpeed(ScrollSpeed);
            yield return 0;
        }

        ScrollUpdate(StageState.IDLE);
        yield return 1;
    }

    public void ScrollUpdate(StageState _state, float Speed = 0)
    {
        ScrollSpeed = Speed;
        state = _state;
        switch (state) 
        {
            case StageState.IDLE:
                
                SetSpeed();
                break;
            case StageState.ACCEL:
                AccelSpeed(Speed);
                break;
            case StageState.SCROLL:
                SetSpeed(ScrollSpeed);
                break;
            case StageState.STOPPING:
                BreakSpeed();
                break;
        }
    }

}
