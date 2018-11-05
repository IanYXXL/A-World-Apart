using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpenClose : MonoBehaviour {


	private GameObject menu;
	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("Inventory");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.M)){
			if (menu.activeSelf == true) {
				menu.SetActive (false);

			} else {
				menu.SetActive (true);

			}
		}
	}
}
