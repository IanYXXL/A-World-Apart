using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    int resolution;
    bool fullWindowed;
    int sHeight, sWidth;

    // Use this for initialization
    void Start()
    {
        try
        {
            resolution = PlayerPrefs.GetInt("res"); //resolutionList = { "", "1600x900", "1366x1080",  "1280x1024", "1280x800", "800x600" };
            int fw = PlayerPrefs.GetInt("screen");
            if (fw== 1){ fullWindowed = true; }
            else{ fullWindowed = false; }
           
            switch (resolution)
            {
                case (1):
                    Screen.SetResolution(1600, 900, fullWindowed);
                    break;
                case (2):
                    Screen.SetResolution(1366, 1080, fullWindowed);
                    break;
                case (4):
                    Screen.SetResolution(1280, 800, fullWindowed);
                    break;
                case (3):
                    Screen.SetResolution(1280, 1024, fullWindowed);
                    break;
                case (5):
                    Screen.SetResolution(800, 600, fullWindowed);
                    break;
            }

        }
        catch { }

    }

    // Update is called once per frame
    void Update()
    {

    }
    

    public void StartGame()
    {
        GameObject.Find("SFX Source").GetComponent<SFXLoader>().LoadDoorSFX();
        PlayerPrefs.SetFloat("musicVolume", GameObject.Find("Music Source").GetComponent<MusicLoader>().music);
        PlayerPrefs.SetFloat("SFXVolume", GameObject.Find("SFX Source").GetComponent<SFXLoader>().SFX);
        SceneManager.LoadScene("Map");

    }

    public void ExitGame()
    {
        GameObject.Find("SFX Source").GetComponent<SFXLoader>().LoadDoorSFX();
        Application.Quit();
    }

    public void Options()
    {
        GameObject.Find("SFX Source").GetComponent<SFXLoader>().LoadDoorSFX();
        if (transform.Find("OptionsPanel").gameObject.activeSelf == true)
        {
            transform.Find("OptionsPanel").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("OptionsPanel").gameObject.SetActive(true);
        }
    }

    public void Instructions()
    {
        GameObject.Find("SFX Source").GetComponent<SFXLoader>().LoadDoorSFX();
        if (transform.Find("InstructionPanel").gameObject.activeSelf == true)
        {
            transform.Find("InstructionPanel").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("InstructionPanel").gameObject.SetActive(true);
        }
    }

}
