using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        Debug.LogError("wtfff");
        MenuController.instance.OnPreGame();
        PictureManager.instance.chosenPictureOrder = order;
        PreparePanel.instance.image.sprite =
            MenuController.instance.pictureConfig.listFood[PictureManager.instance.chosenPictureOrder];
    }
}
