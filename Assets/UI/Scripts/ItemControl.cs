using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemControl : MonoBehaviour {

	public GameObject item;
	private ItemLayout layout;
	private bool trigger;
	void Start(){
		layout = gameObject.GetComponent<ItemLayout> ();
	}
	void Update(){
		//itemPanel.GetComponentsInChildren<
		Button b = item.GetComponent<Button> ();
		InputControl ();
	}
	//Temporary item manipulation for users, feel free to use to play around
	void InputControl(){
		
		if (Input.GetKeyDown (KeyCode.A)) {
			Add (item);
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			Create (item);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			Hide (item);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			Remove (item);
		}
		if (trigger) {
			layout.formatItems ();
			trigger = false;
		}
	}
	//Unhides the item and adds it back into the pool
	public void Add(GameObject b){
		b.SetActive (true);
		trigger = true;
	}
	//Creates a new item and adds it to the item pool
	public void Create(GameObject b){
		//Created new Item
		Instantiate(b);
		trigger = true;
	}
	//Removes the item from the pool via equiping
	public void Hide(GameObject b){
		b.SetActive(false);
		trigger = true;
		//run the equiping function that equips the item
	}
	//Destroys the item via selling or consumption
	public void Remove(GameObject b){
		Destroy (b);
		trigger = true;
	}
}
