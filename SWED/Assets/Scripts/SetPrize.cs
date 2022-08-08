using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SetPrize : MonoBehaviour
{
    // lottery result
    public Prize earnedPrize;
    
    // There is 8 slot currently but I made it changeable.
    [SerializeField] private int displayedPrizeNumber;

    public Prize[] displayedList;

    public string[] lottoList;
    
    float totalWinningChances; // Σ winning chances of chosen (b/s/g) list

    private void Start()
    {
        Random.InitState((int) DateTime.Now.Ticks); // to take different prizes to displayed last at possible.
        displayedList = new Prize[displayedPrizeNumber];
    }

    public async Task SetDisplayedList(Prize[] prizeList)
    {
        Debug.Log(prizeList);
       for(int i = 0; i < displayedPrizeNumber; i++)
        {
            var randomNumber = Random.Range(0, prizeList.Length);
            displayedList[i] = prizeList[randomNumber];
        }
       await Task.Yield();
    }

    public async Task CalculateTotalWinningChance()
    {
        lottoList = null;
        totalWinningChances = 0;
        for (int i = 0; i < displayedList.Length; i++)
        {
            Debug.Log(displayedList[i]);
            totalWinningChances += displayedList[i].winningChance;
        }

        lottoList = new string[totalWinningChances.ConvertTo<int>()];
        await Task.Yield();
    }

    public async Task CreateLottoList()
    {
        totalWinningChances = 0;
        for (int i = 0; i < displayedList.Length; i++)
        {
            for (int k = 0; k < displayedList[i].winningChance; k++)
            {
                lottoList[totalWinningChances.ConvertTo<int>()] = displayedList[i].id;
                totalWinningChances++;
            }
        }
        await Task.Yield();
    }

    public async Task SelectPrize()
    {
        int winnerID = Random.Range(0, lottoList.Length);
        
        for (int i = 0; i < displayedList.Length; i++)
        {
            if (displayedList[i].id == lottoList[winnerID])
            {
                earnedPrize = displayedList[i];
                Debug.Log(earnedPrize);
            }
        }
        await Task.Yield();
    }
    
}
