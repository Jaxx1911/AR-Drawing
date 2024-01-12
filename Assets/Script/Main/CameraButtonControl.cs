using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraButtonControl : MonoBehaviour
{
    public GameObject markPhoto, markVideo;

    public GameObject RecordBtn, CapBtn;
    // Start is called before the first frame update
    void Start()
    {
        PhotoOff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickVideo()
    {
        PhotoOff();
        RecordBtn.SetActive(true);
        markVideo.SetActive(true);
    }

    public void OnClickPhoto()
    {
        VideoOff();
        CapBtn.SetActive(true);
        markPhoto.SetActive(true);
    }

    private void VideoOff()
    {
        RecordBtn.SetActive(false);
        markVideo.SetActive(false);
    }

    private void PhotoOff()
    {
        CapBtn.SetActive(false);
        markPhoto.SetActive(false);
    }
}
