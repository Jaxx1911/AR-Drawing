using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureManager : MonoBehaviour
{
    public static PictureManager instance;

    public int chosenPictureOrder;
    
    public Picture picture;

    public GameObject parent;

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
        for (int i = 0; i < MenuController.instance.pictureConfig.listFood.Count; i++)
        {
            picture.order = i;
            picture.image.sprite = MenuController.instance.pictureConfig.listFood[i];
            Instantiate(picture, parent.transform);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
