using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WheelAttributes : MonoBehaviour
{
    public bool spinning;
    public int currentSpin;

    // Since there are some safe areas without death, I added it to the final list => displayedList.
    // It's winning chance is 1/displayedPrizeNumber here but, there is a chance I can change it and
    // add it to the lotto list for the lottery at some point. It is a Prize scriptable object,
    // so there is a custom winning chance for death too.
    [SerializeField] private Prize death;
    
    // the endpoint
    [SerializeField] private int maxSpinCount;
    
    // The switchpoints are where we upgrade the prizes.
    [SerializeField] private int bronzeSwitchPoint, silverSwitchPoint, goldSwitchPoint;
    
    // Whichever list you want for prize lists, we have bronze, silver, and gold here.
    [SerializeField] private PrizeList[] prizeLists;

    
    private SetPrize _setPrize;
    private void Start()
    {
        _setPrize = GetComponent<SetPrize>();
        currentSpin = 1;
    }

    private void Update()
    {
        if (!spinning && Input.GetKeyDown(KeyCode.Space) && currentSpin < maxSpinCount)
        {
            SpinWheel();
            currentSpin++;
        }
    }

    public async void SpinWheel()
    {
        spinning = true;

        var tasks = new Task[4];
        tasks[0] = _setPrize.SetDisplayedList(ListRange());
        tasks[1] = _setPrize.CalculateTotalWinningChance();
        tasks[2] = _setPrize.CreateLottoList();
        tasks[3] = _setPrize.SelectPrize();

        await Task.WhenAll(tasks);

        spinning = false;
    }

    void SelectPrize(Prize[] currentList)
    { 
        if (currentSpin % 5 == 0 || currentSpin == 1)
        {
        }
    }

    // This method is the part of the code that returns the correct list.
    // It's 1-10 bronze; 10-20 silver; 20-30 gold for now.
    public Prize[] ListRange()
    {
        if (currentSpin > 0 && currentSpin <= bronzeSwitchPoint) return prizeLists[0].prizeList;
        else if (currentSpin > bronzeSwitchPoint && currentSpin <= silverSwitchPoint) return prizeLists[1].prizeList;
        else if (currentSpin > silverSwitchPoint && currentSpin <= goldSwitchPoint) return prizeLists[2].prizeList;
        else return null;
    }
}
