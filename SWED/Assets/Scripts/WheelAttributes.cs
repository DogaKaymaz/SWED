using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WheelAttributes : MonoBehaviour
{
    public int currentSpin;
    
    // the endpoint
    [SerializeField] private int maxSpinCount;
    
    // The switchpoints are where we upgrade the prizes.
    [SerializeField] private int bronzeSwitchPoint, silverSwitchPoint, goldSwitchPoint;

    [SerializeField] private SpriteAtlas spinAtlas;
    [SerializeField] private string[] spinBase, spinIndicator;

    // Whichever list you want for prize lists, we have bronze, silver, and gold here.
    [SerializeField] private PrizeList[] prizeLists;
    
    [SerializeField] private CardPanel[] cardPanel;

    [SerializeField] private Button spinButton;

    private SpriteRenderer _spinBaseRenderer, _spinIndicatorRenderer;
    
    
    private CardCanvasManager _cardCanvasManager;
    private SetPrize _setPrize;
    private Spinning _spinning;
    [SerializeField] private WheelIcons[] wheelIcons;

    private void Start()
    {
        _cardCanvasManager = GetComponent<CardCanvasManager>();
        _setPrize = GetComponent<SetPrize>();
        _spinning = GetComponent<Spinning>();
        _spinBaseRenderer = GetComponent<SpriteRenderer>();
        _spinIndicatorRenderer = GameObject.Find("ui_spin_indicator_value").GetComponent<SpriteRenderer>();
        currentSpin = 0;
    }

    private void Update()
    {
        spinButton.onClick.AddListener(() => SpinWheel());

    }

    public async void SpinWheel()
    {
        if (!_spinning.spinning && currentSpin < maxSpinCount)
        {
            currentSpin++;
            
            _spinning.SpinWheel
            (_setPrize.earnedPrizeOrderInDisplayedList, 
            _setPrize.displayedPrizeNumber);
            
        var tasks = new Task[5];
        tasks[0] = _setPrize.SetDisplayedList(ListRange(), currentSpin);
        tasks[1] = SetSprites();
        tasks[2] = _setPrize.CalculateTotalWinningChance();
        tasks[3] = _setPrize.CreateLottoList();
        tasks[4] = _setPrize.SelectPrize();
        
        await Task.WhenAll(tasks);

        StartCoroutine(SetEarnedPrizeCard());
        
        SetPanel();
        }

        else if (currentSpin == maxSpinCount)
        {
            _cardCanvasManager.Quit();
        }
    }

    private IEnumerator SetEarnedPrizeCard()
    {
        yield return new WaitUntil(() => !_spinning.spinning);
        _cardCanvasManager.OpenCard();
        _cardCanvasManager.SetCard(_setPrize.earnedPrize);

        if (_setPrize.earnedPrize == _setPrize.death)
        {
            _cardCanvasManager.EndGame();
        }
    }
    
    
    private async Task SetSprites()
    {
        for (int i = 0; i < _setPrize.displayedList.Length; i++)
        {
            wheelIcons[i].SetSprite(_setPrize.displayedList[i].GetPrizeSprite());
        }
        await Task.Yield();
    }

    private void SetPanel()
    {
        cardPanel[0].SetCounter(currentSpin);
        cardPanel[1].SetCounter(currentSpin+1);
    }
    
    // This method is the part of the code that returns the correct list.
    // It's 1-10 bronze; 10-20 silver; 20-30 gold for now.
    public Prize[] ListRange()
    {
        if (currentSpin >= 0 && currentSpin < bronzeSwitchPoint)
        {
            _spinBaseRenderer.sprite = spinAtlas.GetSprite(spinBase[0]);
            _spinIndicatorRenderer.sprite = spinAtlas.GetSprite(spinIndicator[0]);
            return prizeLists[0].prizeList;
        }
        else if (currentSpin >= bronzeSwitchPoint && currentSpin < silverSwitchPoint)
        {
            _spinBaseRenderer.sprite = spinAtlas.GetSprite(spinBase[1]);
            _spinIndicatorRenderer.sprite = spinAtlas.GetSprite(spinIndicator[1]);
            return prizeLists[1].prizeList;
        }
        else if (currentSpin >= silverSwitchPoint && currentSpin < goldSwitchPoint)
        {
            _spinBaseRenderer.sprite = spinAtlas.GetSprite(spinBase[2]);
            _spinIndicatorRenderer.sprite = spinAtlas.GetSprite(spinIndicator[2]);
            return prizeLists[2].prizeList;
        }
        else return null;
    }
}