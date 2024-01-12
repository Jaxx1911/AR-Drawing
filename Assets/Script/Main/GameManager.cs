    using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using GplaySDK.Ads;
using Script;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ImageController imageControl;
    
    private RectTransform originalTranform;
    public bool isHideLowerNavBar;
    public Slider blur;
    public GameObject UpBar, LowBar, MainImage;
    public RectTransform cameraPanel;
    
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
        Init();
        ImageDefaultSetting();
        StartWebCam();
    }

    // Update is called once per frame
    void Update()
    {
        var color = imageControl.image.color;
        color.a = blur.value;
        imageControl.image.color = color;
    }

    private void Init()
    {
        isHideLowerNavBar = true;
        //cameraPanel.sizeDelta = new Vector2(Display.main.systemHeight , Display.main.systemWidth);
        imageControl.image.sprite = MenuController.instance.pictureConfig.list[DataController.CURRENT_PICTURE];
        /*imageControl.transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f);
        UpBar.gameObject.transform.DOLocalMove(new Vector3(0, 870, 0), 0.2f);
        LowBar.gameObject.transform.DOLocalMove(new Vector3(0, -800, 0), 0.2f);*/
    }

    public void BackHome()
    {
        AdsController.ShowInterstitial(() =>
        {
            SceneManager.LoadSceneAsync("Menu");
        },StringConst.LocalKey.StringAds.BACK_HOME,false);
        
    }

    private void StartWebCam()
    { 
        WebCam.instance.StartCam();
    }

    private void ImageDefaultSetting()
    {
        Debug.LogError("???");
        blur.value = 1f;
        originalTranform = imageControl.rectTransform;
    }

    public void OnHideLowerNavBar()
    {
        isHideLowerNavBar = !isHideLowerNavBar;
        LowBar.SetActive(isHideLowerNavBar);
    }

    public void ShowTutorialBanner()
    {
        //show Banner
        AdsController.ShowInterstitial(() =>
        {
            
        },StringConst.LocalKey.StringAds.SHOW_TUTORIAL,false);
    }
}
