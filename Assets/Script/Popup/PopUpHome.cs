using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpHome : MonoBehaviour
{
    [SerializeField] private GameObject popUpPanel;
    [SerializeField] private TextMeshProUGUI infoBanron;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btn25v25;
    private void OnEnable()
    {
        infoBanron.text = "Level "+ GameManager.Instance.GetCurrentLevelIndex().ToString();
        ListenerManager.Instance.AddListener(EventName.WinGameToHome, ShowHome);


    }

    private void OnDisable()
    {
        if (ListenerManager.Instance != null)
        {
            ListenerManager.Instance.RemoveListener(EventName.WinGameToHome, ShowHome);
        }

    }
    void Start()
    {
        btnPlay.onClick.AddListener(OnNextButtonClick);
        btn25v25.onClick.AddListener(On25v25ButtonClick);
    }

    void Update()
    {

    }
    private void OnNextButtonClick()
    {
        ListenerManager.Instance.TriggerEvent(EventName.HomeGame);
        popUpPanel.SetActive(false);
        
    }
    private void On25v25ButtonClick()
    {
        ListenerManager.Instance.TriggerEvent(EventName.GameChallenge);
        popUpPanel.SetActive(false);

    }
    private void ShowHome()
    {
        popUpPanel.SetActive(true);
        infoBanron.text = "Level " + GameManager.Instance.GetCurrentLevelIndex().ToString();
        popUpPanel.transform.localScale = Vector3.zero;
        popUpPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }





}
