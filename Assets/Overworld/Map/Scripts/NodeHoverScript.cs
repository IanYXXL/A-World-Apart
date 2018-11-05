 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeHoverScript : MonoBehaviour {
    public Color TileColor;
    bool clickToggle = false;
    public bool Clickable = false;
	// Use this for initialization
	void Start () {
        TileColor = gameObject.GetComponent<SpriteRenderer>().material.color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //These functions updates the info panel with information regarding the node when the cursor hovers over it
    void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.blue ;
        gameObject.GetComponent<NodeProperties>().drawPaths();
        GameObject Canvas = GameObject.FindWithTag("Overworld Canvas");
        Canvas.GetComponent<OverlayUIScripts>().UpdateNodePopUp(gameObject.GetComponent<NodeProperties>());
    }
    void OnMouseExit()
    {
        gameObject.GetComponent<NodeProperties>().clearPaths();
        if (clickToggle)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = TileColor;
        }
        GameObject Canvas = GameObject.FindWithTag("Overworld Canvas");
        Canvas.GetComponent<OverlayUIScripts>().ClearNodePopUp();
    }

    void OnMouseUpAsButton()
    {
        //blank for now
       
    }
    //returns the node to its original color before it was highlighted
    public void ReturnNaturalColor()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = TileColor;
        clickToggle = false;
    }
    //not used for now, sets node color to green
    public void SetActiveColor()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;
        clickToggle = true;
    }

}
