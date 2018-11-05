using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItemInfo : MonoBehaviour {

	public GameObject item;								//States the item that is being referenced
	private Image panel;								//This is the item panel that to display the item
	private Text[] displayText;							//This is the text that is displayed on screen
	public Image background;									//This is the text section that is filled with item descriptions
	// Use this for initialization	
	void Start () {
		setItem (item);
		displayItem ();
	}
	
	// Update is called once per frame
	void Update () {
		try{
			if(item.activeSelf){
			displayItem ();
			}else{
				displayNothing();
			}
		}
		catch(MissingReferenceException){
			displayNothing();
		}
	}

	//	Sets the item that is selected
	void setItem(GameObject selection){
		item = selection;
	}
	//	makes a copy of the formatted image and brings it over to the item panel to be displayed
	//	The text is also displayed
	//	TODO if the item is deselected, then clear the information in the panel

	void displayItem(){
		//Image Stuff
		panel = gameObject.GetComponent<Image>();
		Image[] temp = item.GetComponentsInChildren<Image>();
		panel.sprite = temp[1].sprite;
		//Title Stuff
		displayText = gameObject.GetComponentsInChildren<Text>();
		ItemInfo text = item.GetComponent<ItemInfo>();
		displayText[1].text = text.name;
		//Text Stuff
		displayText[0].text = text.description;
	}
	void displayNothing(){
		//Image Stuff
		panel = gameObject.GetComponent<Image>();
		panel.sprite = background.sprite;
		//Title Stuff
		displayText = gameObject.GetComponentsInChildren<Text>();
		displayText [1].text = "";
		//Text Stuff
		displayText[0].text = "";
	}
}
