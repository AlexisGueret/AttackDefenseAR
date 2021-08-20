using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class RequestPermission : MonoBehaviour
{
    bool permissionAsked = false;
    // Start is called before the first frame update

    private void OnApplicationFocus(bool focus)
    {
        if (!permissionAsked || !focus)
            return;

        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            SceneManager.LoadScene("NormalGame");
        }
    }

    private void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            permissionAsked = true;
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
