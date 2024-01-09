using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStateButton : MonoBehaviour
{
    public Sprite stateOn, stateOff;
    public Image mainImage;
    public bool state = false;
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
        state = !state;
        mainImage.sprite = state ? stateOn : stateOff;
    }
}
