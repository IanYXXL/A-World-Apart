using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapTest : IPrebuildSetup
{

    GameObject MCont;
    public void Setup()
    {
        SceneManager.LoadScene("Map");
    }


//#################Tests Expected to PASS###############################
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator AtLestTenNodes() {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        Debug.Log("Testing 10 Nodes");
        Assert.IsTrue(MCont.GetComponent<MapProperties>().Nodes.Count >= 10);
        yield return null;
    }
    [UnityTest]
    public IEnumerator EachNodeThreeNeighbours()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        Debug.Log("Testing 3 Neighbours");
        for (int i = 0; i < nodes.Count; i++)
        {
            Assert.IsTrue(nodes[i].GetComponent<NodeProperties>().Neighbors.Count >= 3);
        }
        yield return null;
    }
    public IEnumerator EachNodeLessThanEightNeighbours()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        Debug.Log("Testing 3 Neighbours");
        for (int i = 0; i < nodes.Count; i++)
        {
            Assert.IsTrue(nodes[i].GetComponent<NodeProperties>().Neighbors.Count <=8);
        }
        yield return null;
    }
    [UnityTest]
    public IEnumerator EachNodeReachable()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        Debug.Log("Testing Reachability");
        List<GameObject> reachable = new List<GameObject>();
        RTest(reachable, nodes[1]);
        Debug.Log("RTest Done");
        for (int i = 0; i < nodes.Count; i++)
        {
   
            Assert.IsTrue(reachable.Contains(nodes[i]));

        }
        yield return null;
    }

    //--------------Private methods
    private void RTest(List<GameObject> visited, GameObject currentNode)
    {
        if (visited.Contains(currentNode))
        {
            return;
        }
        else
        {
            visited.Add(currentNode);
            List<GameObject> neigh = currentNode.GetComponent<NodeProperties>().Neighbors;
            for (int i = 0; i < neigh.Count; i++)
            {
                if (!visited.Contains(neigh[i]))
                {
                    RTest(visited, neigh[i]);
                }
            }
        }
    }

    
    [UnityTest]
    public IEnumerator EachNodeHasEvent()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        int noEvent = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].GetComponent<NodeProperties>().NodeEvent == NodeProperties.EventType.NONE)
            {
                noEvent++;
            }
            Assert.IsTrue(noEvent <=1);

        }

        yield return null;
    }

    //This is the only test that should fail in MapTests due to lack on region implementation
    [UnityTest]
    public IEnumerator EachNodeInARegion()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        for (int i = 0; i < nodes.Count; i++)
        {

            Assert.IsTrue(nodes[i].GetComponent<NodeProperties>().nodeRegion != NodeProperties.Region.NULL);

        }

        yield return null;
    }

    [UnityTest]
    public IEnumerator NodeTravelHasCost()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        //float maxCost = 0f;
        for (int i = 0; i < nodes.Count; i++)
        {
            float cost = nodes[i].GetComponent<NodeProperties>().costTo(nodes[0]);
            Assert.IsTrue(cost > 0f );
            //maxCost = Mathf.Max(maxCost, cost);

        }

        yield return null;
    }
    [UnityTest]
    public IEnumerator NodeNoOverLap()
    {
        Setup();
        yield return new WaitForSeconds(2);
        MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        //float maxCost = 0f;
        for (int i = 0; i < nodes.Count; i++)
        {
            for(int j = 0; j < nodes.Count; j++)
            {
               Assert.IsTrue( Vector3.Distance(nodes[i].GetComponent<Transform>().position, nodes[j].GetComponent<Transform>().position)>=0.1 || i==j);
            }

        }

        yield return null;
    }





}
