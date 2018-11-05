using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
*Used Unity LERP Example from https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
* 
*/

public class PartyWalk : MonoBehaviour {
    public Transform startNode;
    public Transform endNode;
    public float speed = 0.00001F;
    public Vector3 playerSpriteOffset = new Vector3(0, 0.12f, -0.01f);
    private float startTime;
    private float journeyLength;
    bool walkflag = false;
    void Start()
    {

    }
    void Update()
    {
        if (walkflag)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startNode.position+ playerSpriteOffset, endNode.position+ playerSpriteOffset, fracJourney);
            if (fracJourney >= 1)
            {
                walkflag = false;
                startNode.GetComponent<NodeHoverScript>().ReturnNaturalColor();
                endNode.GetComponent<NodeHoverScript>().ReturnNaturalColor();
                NodePartySelect.walk = false;
                endNode.GetComponent<NodeProperties>().PopEvent();
                endNode.GetComponent<NodeProperties>().drawCurrentPaths();
                startNode.GetComponent<NodeProperties>().clearCurrentPaths();
                //SceneManager.LoadScene("TestBattle");
            }
        }
        
    }
    public void startWalk(Transform start,Transform end)
    {
        startNode = start;
        endNode = end;
        walkflag = true;
        startTime = Time.time;
        journeyLength = Vector3.Magnitude(end.position - start.position);
        GameObject.Find("SFX Source").GetComponent<SFXLoader>().LoadEngiRobotDeploySFX();
    }


}
