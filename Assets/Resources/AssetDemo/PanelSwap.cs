using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwap : MonoBehaviour {
    public GameObject mainPanel,amPanel,cmPanel,sfxPanel;
    GameObject currentPanel;


	// Use this for initialization
	void Start () {
        amPanel.SetActive(false);
        currentPanel = mainPanel;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void swapToAM()
    {
        currentPanel.SetActive(false);
        amPanel.SetActive(true);
        currentPanel = amPanel;
    }

    public void swapToMain()
    {
        currentPanel.SetActive(false);
        mainPanel.SetActive(true);
        currentPanel = mainPanel;
    }
    public void swapToCM()
    {
        currentPanel.SetActive(false);
        cmPanel.SetActive(true);
        currentPanel = cmPanel;
    }

    public void swapToSFX()
    {
        currentPanel.SetActive(false);
        sfxPanel.SetActive(true);
        currentPanel = sfxPanel;
    }
}
