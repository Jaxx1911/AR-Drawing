using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ImageController image;
    public ChangeStateButton lockPos, hideImage;
    public bool isLock, isHide;
    private RectTransform originalTranform;
    public Slider scale, blur;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isLock = lockPos.state;
        isHide = hideImage.state;
        scale.value = 0.5f;
        blur.value = 0.5f;
        originalTranform = image.rectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        var color = image.image.color;
        color.a = blur.value;
        image.image.color = color;

        ValueToScale();
    }

    void ValueToScale()
    {
        float percent = scale.value / 0.5f;
        Vector3 vector3 = image.rectTransform.localScale;
        vector3.x = percent;
        vector3.y = percent;

        image.rectTransform.localScale = vector3;
    }
    
    public void OnFlip()
    {
        Vector3 vector3 = image.rectTransform.localScale;
        vector3.x *= -1;
        image.rectTransform.localScale = vector3;
    }

    public void OnHide()
    {
        isHide = !isHide;
        image.gameObject.SetActive(!isHide);
    }

    public void OnLock()
    {
        isLock = !isLock;
    }
}
