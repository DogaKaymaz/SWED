using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject cardCanvas;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private TextMeshProUGUI buttonText;
    private bool isButtonContinue;
    public bool isCardOpen;

    private void Start()
    {
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.SetText("CONTINUE");
        isButtonContinue = true;
        CloseCard();
        isCardOpen = false;
    }

    private void Update()
    {
        if(isButtonContinue) button.onClick.AddListener(CloseCard);
        else if (!isButtonContinue) button.onClick.AddListener(Quit);
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

    public void EndGame()
    {
        buttonText.SetText("QUIT");
        isButtonContinue = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    
    public void SetCard(Prize earnedPrize)
    {
        name.SetText(earnedPrize.displayName);
        image.sprite = earnedPrize.GetPrizeSprite();
        image.SetNativeSize();
    }
}
