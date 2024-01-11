using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ImageController imageControl;
    public ChangeStateButton lockPos, hideImage;
    public bool isLock, isHide, isFlip;
    private RectTransform originalTranform;
    
    public Slider scale, blur;
    public GameObject UpBar, LowBar, MainImage;
    
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
        StartWebCam();
        ImageDefaultSetting();
    }

    // Update is called once per frame
    void Update()
    {
        var color = imageControl.image.color;
        color.a = blur.value;
        imageControl.image.color = color;

        ValueToScale();
    }

    void ValueToScale()
    {
        float percent = scale.value / 0.5f;
        Vector3 vector3 = imageControl.rectTransform.localScale;
        vector3.x = percent * (isFlip ? -1 : 1);
        vector3.y = percent;

        imageControl.rectTransform.localScale = vector3;
    }
    
    public void OnFlip()
    {
        isFlip = !isFlip;
        var vector3 = imageControl.rectTransform.localScale;
        vector3.x *= -1;
        imageControl.rectTransform.localScale = vector3;
    }

    public void OnHide()
    {
        isHide = !isHide;
        imageControl.gameObject.SetActive(!isHide);
    }

    public void OnLock()
    {
        isLock = !isLock;
    }

    private void Init()
    {
        imageControl.image.sprite = MenuController.instance.pictureConfig.list[DataController.CURRENT_PICTURE];
        imageControl.transform.DOLocalMove(new Vector3(0, 0, 0), 0.2f);
        UpBar.gameObject.transform.DOLocalMove(new Vector3(0, 870, 0), 0.2f);
        LowBar.gameObject.transform.DOLocalMove(new Vector3(0, -800, 0), 0.2f);
    }

    public async void BackHome()
    {
        imageControl.transform.DOLocalMove(new Vector3(1080, imageControl.transform.position.y, 0), 0.2f);
        UpBar.gameObject.transform.DOLocalMove(new Vector3(1080, 870, 0), 0.2f);
        LowBar.gameObject.transform.DOLocalMove(new Vector3(1080, -800, 0), 0.2f);
        await Task.Delay(100);
        SceneManager.LoadSceneAsync("Menu");
    }

    private void StartWebCam()
    {
        if(DataController.CAMERA_ON) WebCam.instance.StartCam();
    }

    private void ImageDefaultSetting()
    {
        Debug.LogError("???");
        isLock = lockPos.state;
        isHide = hideImage.state;
        scale.value = 0.5f;
        blur.value = 0.5f;
        originalTranform = imageControl.rectTransform;
    }
}
