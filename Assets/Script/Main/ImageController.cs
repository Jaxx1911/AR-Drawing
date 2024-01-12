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

    private Vector2 _temp;
    private void OnMouseDown()
    {
        Vector2 screenPoint = Input.mousePosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, mainCamera,
            out var localPosition);
        
        _temp =  (Vector2)rectTransform.position - localPosition;
    }

    private void OnMouseDrag()
    {
        Vector2 srceenPoint = Input.mousePosition;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, srceenPoint, mainCamera,
                out var localPosition);

        rectTransform.position = Vector3.Lerp(rectTransform.position, rectTransform.TransformPoint(localPosition + _temp),
                30 * Time.deltaTime);
    }

    private void Update()
    {
        
    }
}