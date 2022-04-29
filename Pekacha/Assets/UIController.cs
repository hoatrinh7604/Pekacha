using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI endGameTitle;
    [SerializeField] GameObject gameOver;
    [SerializeField] TextMeshProUGUI shuffe_tx;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver(bool isWin)
    {
        gameOver.SetActive(true);

        endGameTitle.text = "Game over!!!";
        if (isWin) endGameTitle.text = "You win!!!";
    }

    public void UpdateRemainingShuffe(int value)
    {
        shuffe_tx.text = value.ToString();
    }

    public void SetSlider(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void UpdateSliderValue(float value)
    {
        slider.value = value;
    }
}
