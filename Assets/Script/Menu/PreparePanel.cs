using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreparePanel : MonoBehaviour
{
    public static PreparePanel instance;
    
    public Image image;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void BackToPicturePanel()
    {
        gameObject.transform.DOLocalMove(new Vector3(1080, 0, 0), 0.2f);

        await Task.Delay(200);
        
        gameObject.SetActive(false);
    }

     public async void OnClickCanvas()
     {
         gameObject.transform.DOLocalMove(new Vector3(-1080, 0, 0), 0.2f);
         await Task.Delay(200);
         //DataController.CAMERA_ON = false;
         SceneManager.LoadSceneAsync("Main");
    }

     public async void OnClickCamera()
     {
         gameObject.transform.DOLocalMove(new Vector3(-1080, 0, 0), 0.2f);
         await Task.Delay(200);
         //DataController.CAMERA_ON = true;
         SceneManager.LoadSceneAsync("Camera");
     }
}
