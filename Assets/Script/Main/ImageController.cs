using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public bool drag;

    public RectTransform rectTransform;

    public Camera mainCamera;

    public Image image;
    
    private void OnMouseDrag()
    {
        if (!GameManager.instance.isLock)
        {
            Vector2 srceenPoint = Input.mousePosition;


            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, srceenPoint, mainCamera,
                out var localPosition);

            rectTransform.position = Vector3.Lerp(rectTransform.position, rectTransform.TransformPoint(localPosition),
                30 * Time.deltaTime);
        }
    }

    private void Update()
    {
        
    }
}