using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncButtonControl : MonoBehaviour
{
    public Button drawBtn, cameraBtn, flashBtn;
    [HideInInspector]
    public Image drawOff, cameraOff;
    [HideInInspector]
    public GameObject drawOn, cameraOn, flashOn, flashOff;

    private bool isFlash;

    public GameObject PictureFunc, CameraFunc;
    // Start is called before the first frame update
    void Start()
    {
        isFlash = false;
        OnDrawClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawClick()
    {
        CamOff();
        var color = new Color(255, 255, 255);
        drawOff.color = color;
        drawOn.SetActive(true);
        PictureFunc.SetActive(true);
    }

    public void OnCameraClick()
    {
        DrawOff();
        var color = new Color(255, 255, 255);
        cameraOff.color = color;
        cameraOn.SetActive(true);
        CameraFunc.SetActive(true);
    }

    private void OnFlashClick()
    {
        ChangeFlash();
    }
    
    private void DrawOff()
    {
        var color = new Color(0, 0, 0);
        drawOff.color = color;
        drawOn.SetActive(false);
        PictureFunc.SetActive(false);
    }

    private void CamOff()
    {
        var color = new Color(0, 0, 0);
        cameraOff.color = color;
        cameraOn.SetActive(false);
        CameraFunc.SetActive(false);
    }

    public  void ChangeFlash()
    {
        isFlash = !isFlash;
        flashOn.SetActive(isFlash);
    }
}
