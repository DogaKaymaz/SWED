using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public int howManyTurn;
    public float time;
    public int topOff;
    public bool spinning;

    private void Start()
    {
        spinning = false;
        topOff = 0;
    }

    public void SpinWheel(int reachPoint, int howManySlot)
    {
        spinning = true;
        var rotateSum = 360 * howManyTurn + ((360 / howManySlot) * reachPoint) + (topOff * (360 / howManySlot));
        transform.DOLocalRotate(new Vector3(0, 0, rotateSum), time, RotateMode.WorldAxisAdd).SetRelative(true)
            .OnComplete(() => SpinningEnded(reachPoint, howManySlot));

    }

    private void SpinningEnded(int reachPoint, int howManySlot)
    {
        spinning = false;
        topOff = howManySlot - reachPoint;
    }
}