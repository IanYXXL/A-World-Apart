using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipControl : MonoBehaviour {

	public GameObject selectedItem;
	public ItemControl displayItem;
	private Button b;
	private bool equipped;
	public Sprite defaultImage;
	// Use this for initialization
	void Start () {
		b = gameObject.GetComponent<Button> ();
		equipped = false;
	}
	
	// Update is called once per frame
	void Update () {
		selectedItem = GameObject.Find("ScrollMenu");
		displayItem =  selectedItem.GetComponent<ItemControl> ();
		selectedItem = displayItem.item;
		b.onClick.AddListener (delegate { 
			EquipItem();
		});
		//TODO have right click remove the item
		if (equipped) {
			b.onClick.AddListener (delegate { 
				DequipItem (selectedItem);
			});
		}
	}
	//This function equips the item graphically by sending it to the button icon
	//TODO have the equipped item reflect itself on the user's stats
	void EquipItem(){
		transform.GetChild(1).gameObject.SetActive(true);				//Set the level pic to active

		displayItem.Hide (selectedItem);
		Image[] panel = gameObject.GetComponentsInChildren<Image>();
		Image[] temp = selectedItem.GetComponentsInChildren<Image>();
		panel[1].sprite = temp[1].sprite;
		panel[2].sprite = temp[2].sprite;
		equipped = true;
	}
	//This function dequips the item graphically by sending it to the button icon
	//TODO have the dequipped item reflect itself on the user's stats
	void DequipItem(GameObject e){
		transform.GetChild(1).gameObject.SetActive(false);				//Set the level pic to active
		displayItem.Add (e);
		Image[] panel = gameObject.GetComponentsInChildren<Image>();
		panel [1].sprite = defaultImage;
		equipped = false;
		//
	}
}
