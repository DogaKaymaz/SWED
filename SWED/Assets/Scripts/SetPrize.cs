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
    public int displayedPrizeNumber;

    public Prize[] displayedList;

    public string[] lottoList;
    
    float totalWinningChances; // Σ winning chances of chosen (b/s/g) list
    public int earnedPrizeOrderInDisplayedList;
    
    // Since there are some safe areas without death, I added it to the final list => displayedList.
    // It's winning chance is 1/displayedPrizeNumber here but, there is a chance I can change it and
    // add it to the lotto list for the lottery at some point. It is a Prize scriptable object,
    // so there is a custom winning chance for death too.
    [SerializeField] private Prize death;

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
        } await Task.Yield();
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
                earnedPrizeOrderInDisplayedList = i;
                Debug.Log(earnedPrize);
            }
        }
        await Task.Yield();
    }

    public async Task PutDeathToList(int currentSpin)
    {
        if (currentSpin % 5 == 0 || currentSpin == 1) return;
        displayedList[Random.Range(0, displayedPrizeNumber)] = death;
        await Task.Yield();
    }
    
}
