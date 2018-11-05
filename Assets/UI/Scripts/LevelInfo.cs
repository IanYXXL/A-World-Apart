using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour {
			



	private Sprite level;				//	The sprite that is used for the level type
	private Image levelRenderer;		//	The Image that is layered ontop of the image
	public int levelnum;				//	The value of the level, being 1, 2, 3 or 4
	// Use this for initialization
	void Awake () {

		Sprite[] levels = Resources.LoadAll <Sprite>("Icons/levels");			//Loads all levels, randomly assigns a level to the game object
		levelnum = Random.Range (0, 4);	
		level = levels[levelnum];
		levelRenderer = gameObject.GetComponent <Image>();
		levelRenderer.sprite = level;
	}
	
	// Update is called once per frame
	void Update () {
		//levelRenderer.sprite = levels[level];
	}
	// Returns the value of the level, used for item calculation
	public int getlevel(){
		return levelnum;
	}

}
