//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using System;
//using DG.Tweening;

//public class UIScaleAnimation : MonoBehaviour
//{
//    public RectTransform panelRectTransform; // Reference to the RectTransform component of the UI panel
//    public Vector2 startScale = new Vector2(0.2f, 0.2f); // The starting scale for the animation
//    public Vector2 endScale = new Vector2(1f, 1f); // The target scale for the animation
//    public float animationDuration = 2f; // The duration of the animation in seconds

//    void Start()
//    {
//        panelRectTransform = 
//        // Set the initial scale of the panel
//        //panelRectTransform.localScale = startScale;

//        // Animate the scale of the panel from the start scale to the end scale over the specified duration
//        panelRectTransform.DOScale(endScale, animationDuration);
//    }
//}