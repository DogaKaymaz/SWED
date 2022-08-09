using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public int howManyTurn;
    public float time;
    private int topOff;
    public bool spinning;

    private void Start()
    {
        spinning = false;
        topOff = 0;
    }

    public void SpinWheel(int reachPoint, int howManySlot)
    {
        spinning = true;
        var rotateSum = 360 * howManyTurn + ((360 / howManySlot) * reachPoint) + ((360 / howManySlot) * topOff);
        topOff = howManySlot - reachPoint;
        transform.DOLocalRotate(new Vector3(0, 0, rotateSum), time, RotateMode.WorldAxisAdd).SetRelative(true)
            .OnComplete(() => spinning = false);
        
    }
}
