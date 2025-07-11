using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : Singleton<GameView>
{
    [Header("UI Elements")]
    [SerializeField] private Image hpImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI modeText;
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void UpdateGameInfo(float maxHp, float hp, string levelText, string modeText)
    {
        hpImage.fillAmount = Mathf.Clamp01(hp / maxHp);

        this.levelText.text = levelText;
        this.modeText.text = modeText;

        AnimateTextScale(this.levelText.rectTransform);
        AnimateTextScale(this.modeText.rectTransform);
    }

    private void AnimateTextScale(RectTransform rectTransform)
    {
        rectTransform.localScale = Vector3.zero; 
        rectTransform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack); 
    }
    public void UpdateHpPlayer(float maxHp, float hp)
    {
        float targetFill = Mathf.Clamp01(hp / maxHp);
        hpImage.DOKill();
        hpImage.DOFillAmount(targetFill, 0.3f).SetEase(Ease.OutQuad);
    }
}
