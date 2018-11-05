
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class MapGenerator : MonoBehaviour {

    public static MapGenerator _instance;
    public GameObject TilePrefab;
    public GameObject ClutterPrefab;
    public Sprite[] clutters = new Sprite[20];
    public bool Generated = false;
    public float xSpawnMax=2.7f, xSpawnMin=-7f, ySpawnMax=2.2f, ySpawnMin=-3f;
    public int NumOfCoreNarration=15;
    public int MaxNarration=40;
    public float VerticleTilingOffset;
    public float HorizontleTilingOffset;
    public float HorizontalRowOffset;
    public int Rows;
    public int Cols;
    public float PruneChance;

    public float GlobalCombatChanceMod=1.0f;

    struct Edge
    {
        int from;
        int to;
        float dist;
    }
    struct IndexedDistance
    {
        int index;
        float distance;
    }

    struct DialogueChain
    {
        public int chainID;
        public List<int> members;
    }

    void Awake()
    {
        //if we don't have an [_instance] set yet
        if (!_instance)
            _instance = this;
        //otherwise, if we do, kill this thing
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(transform.gameObject);
        if (!Generated)
        {
            gameObject.GetComponent<MapProperties>().initList();
            GenerateTiles(Rows, Cols);
            gameObject.GetComponent<MapProperties>().Nodes[0].GetComponent<NodePartySelect>().SpawnParty();
            GenerateEvents();
            GenerateClutter();
            PopulateDialogue();
            Generated = true;
        }

    }

    private void PopulateDialogue()
    {
        //parse all dialogue  and create a list of dialogue chains(a dialogue chain is a group of related dialogue sets)
        List<DialogueChain> dSets = new List<DialogueChain>();
        string dialogue = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Dialogue.json"));
        DialogueSet[] allDialogue = JsonHelper.getJsonArray<DialogueSet>(dialogue);
        foreach (DialogueSet d in allDialogue)
        {
            bool found = false;
            foreach(DialogueChain dc in dSets)
            {
                if(dc.chainID == d.id / 10)
                {
                    dc.members.Add(d.id);
                    found = true;
                    break;
                }

            }
            if (!found)
            {
                DialogueChain nDChain = new DialogueChain();
                nDChain.chainID = d.id/ 10;
                nDChain.members = new List<int>();
                nDChain.members.Add(d.id);
                dSets.Add(nDChain);
            }
        }
        dSets.RemoveAt(0);//removes intro scene from list of dialogues


        //now we generate a list of narrativecore nodes
        List<GameObject> NCNodes = new List<GameObject>();
        foreach(GameObject Node in gameObject.GetComponent<MapProperties>().Nodes)
        {
            if (Node.GetComponent<NodeProperties>().NodeEvent == NodeProperties.EventType.NARRATIVECORE)
            {
                NCNodes.Add(Node);
            }
        }

        //with the list of narrative core nodes, assign each NCNode a dialogue chain until we run out of nodes or dialogue
        foreach(GameObject Node in NCNodes)
        {
            if(dSets.Count == 0)
            {
                return;//if we no longer have any unassigned dialogue left, we are done
            }
            Node.GetComponent<NodeProperties>().chainID = dSets[0].chainID;
            Node.GetComponent<NodeProperties>().dialogueSet.AddRange(dSets[0].members);
            Node.GetComponent<NodeProperties>().SetColor();
            dSets.RemoveAt(0);

        }

        //now we generate a list of side narrative nodes
        List<GameObject> SNNodes = new List<GameObject>();
        foreach (GameObject Node in gameObject.GetComponent<MapProperties>().Nodes)
        {
            if (Node.GetComponent<NodeProperties>().NodeEvent == NodeProperties.EventType.NARRATIVE)
            {
                SNNodes.Add(Node);
            }
        }

        //with the list of side narrative, assign each NCNode a dialogue chain until we run out of nodes or dialogue
        foreach (GameObject Node in SNNodes)
        {
            if (dSets.Count == 0)
            {
                return;//if we no longer have any unassigned dialogue left, we are done
            }
            Node.GetComponent<NodeProperties>().chainID = dSets[0].chainID;
            Node.GetComponent<NodeProperties>().dialogueSet.AddRange(dSets[0].members);
            Node.GetComponent<NodeProperties>().SetColor();
            dSets.RemoveAt(0);

        }



    }

    // Update is called once per frame
    void Update () {
		
	}
    void GenerateClutter()
    {
        foreach (GameObject Node in gameObject.GetComponent<MapProperties>().Nodes)
        {

            placeclutter(1.0f, 5.0f, clutters, 0, 4, Node,0.5f,0);
            placeclutter(1.0f, 15.0f, clutters, 5, 6, Node, 0.6f,1);
            placeclutter(1.0f, 15.0f, clutters, 7, 10, Node, 0.5f,4);
            placeclutter(0.5f, 15.0f, clutters, 11, 17, Node, 0.5f,7);

        }
    }
    void placeclutter(float inner, float outer, Sprite[] imgs,int begin,int end,GameObject Node,float chance,int clutteramt)
    {
        float roll = Random.value;
        if (roll <= chance)
        {
            GameObject newclutter = GameObject.Instantiate(ClutterPrefab);
            DontDestroyOnLoad(newclutter);
            Vector3 inipos = Node.GetComponent<Transform>().position;
            Vector3 offset = Random.insideUnitCircle * outer * 0.8f;
            offset += new Vector3(0, 0, -0.1f);
            newclutter.transform.position = inipos + offset;
            newclutter.GetComponent<SpriteRenderer>().sprite = imgs[Random.Range(begin, end + 1)];
            for (int i = 0; i < clutteramt; i++)
            {
                GameObject additionalclutter = GameObject.Instantiate(ClutterPrefab);
                DontDestroyOnLoad(additionalclutter);
                Vector3 initpos = Node.GetComponent<Transform>().position;
                Vector3 offsetpos = Random.insideUnitCircle * inner * 0.8f;
                offsetpos += new Vector3(0, 0, -0.1f);
                additionalclutter.transform.position = initpos + offsetpos;
                if (additionalclutter.transform.position.x >xSpawnMin && additionalclutter.transform.position.x <xSpawnMax && additionalclutter.transform.position.y <ySpawnMax && additionalclutter.transform.position.y > ySpawnMin)
                {
                    additionalclutter.GetComponent<SpriteRenderer>().sprite = imgs[Random.Range(begin, end + 1)];
                }
   
               

            }
        }
    }
    //assigns all nodes on the map to an event architype
    void GenerateEvents()
    {
        int count = gameObject.GetComponent<MapProperties>().Nodes.Count;
        int root = (int)Mathf.Sqrt((float)count);
        List<GameObject> templist = new List<GameObject>();
        //last node is combat node
        gameObject.GetComponent<MapProperties>().Nodes[count-1].GetComponent<NodeProperties>().NodeEvent = NodeProperties.EventType.COMBAT;
        gameObject.GetComponent<MapProperties>().Nodes[count - 1].GetComponent<NodeProperties>().SetColor();
        //populate temporary list for node event generation
        
        //0 not included due to it being the node the party starts on
        for (int i = 1; i < count-1; i++)
        {
            templist.Add(gameObject.GetComponent<MapProperties>().Nodes[i]);
        }

        //Placing narrative nodes (those with pure story)
        int ncount = 0;//number of narrative nodes placed
        while(ncount < NumOfCoreNarration)
        {
            int randindex = Random.Range(0, templist.Count);
            templist[randindex].GetComponent<NodeProperties>().NodeEvent = NodeProperties.EventType.NARRATIVECORE;
            ncount++;
            templist[randindex].GetComponent<NodeProperties>().SetColor();
            templist.RemoveAt(randindex);
        }
        //placing combat nodes on remaining node list
        for(int i = templist.Count-1;i >= 0; i--)
        {
            //multiplication table probability distribution
            //higher chance closer to bottom right hand corner
            float prob = (GlobalCombatChanceMod*(float)((((float)(i+1))%root)*(((float)(i+1))/root))) / ((float)count);
            //print(prob);
            if(Random.value < prob)
            {
                templist[i].GetComponent<NodeProperties>().NodeEvent = NodeProperties.EventType.COMBAT;
                templist[i].GetComponent<NodeProperties>().SetColor();
                templist.RemoveAt(i);
            }
        }
        //placing bonus side/resource narrative nodes
        for (int i = templist.Count - 1; i >= 0; i--)
        {
            //inverse multiplication table probability distribution
            //higher chance closer to upper left hand corner
            //float prob = ((float)count - (float)((i % root) * (i / root))) / ((float)count);
            //for first iteration, remaining nodes are all side/resource narrative nodes
            float prob = 1.1f;
            if (Random.value < prob)
            {
                templist[i].GetComponent<NodeProperties>().NodeEvent = NodeProperties.EventType.NARRATIVE;
                templist[i].GetComponent<NodeProperties>().SetColor();

                for (int j = 0; j < (int)PartyProperties.ResourceType.SIZE; j++)
                {
                    templist[i].GetComponent<NodeProperties>().ResourceMod[j] += (float)Random.Range(0,4);
                }
                

                templist.RemoveAt(i);
            }
        }

        
    }

    //generates the hexagon (invisible) tiles, and selectively prunes nodes
    void GenerateTiles(int numOfRows,int numOfCols)
    {
        //place all phase
        for(int i = 0; i < numOfRows; i++)
        {
            for(int j = 0;j < numOfCols; j++)
            {
                GameObject Tile = InitTile(j, i);
                Tile.GetComponent<TileProperties>().GenNode();
                gameObject.GetComponent<MapProperties>().Tiles[i, j] = Tile;
                //add nodes to master list
                gameObject.GetComponent<MapProperties>().Nodes.Add(Tile.GetComponent<TileProperties>().NodeChild);
            }
        }
        
    

    
     //Deletion method

        //randomly remove nodes from the list based on preset probability and prune out all the destroyed entries
        for (int i = gameObject.GetComponent<MapProperties>().Nodes.Count-1; i >= 0; i--)
        {
            float roll = Random.Range(0.0f, 1.0f);
            if (roll <= PruneChance)
            {

                Destroy(gameObject.GetComponent<MapProperties>().Nodes[i]);
                gameObject.GetComponent<MapProperties>().Nodes.RemoveAt(i);
            }
        }


        //calculate distance matrix
        int l = gameObject.GetComponent<MapProperties>().Nodes.Count;
        float[,] distance = new float[l, l];
        for(int a = 0; a < l; a++)
        {
            distance[a, a] = float.MaxValue;
            for (int b = a+1; b < l; b++)
            {
                distance[a, b] = Vector2.Distance(gameObject.GetComponent<MapProperties>().Nodes[a].GetComponent<Transform>().position, gameObject.GetComponent<MapProperties>().Nodes[b].GetComponent<Transform>().position);
                distance[b, a] = distance[a, b];
            }
        }
        //Prim's Algorithm
        
        int[] bestI = new int[l];
        int[] flags = new int[l];
        for (int i = 0; i < l; i++)
        {
            bestI[i] = -1;
            flags[i] = -1;
        }
        float[,] adjacencyMatrix = (float[,])distance.Clone();
        bool notDone = true;
        int pivot = 0;
        flags[0] = 1;
        while (notDone)
        {
            
            //delete row from choosen vortex
            for(int i = 0; i < l; i++)
            {
                adjacencyMatrix[pivot, i] = float.MaxValue;
            }

            //find lowest value in column
            float min = float.MaxValue;
            int newPivot = -1;
            for (int i = 0; i < l; i++)
            {
                if(flags[i] > 0)
                {
                    for (int j = 0; j < l; j++)
                    {
                       
                       if (adjacencyMatrix[j,i] < min)
                        {
                            //print(adjacencyMatrix[j, i]);
                            min = adjacencyMatrix[j, i];
                            newPivot = j;
                        } 
                    }
                }
            }
            if(newPivot != -1)
            {
                bestI[pivot] = newPivot;
                pivot = newPivot;
                flags[pivot] = 1;
            }
            else
            {
                print("Error with minimum spanning tree");
                return;
            }

            //check if all columns have been visited
            int counter = 0;
            for(int i = 0; i < l; i++)
            {
                if(bestI[i] >= 0)
                {
                    counter++;
                }
            }

            notDone = (counter == l-1);
            //print(bestI.ToString());
        }

        //connect the nodes
        int index = 0;
        while (bestI[index] != -1)
        {
            connectNode(gameObject.GetComponent<MapProperties>().Nodes[index], gameObject.GetComponent<MapProperties>().Nodes[bestI[index]]);
            index = bestI[index];
            //print(string.Format("connecting %i to %i", index, bestI[index]));
        }
        
        
        


        //calculate and connect 3 closest node for each node
        for (int i = 0; i < l; i++)
        {
            int[] minIndices = { 0, 1, 2 };
            float[] minDistances = { distance[i,0], distance[i,1], distance[i,2] };

            for (int j = 3;j < l; j++)
            {
                if( distance[i, j] <= 0)
                {
                    continue;
                }
                if(distance[i,j] < minDistances[0]  )
                {
                    minIndices[0] = j;
                    minDistances[0] = distance[i, j];
                }else if(distance[i, j] < minDistances[1] )
                {
                    minIndices[1] = j;
                    minDistances[1] = distance[i, j];
                }
                else if (distance[i, j] < minDistances[2])
                {
                    minIndices[2] = j;
                    minDistances[2] = distance[i, j];
                }
            }
            connectNode(gameObject.GetComponent<MapProperties>().Nodes[i], gameObject.GetComponent<MapProperties>().Nodes[minIndices[0]]);
            connectNode(gameObject.GetComponent<MapProperties>().Nodes[i], gameObject.GetComponent<MapProperties>().Nodes[minIndices[1]]);
            connectNode(gameObject.GetComponent<MapProperties>().Nodes[i], gameObject.GetComponent<MapProperties>().Nodes[minIndices[2]]);



        }
        //debug draw all paths
        /*
        foreach(GameObject Node in gameObject.GetComponent<MapProperties>().Nodes)
        {
            Node.GetComponent<NodeProperties>().drawPaths();
        }
        */


    }

    //"Connects" nodes by updating the associated linked lists
    void connectNode(GameObject nodeA,GameObject nodeB)
    {
        if (!nodeA.GetComponent<NodeProperties>().Neighbors.Contains(nodeB))
        {
            nodeA.GetComponent<NodeProperties>().Neighbors.Add(nodeB);
        }
        if (!nodeB.GetComponent<NodeProperties>().Neighbors.Contains(nodeA))
        {
            nodeB.GetComponent<NodeProperties>().Neighbors.Add(nodeA);
        }
    }



    /*

    
    void fillNeighbours(GameObject Tile,int i, int j)
    {
        for(int x = -1;x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if(!(x == 0 && y == 0)){
                    if (inRange(i+x, j+y))
                    {
                        Tile.GetComponent<TileProperties>().NodeChild.GetComponent<NodeProperties>().Neighbors.Add(gameObject.GetComponent<MapProperties>().Tiles[i + x, j + y].GetComponent<TileProperties>().NodeChild);
                    }
                }
            }
        }
    }
    */




    bool inRange(int x, int y)
    {
        if (0<=x && x < Rows && 0<= y && y< Cols)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 calcSpawnPositionFromIndex(int x, int y)
    {
        Vector3 initpos = gameObject.GetComponent<Transform>().position;
        Vector3 offset = new Vector3(x * HorizontleTilingOffset + (y % 2 * HorizontalRowOffset),-1*VerticleTilingOffset * y,10);//not sure why i need the 10 there
        return initpos + offset;
    }

   
    GameObject InitTile(int x,int y)
    {
        GameObject Tile = Instantiate(TilePrefab);
        Tile.transform.position = calcSpawnPositionFromIndex(x, y);
        return Tile;
    }

}
