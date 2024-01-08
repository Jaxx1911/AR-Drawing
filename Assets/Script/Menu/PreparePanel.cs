using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
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
        gameObject.transform.DOMove(new Vector3(1620, 960, 0), 0.5f);

        await Task.Delay(500);
        
        gameObject.SetActive(false);
    }
}
