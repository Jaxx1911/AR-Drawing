using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public async void BackMenu()
    {
        gameObject.transform.DOMove(new Vector3(1620, 960, 0), 0.5f);

        await Task.Delay(500);
        
        gameObject.SetActive(false);
    }
}
