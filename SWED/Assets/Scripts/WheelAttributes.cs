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
    public int currentSpin;
    
    // the endpoint
    [SerializeField] private int maxSpinCount;
    
    // The switchpoints are where we upgrade the prizes.
    [SerializeField] private int bronzeSwitchPoint, silverSwitchPoint, goldSwitchPoint;
    
    // Whichever list you want for prize lists, we have bronze, silver, and gold here.
    [SerializeField] private PrizeList[] prizeLists;

    
    private SetPrize _setPrize;
    private Spinning _spinning;
    [SerializeField] private WheelIcons[] wheelIcons;
    private void Start()
    {
        _setPrize = GetComponent<SetPrize>();
        _spinning = GetComponent<Spinning>();
        currentSpin = 1;
    }

    private void Update()
    {
        if (!_spinning.spinning && Input.GetKeyDown(KeyCode.Space) && currentSpin < maxSpinCount)
        {
            SpinWheel();
        }
    }

    public async void SpinWheel()
    {
        _spinning.SpinWheel
            (_setPrize.earnedPrizeOrderInDisplayedList, 
            _setPrize.displayedPrizeNumber,
            _setPrize.earnedPrize);
        var tasks = new Task[6];
        tasks[0] = _setPrize.SetDisplayedList(ListRange());
        tasks[1] = SetSprites();
        tasks[2] = _setPrize.CalculateTotalWinningChance();
        tasks[3] = _setPrize.CreateLottoList();
        tasks[4] = _setPrize.SelectPrize();
        tasks[5] = _setPrize.PutDeathToList(currentSpin);

        await Task.WhenAll(tasks);
        
        currentSpin++;
    }
    
    private async Task SetSprites()
    {
        for (int i = 0; i < _setPrize.displayedList.Length; i++)
        {
            wheelIcons[i].SetSprite(_setPrize.displayedList[i].GetPrizeSprite());
        }
        await Task.Yield();
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
