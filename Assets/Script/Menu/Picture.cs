using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GplaySDK.Ads;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class Picture : MonoBehaviour
{
    public int order;

    public Image image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        AdsController.ShowInterstitial(() =>
        {
            DataController.CURRENT_PICTURE = order;
            MenuController.instance.OnGame();
        },StringConst.LocalKey.StringAds.PICK_PICTURE,false);
        
    }
}
