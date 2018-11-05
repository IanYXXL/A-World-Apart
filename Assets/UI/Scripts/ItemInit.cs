using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInit : MonoBehaviour {

	public GameObject selectedItem;
	// Use this for initialization
	void Start () {
		//sets the onclick to the activate displayItem
		//sets displayItem as the target in MouseHover
		GameObject target = GameObject.Find("Image");
		GameObject itemPool = GameObject.Find("ScrollMenu");
		MouseHover mh = GetComponent<MouseHover>();
		DisplayItemInfo displayItem = target.GetComponent<DisplayItemInfo> ();
		ItemControl ic = itemPool.GetComponent<ItemControl> ();
		Button b = gameObject.GetComponent<Button> ();
		mh.display = displayItem;
		//If on click occurs, then the mouse hover enables, the image displays in infoPanl and the
		//game object that is selected for the equipment is this one.
		b.onClick.AddListener(delegate { 
			mh.enabled = true;
			displayItem.enabled = true;
			ic.item = gameObject;
			selectedItem = gameObject;
		});
	}
}
