using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]  Image fillImage;
    private void Awake()
    {
        mainCamera = Camera.main;
        this.GetComponent<RectTransform>().pivot = new Vector2(0.5f, -12f);
    }
    public void SetColorBar(bool enemy)
    {
        if (enemy)
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = Color.green;
        }
    }
    public void SetupHealthBar(Transform target, float maxHealth, float currentHealth)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);
        this.transform.position = screenPos;
        float targetFill = Mathf.Clamp01(currentHealth / maxHealth);
        fillImage.DOKill();
        fillImage.DOFillAmount(targetFill, 0.3f).SetEase(Ease.OutQuad);
        if (Mathf.Clamp01(currentHealth / maxHealth) <= 0)
        {
            gameObject.SetActive(false);
            //ObjectPool.Instance.ReturnObject(CONST.POOL_HEALTHBAR,gameObject);
        }
    }
}
