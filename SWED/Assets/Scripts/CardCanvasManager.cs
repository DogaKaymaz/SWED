using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject cardCanvas;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    public bool isCardOpen;

    private void Start()
    {
        CloseCard();
        isCardOpen = false;
    }

    private void Update()
    {
        button.onClick.AddListener(CloseCard);
    }

    public void OpenCard()
    {
        cardCanvas.transform.localScale = new Vector3(1, 1, 1);
        isCardOpen = true;
    }

    public void CloseCard()
    {
        cardCanvas.transform.localScale = new Vector3(0, 0, 0);
        isCardOpen = false;
    }
    
    public void SetCard(Prize earnedPrize)
    {
        name.SetText(earnedPrize.displayName);
        image.sprite = earnedPrize.GetPrizeSprite();
        image.SetNativeSize();
    }
}
