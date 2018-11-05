using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePartySelect : MonoBehaviour {
    public static GameObject Map;
    public static GameObject PartyIcon;
    public static GameObject SourceNode;
    public static GameObject LooseScreen;
    public static bool spawned = true;
    public static bool walk = false;
   
    // Use this for initialization
    void Start()
    {
        NodePartySelect.PartyIcon = GameObject.FindWithTag("Overworld Party");
    }

    //spawns the party on the 0 node and updates related stats
    public void SpawnParty()
    {
        NodePartySelect.PartyIcon = GameObject.FindWithTag("Overworld Party");
        NodePartySelect.Map = GameObject.FindWithTag("Overworld Map");
        PartyIcon.GetComponent<SpriteRenderer>().enabled = true;
        PartyIcon.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position + new Vector3(0, 0.12f, -0.01f);
        PartyIcon.GetComponent<PartyProperties>().OccupiedNode = gameObject;
        PartyIcon.GetComponent<PartyProperties>().OccupiedNode.GetComponent<NodeProperties>().NodeEvent = NodeProperties.EventType.NONE;
        SourceNode = PartyIcon.GetComponent<PartyProperties>().OccupiedNode;
        SourceNode.GetComponent<NodeProperties>().SetColor();
        SourceNode.GetComponent<NodeProperties>().drawCurrentPaths();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseUpAsButton()
    {
        //commented section only used during debugging
        /*
        if (!spawned)
        {
            spawned = true;
            PartyIcon.GetComponent<SpriteRenderer>().enabled = true;
            PartyIcon.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position + new Vector3(0, 0.12f, -0.01f);
            PartyIcon.GetComponent<PartyProperties>().OccupiedNode = gameObject;
            return;
        }
        */
        if (walk)
        {
            return;
        }
        if (SourceNode == null && gameObject == PartyIcon.GetComponent<PartyProperties>().OccupiedNode) 
        {
            SourceNode = gameObject;
            gameObject.GetComponent<NodeHoverScript>().SetActiveColor();
        }else if(SourceNode == gameObject)
        {
            //node deselection was swapped from "click to select" to "click to travel"
            /*
            SourceNode = null;
            gameObject.GetComponent<NodeHoverScript>().ReturnNaturalColor();
            */
        }
        else if (SourceNode != null && SourceNode.GetComponent<NodeProperties>().Neighbors.Contains(gameObject))
        {
            //print(PartyIcon.GetComponent<PartyProperties>().Resources[(int)PartyProperties.ResourceType.SUPPLY]);
            if(PartyIcon.GetComponent<PartyProperties>().Resources[(int)PartyProperties.ResourceType.SUPPLY] < 1)
            {
                print("no supply");
                LooseScreen = GameObject.FindWithTag("Overworld Canvas").GetComponent<OverlayUIScripts>().LooseScreen;

                LooseScreen.SetActive(true);
                Map.GetComponent<MapProperties>().defeat();
            }
            walk = true;
            PartyIcon.GetComponent<PartyWalk>().startWalk(SourceNode.GetComponent<Transform>(), gameObject.GetComponent<Transform>());
            PartyIcon.GetComponent<PartyProperties>().OccupiedNode = gameObject;
            SourceNode = gameObject;
            PartyIcon.GetComponent<PartyProperties>().ModResource(PartyProperties.ResourceType.SUPPLY,-1.0f);

        }
 

    }
    //debug function used for testing
    public void DebugClick()
    {
        OnMouseUpAsButton();
    }
}
