using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class NodeProperties : MonoBehaviour {
    public GameObject ParentTile;
    public GameObject PathPrefab;
    public GameObject Party;
    public int ID; //node id used purely for saving/loading
    public Color ResourceEvent, DialogueEvent, CombatEvent, NoEvent;
    public List<GameObject> Neighbors;
    public bool visited = false;
    public List<GameObject> Pathing,OccupiedPathing;
    public EventType NodeEvent =EventType.NONE ;
    public int dialogueID = 0, chainID = 0;
    public enum Region{A,B,C,D,E,NULL};
    public float[] ResourceMod = new float[(int)PartyProperties.ResourceType.SIZE];
    public enum EventType { COMBAT,NARRATIVECORE,NARRATIVE,NONE};
    public Region nodeRegion = Region.NULL;
    public List<int> dialogueSet = new List<int>();
    public List<DiaReq> dialogueReqs= new List<DiaReq>();

    public struct DiaReq
    {
        public int diaID;
        public int[] Req;
    }


    // Use this for initialization
    void Start () {
        Party = GameObject.Find("PartyPlaceholder");
        DontDestroyOnLoad(transform.gameObject);
        DontDestroyOnLoad(ParentTile);
        dialogueSet.Add(0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //pathing functions
    public void drawCurrentPaths()//draws paths available on occupied node
    {
        foreach (GameObject Node in Neighbors)
        {
            Connect(Node, OccupiedPathing);
        }
    }
    public void clearCurrentPaths()//clears pathing when party moves
    {
        for (int i = OccupiedPathing.Count - 1; i >= 0; i--)
        {
            Destroy(OccupiedPathing[i]);
            OccupiedPathing.RemoveAt(i);
        }
    }
    public void drawPaths()//draws paths of highlighted node
    {
        foreach(GameObject Node in Neighbors)
        {
            Connect(Node);
        }
    }
    public void clearPaths()//clears pathing when party moves
    {
        for(int i = Pathing.Count-1; i >=0 ; i--)
        {
            Destroy(Pathing[i]);
            Pathing.RemoveAt(i);
        }
    }

    //Connects this node to another node by updating both node's neighbour lists

    void Connect(GameObject NodeB, List<GameObject> walkpath = null)
    {
        Vector3 unitVec = NodeB.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
        float length = unitVec.magnitude;
        unitVec = unitVec.normalized;
        float angle = Vector3.SignedAngle(new Vector3(1, 0,0), unitVec, new Vector3(1, 0, 0));
        if (unitVec[1] < 0)
        {
            angle *= -1;
        }
        //print(angle);
        for (float f = 0.03f;f < length - 0.03f; f+= 0.06f)
        {
            GameObject path = Instantiate(PathPrefab);
            Vector3 offset = f * unitVec;
            path.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position + offset;
            path.GetComponent<Transform>().rotation = Quaternion.Euler(0,0, angle);
            if (walkpath==null)
            {
                Pathing.Add(path);
            }
            else
            {
                walkpath.Add(path);
            }

        }

    }
    //cost function
    public float costTo(GameObject NodeB)
    {
        return 1.0f;
    }

    //processes the event associated with the node
    public void PopEvent()
    {
        if (NodeEvent == EventType.NONE)
        {
            return;
        } else if (NodeEvent == EventType.NARRATIVECORE)
        {
          
            startDialogue();
            if (dialogueSet.Count == 1 && dialogueSet[0] == 0)
            {
                NodeEvent = EventType.NONE;
            }
            

        } else if (NodeEvent == EventType.COMBAT)
        {
            NodeEvent = EventType.NONE;
            GameObject MController = GameObject.Find("MapController");
            Party.GetComponent<PartyProperties>().battleState.enemyID = Random.Range(1,6);
            if (gameObject == MController.GetComponent<MapProperties>().Nodes[MController.GetComponent<MapProperties>().Nodes.Count - 1])
            {
                Party.GetComponent<PartyProperties>().battleState.finalBattle = true;
                Party.GetComponent<PartyProperties>().battleState.enemyID = 6;
                Party.GetComponent<PartyProperties>().battleState.mapName = "Underground";
                GameObject WinScreen = GameObject.FindWithTag("Overworld Canvas").GetComponent<OverlayUIScripts>().WinScreen;

                WinScreen.SetActive(true);
                //GameObject.FindWithTag("Overworld Canvas").GetComponent<MapProperties>().defeat();
            }
            
            SavedLoad.savedHeroStats = Party.GetComponent<PartyProperties>().battleState;
            SavedLoad.Write();
            GameObject.Find("MapController").GetComponent<AudioSource>().mute = true;
            SceneManager.LoadScene("TestBattle");

        }
        else if (NodeEvent == EventType.NARRATIVE)
        {
            NodeEvent = EventType.NONE;
            GameObject Party = GameObject.FindWithTag("Overworld Party");
            Party.GetComponent<PartyProperties>().ProccessResourceEvent(gameObject.GetComponent<NodeProperties>().ResourceMod);
            startDialogue();
            if (dialogueSet.Count == 1 && dialogueSet[0] == 0)
            {
                NodeEvent = EventType.NONE;
            }


        }
        SetColor();
    }

    //sets the node color to the one appropriate for the event it contains
    public void SetColor()
    {

        switch (NodeEvent)
        {
            case (EventType.NONE):
                gameObject.GetComponent<SpriteRenderer>().color = NoEvent;
                break;
            case (EventType.COMBAT):
                gameObject.GetComponent<SpriteRenderer>().color = CombatEvent;
                break;
            case (EventType.NARRATIVECORE):
                if (dialogueSet.Count > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = DialogueEvent;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().color = NoEvent;
                }

                break;
            case (EventType.NARRATIVE):
                if (dialogueSet.Count > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = ResourceEvent;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().color = ResourceEvent;
                }
                break;
        }
    }

 

    public List<int> fetchRequirements(int dialogueID)
    {

        if(dialogueReqs.Count != dialogueSet.Count)
        {
            string dialogue = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Dialogue.json"));
            List<DialogueSet> allDialogue = new List<DialogueSet>(JsonHelper.getJsonArray<DialogueSet>(dialogue));
            foreach(int d in dialogueSet)
            {
                for (int i = allDialogue.Count - 1;i >= 0; i --)
                {
                    DialogueSet ds = allDialogue[i];
                    if(ds.id == d)
                    {
                        DiaReq newDR = new DiaReq();
                        newDR.diaID = ds.id;
                        newDR.Req = ds.requirement;
                        allDialogue.Remove(ds);
                        dialogueReqs.Add(newDR);
                    }
                }
            }
        }

        foreach(DiaReq dr in dialogueReqs)
        {
            if(dr.diaID == dialogueID)
            {
                return new List<int>(dr.Req);
            }
        }


        return new List<int>(new int[]{-1});
    }

    //test if all requirements passed into the function have been fulfilled
    public bool testDialogueReq(List<int> requirements)
    {
        if(requirements[0] == 0)
        {
            return true;
        }

        foreach(int dialogue in Party.GetComponent<PartyProperties>().CompletedDialogue)
        {
            bool cond1 = requirements.Contains(dialogue / 10 * 10);//requirements ending in 0 are fulfilled if any dialogue within the set has been completed
            bool cond2 = requirements.Contains(dialogue);//specific requirement for a particular dialogue set is complete
            if(cond1 || cond2)
            {
                requirements.Remove(dialogue);
            }
        }



        if(requirements.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //finds the correct dialogue and initiates it
    public void startDialogue()
    {
        int dialogueID = -1;
        foreach (int dialogue in dialogueSet)
        {
            if (testDialogueReq(fetchRequirements(dialogue)))
            {
                dialogueID = dialogue;

            }
        }
        if (dialogueID == -1)
        {
            return;
        }
        else
        {
            GameObject Canvas = GameObject.FindWithTag("Overworld Canvas");
            Canvas.GetComponent<DialogueControl>().startDialogue(dialogueID);
            if (!Party.GetComponent<PartyProperties>().CompletedDialogue.Contains(dialogueID))
            {
                Party.GetComponent<PartyProperties>().CompletedDialogue.Add(dialogueID);
                dialogueSet.Remove(dialogueID);
            }
            Party.GetComponent<PartyProperties>().inDialogue = true;
        }


    }

}
