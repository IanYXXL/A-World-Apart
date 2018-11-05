using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resolution : MonoBehaviour {

    public Dropdown dropdown;
    public Text resolutionWarning;
    public Toggle toggle;
    public Slider sfx;
    public Slider music;

    List<string> resolutionList = new List<string>() { "", "1600x900", "1366x1080",  "1280x1024", "1280x800", "800x600" };

    public void Dropdown_IndexChanged(int index)
    {
        if (index > 0)
        {
            PlayerPrefs.SetInt("res", index);
            resolutionWarning.gameObject.SetActive(true);
        }
    }

    public void Screen_ToggleChanged(bool b)
    {
        if (b) { PlayerPrefs.SetInt("screen", 1); }
        else { PlayerPrefs.SetInt("screen", 0); }
        print(PlayerPrefs.GetInt("screen"));
    }

    // Use this for initialization
    void Start() {
        PopulateList();

        if (PlayerPrefs.HasKey("screen"))
        {
            if (PlayerPrefs.GetInt("screen") == 1) { toggle.isOn = true; } else { toggle.isOn = false; }
            if (PlayerPrefs.HasKey("musicVolume")) { sfx.value = PlayerPrefs.GetFloat("musicVolume"); }
            if (PlayerPrefs.HasKey("SFXVolume")) { music.value = PlayerPrefs.GetFloat("SFXVolume"); }
            if (PlayerPrefs.HasKey("res")) { dropdown.value = PlayerPrefs.GetInt("res"); } else { dropdown.value = 0; }

        }

    }


    void PopulateList()
    {

        dropdown.AddOptions(resolutionList);

    }
}
