using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLayout : MonoBehaviour {


	public float baseX;
	public float baseY;
	public float spaceX;
	public float spaceY;
	// Use this for initialization
	void Start () {
		formatItems ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// Retrived the total number of items and then formats them within the centre panel
	//	TODO use relative values instead of hardcoded values, used for demo purposes
	public void formatItems(){
		RectTransform canvasRT = gameObject.GetComponent<RectTransform>();
		RectTransform[] rt = gameObject.GetComponentsInChildren<RectTransform>();

		float x = baseX;					//for scale -570
		float y = spaceY + baseY;				//for scale -1050, far right boundary is 1800
		int i = 0;					//item counter
		//print("The position of the canvas is " + canvasRT.offsetMin.x);
		foreach (RectTransform t in rt){
			if(t.gameObject.GetInstanceID() != gameObject.GetInstanceID()){
				
				if (x >= 450 ){//canvasRT.offsetMin.x*2) {			// Times two because of width
						//move to next row;
					x = baseX;
					y -= spaceY;
				}
				t.position = new Vector2 (x, y);
				i++;
				if (i == 3) {								// three because of the parent, base picture and level
					x += spaceX;
					i = 0;
				}
			}
		}
	}
}
