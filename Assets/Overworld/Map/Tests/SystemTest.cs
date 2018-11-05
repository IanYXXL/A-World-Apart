using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SystemTest : IPrebuildSetup
{

    public void Setup()
    {
        SceneManager.LoadScene("Map");
    }


//#################Tests Expected to PASS###############################
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator MainMenuStartGame() {
        SceneManager.LoadScene("menu");
        yield return new WaitForSeconds(2);
        GameObject startButton = GameObject.Find("Canvas");
        startButton.GetComponent<MainMenu>().StartGame();
        yield return new WaitForSeconds(2);
        Assert.IsTrue(SceneManager.GetActiveScene().name=="Map");
        yield return null;
    }



    [UnityTest]
    public IEnumerator NodeLoadsCombat()
    {
        
        SceneManager.LoadScene("Map");
        yield return new WaitForSeconds(2);
        GameObject MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        GameObject Party = GameObject.FindWithTag("Overworld Party");

        int[] visited = new int[nodes.Count];
        for (int j = 0; j < visited.Length; j++)
        {
            visited[j] = 0;
        }

        int[] prev = new int[nodes.Count];
        //setting prev values to -1 for safety
        for(int j = 0; j < prev.Length; j++)
        {
            prev[j] = -1;
        }
        List<int> queue = new List<int>();

        //searches for a path to the a combat node
        int i = 0;
        while(nodes[i].GetComponent<NodeProperties>().NodeEvent != NodeProperties.EventType.COMBAT)
        {
            visited[i] = 1;
            for(int j = 0; j < nodes[i].GetComponent<NodeProperties>().Neighbors.Count; j++)
            {
               //debug code
               /*
               for (int k = 0; k < visited.Length; k++)
                {
                    System.Console.Write(visited[k]);
                }
                System.Console.WriteLine();
                */


                int neighbourIndex = nodes.IndexOf(nodes[i].GetComponent<NodeProperties>().Neighbors[j]);
                if(visited[neighbourIndex] ==0 && !queue.Contains(neighbourIndex))
                {
                    queue.Add(neighbourIndex);
                    prev[neighbourIndex] = i;
                }
            }
            if(queue.Count < 1)
            {
                Assert.Fail("no valid path to combat node");
            }

            i = queue[0];
            queue.RemoveAt(0);

        }

        //builds path
        List<GameObject> path = new List<GameObject>();
        while(i != 0)
            {
                System.Console.WriteLine(i);
                path.Add(nodes[i]);
                i = prev[i];
            }
        path.Reverse();
        //skips initial dialogue
        GameObject Canvas = GameObject.FindWithTag("Overworld Canvas");
        Canvas.GetComponent<DialogueControl>().skipDialogue();
        //walk the path to see if we get to the combat node
        while (path.Count > 0)
        {

            yield return new WaitForSeconds(2);
            path[0].GetComponent<NodePartySelect>().DebugClick();  
            Canvas.GetComponent<DialogueControl>().skipDialogue();
            path.RemoveAt(0);
        }




        yield return new WaitForSeconds(5);
        Assert.IsTrue(SceneManager.GetActiveScene().name == "TestBattle");
        yield return null;
    }


    [UnityTest]
    public IEnumerator NodeRepeatDeath()
    {
        SceneManager.LoadScene("Map");
        yield return new WaitForSeconds(2);
        GameObject MCont = GameObject.Find("MapController");
        List<GameObject> nodes = MCont.GetComponent<MapProperties>().Nodes;
        GameObject Party = GameObject.FindWithTag("Overworld Party");
        Party.GetComponent<PartyProperties>().Resources[(int)PartyProperties.ResourceType.SUPPLY] = 6.0f;
        GameObject Canvas = GameObject.FindWithTag("Overworld Canvas");
       

        int bNode = nodes.IndexOf(nodes[0].GetComponent<NodeProperties>().Neighbors[0]);
        int current = 0;
        float supply = Party.GetComponent<PartyProperties>().Resources[(int)PartyProperties.ResourceType.SUPPLY];
        while (supply >= 0)
        {
            Canvas.GetComponent<DialogueControl>().skipDialogue();
            if (current == 0)
            {
                nodes[bNode].GetComponent<NodePartySelect>().DebugClick();
                yield return new WaitForSeconds(2);
                current = bNode;

            }
            else
            {
                nodes[0].GetComponent<NodePartySelect>().DebugClick();
                yield return new WaitForSeconds(2);
                current = 0;
            }
            supply--;
            
            
        }

       GameObject LooseScreen = GameObject.FindWithTag("Overworld Canvas").GetComponent<OverlayUIScripts>().LooseScreen;
        yield return new WaitForSeconds(2);
        Assert.IsTrue(LooseScreen.activeSelf);

    }
    //--------------Private methods


    //#################Tests Expected to FAIL###############################






}
