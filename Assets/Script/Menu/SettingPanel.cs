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
        gameObject.transform.DOLocalMove(new Vector3(1080, 0, 0), 0.2f);

        await Task.Delay(200);
        
        gameObject.SetActive(false);
    }
}
