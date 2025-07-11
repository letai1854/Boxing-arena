using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [Header("Settings")]
    public float minSwipeDistance = 50f;



    private Vector2 startPosition;

  
    public static event Action OnSwipe;


    public void OnPointerDown(PointerEventData eventData)
    {

        startPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 swipeDelta = eventData.position - startPosition;
        if (Mathf.Abs(swipeDelta.x) < minSwipeDistance)
        {
            return;
        }
        Debug.Log("Swipe detected, distance: " );

       OnSwipe.Invoke();
    }
}
