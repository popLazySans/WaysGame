using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    public GameObject thisScene;
    public GameObject toScene;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void clickToAnotherScene()
    {
        thisScene.SetActive(false);
        toScene.SetActive(true);
    }
    public void exit()
    {
        Application.Quit();
    }
}
