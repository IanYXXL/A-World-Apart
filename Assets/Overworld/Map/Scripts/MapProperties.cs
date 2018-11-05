using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapProperties : MonoBehaviour {
    public GameObject[,] Tiles;
    public List<GameObject> Nodes;
    public GameObject PartyObject;
    bool nodeIDAssigned = false;
    int rows;
    int cols;
    [System.Serializable]
    public struct NodeStruct
    {
        public int ID;


    }
    // Use this for initialization
    void Start () {
        /*
        GameObject MController = GameObject.Find("MapController");
        MController.GetComponent<MapProperties>().loadmap();
        print("map loaded");
        */
        GameObject.Find("Canvas").GetComponent<DialogueControl>().startDialogue(11);
        DontDestroyOnLoad(transform.gameObject);
        
        Camera.main.transform.SetPositionAndRotation(new Vector3(-6, 2, Camera.main.transform.position.z), transform.rotation);



    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Map")
        {
            gameObject.GetComponent<AudioSource>().mute = false;
            Camera.main.GetComponent<CameraScroll>().initX = PartyObject.transform.position.x;
            Camera.main.GetComponent<CameraScroll>().initY = PartyObject.transform.position.y;
            print("Party Location: " + PartyObject.transform.position);
            
        }
    }

    public void defeat()
    {
        GameObject[] thingsToDestroy = GameObject.FindGameObjectsWithTag("DestroyOnDefeat");
        for(int i = 0; i < thingsToDestroy.Length; i++)
        {
            Destroy(thingsToDestroy[i]);
        }
        Destroy(GameObject.FindGameObjectWithTag("Overworld Party"));
        Destroy(transform.gameObject);
    }
    //initialize tile list
    public void initList()
    {
        rows = gameObject.GetComponent<MapGenerator>().Rows;
        cols = gameObject.GetComponent<MapGenerator>().Cols;
        Tiles = new GameObject[rows, cols];
    }
    //not fully implemented yet
    public void savemap()
    {
        if (!nodeIDAssigned)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].GetComponent<NodeProperties>().ID = i;
            }
        }


        

    }  
    public void loadmap()
    {

          
        

    }
    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Map");
        }
	}
}
