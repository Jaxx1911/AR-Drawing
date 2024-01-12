using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    public static WebCam instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private int currentCamIndex = 0;

    private WebCamTexture tex;

    public RawImage camDisplay;
    // Start is called before the first frame update
    public void SwapCam()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;
        }
    }

    public void StartCam()
    {
        var color = new Color(255, 255, 255);
        camDisplay.color = color;
        if (tex != null)
        {
            camDisplay.texture = null;
            tex.Stop();
            tex = null;
        }
        else
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            camDisplay.texture = tex;
            
            tex.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
