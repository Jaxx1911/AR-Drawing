using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( WaitToLoadScene());
    }

    // Update is called once per frame

    IEnumerator WaitToLoadScene()
    {
        Debug.LogError("chuẩn bị load scene");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync("Menu");
    }
}
