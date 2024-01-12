using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GplaySDK.Ads;
using Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public PictureConfig pictureConfig;
    public Button drawBtn, photoBtn, videoBtn, settingBtn;
    public GameObject mainMenu, settingsPanel, selectPicturePanel, preparePanel;

    public static MenuController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotoButtonClick()
    {
        
    }

    public void OnVideoButtonClick()
    {
        
    }

    public void OnSettingButtonClick()
    {
        settingsPanel.SetActive(true);
        settingsPanel.transform.DOLocalMove(new Vector3(0, 0, 10), 0.2f);
    }

    public void OnSelectPictureButtonClick()
    {
        selectPicturePanel.SetActive(true);
        selectPicturePanel.transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f);
    }

    public void OnGame()
    {
        SceneManager.LoadSceneAsync("Camera");
    }
    public void ShowTutorialBanner()
    {
        //show Banner
        AdsController.ShowInterstitial(() =>
        {
            
        },StringConst.LocalKey.StringAds.SHOW_TUTORIAL,false);
    }
}
