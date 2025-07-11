using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWinorLoss : MonoBehaviour
{
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private Image banron;
    [SerializeField] private TextMeshProUGUI infoBanron;
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnHome;
    private void OnEnable()
    {
      

        ListenerManager.Instance.AddListener<bool>(EventName.GameOver, OnGameOver);

    }

    private void OnDisable()
    {
        if (ListenerManager.Instance != null)
        {
            ListenerManager.Instance.RemoveListener<bool>(EventName.GameOver, OnGameOver);
        }
    }
    void Start()
    {
        popUpPanel.SetActive(false);
        btnNext.onClick.AddListener(OnNextButtonClick);
        btnHome.onClick.AddListener(() => 
        {
            ListenerManager.Instance.TriggerEvent(EventName.WinGameToHome);
            popUpPanel.SetActive(false);
            GameManager.Instance.ResetLevel();
        });
    }

    void Update()
    {

    }
    private void OnNextButtonClick()
    {
        ListenerManager.Instance.TriggerEvent(EventName.StartNextLevel);
        popUpPanel.SetActive(false);
    }

    private void OnGameOver(bool isWin)
    {
        Debug.Log("Game Over: " + isWin);
        popUpPanel.SetActive(true); 
        popUpPanel.transform.transform.localScale = Vector3.zero;
        popUpPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        ShowWinorLoss(isWin);
    }


    public void ShowWinorLoss(bool isWin)
    {
        if(GameManager.Instance.currentLevelIndex == 10)
        {
            btnNext.gameObject.SetActive(false);
        }
        else
        {
            btnNext.gameObject.SetActive(true);
        }
        if (isWin)
        {
            banron.color = Color.red;
            infoBanron.text = "You Win!";
        }
        else
        {
            banron.color = new Color(0.5f, 0.0f, 0.5f); 
            infoBanron.text = "You Lose!";
            infoBanron.color = Color.white;
        }

    }
}
