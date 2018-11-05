using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueControl : MonoBehaviour {
    DialogueSet currentDialogue;
    public GameObject Party;

    int dialogueIndex = 11;
    int lineIndex = 0;
    int dialogueSpeed = 100;
    string currentText = "";

    bool doneScrolling = true;
    bool isSkipping = false;

    GameObject dialogueParent;

    //TextObject for the Dialogue
    Text dText;
    //TextObject for the name of the current speaker
    Text nameText;

    //the image on the left and right of the screen
    Image dialogueImageLeft;
    Image dialogueImageRight;

    bool isLastPortraitLeft = true;

    // Initialize Dialogue 
    void Start () {
        dialogueParent = transform.Find("DialogueUI").gameObject;
        dText = dialogueParent.transform.Find("DialogueBox").Find("dText").GetComponent<Text>();
        nameText = dialogueParent.transform.Find("DialogueBox").Find("Name").GetComponent<Text>();
        dialogueImageLeft = dialogueParent.transform.Find("LeftDialogue").GetComponent<Image>();
        dialogueImageRight = dialogueParent.transform.Find("RightDialogue").GetComponent<Image>();
        //startDialogue(11);
    }

    // Update is called once per frame
    /// <summary>
    /// In this method, we handle the cycling of dialogue. This allows the user to have control of the dialogue happening,
    /// going from one to the next, and the option to skip parts of the dialogue.
    /// </summary>
    void Update () {
        
        //move on to the next piece of dialogue on mouseclick and space, but only if the dialoge box is active
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode. Space) ) && dialogueParent.activeSelf && doneScrolling) 
        {
            //move through the dialogue of one character
            if (lineIndex < currentDialogue.Dialogue[dialogueIndex].Lines.Count - 1)
            { 
                lineIndex++;
                StartCoroutine("ScrollDialogue", currentDialogue.Dialogue[dialogueIndex].Lines[lineIndex]);
            }
            //move to the next person speaking
            else if (dialogueIndex < currentDialogue.Dialogue.Length - 1)
            {
                dialogueIndex++;
                lineIndex = 0;
                StartCoroutine("ScrollDialogue", currentDialogue.Dialogue[dialogueIndex].Lines[lineIndex]);
                setName();
                setPortrait();
            }
            //if there is no more dialogue, end the dialogue box
            else
            {
                lineIndex = 0;
                dialogueIndex = 0;

                dialogueParent.SetActive(false);
                Party.GetComponent<PartyProperties>().inDialogue = false;
            }
        }

        if ((Input.GetKeyDown(KeyCode.RightControl)|| Input.GetKeyDown(KeyCode.LeftControl)) && dialogueParent.activeSelf && !isSkipping)
        {
            //move through the dialogue of one character
            
            StartCoroutine("SkipDialogue");
            
        }
        if ((Input.GetKeyUp(KeyCode.RightControl) || Input.GetKeyUp(KeyCode.LeftControl)))
        {
            StopCoroutine("SkipDialogue");
            isSkipping = false;
        }
        dText.text = currentText;
    }

    /// <summary>
    /// Method for loading the dialogue json. This is so that the game will have the dialogues ready to cycle through.
    /// The json is used to dialogue can be stored and edited, and subsequently algorithmically assigned and cycled through
    /// without hardcoding
    /// </summary>
    void loadDialogue(int id)
    {
        string dialogue = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Dialogue.json"));
        DialogueSet[] allDialogue = JsonHelper.getJsonArray<DialogueSet>(dialogue);
        for(int i = 0; i < allDialogue.Length; i++)
        {
            if (allDialogue[i].id == id)
            {
                currentDialogue = allDialogue[i];
                break;
            }
        }
    }

    //// <summary>
    /// Method for opening a set of dialogue given an ID. Allows for any peice of dialogue to be loaded at any time.
    /// This allows for the dynamic allocation of dialogue peices
    /// </summary>
    public void startDialogue(int id)
    {
        dialogueParent.SetActive(true);
        loadDialogue(id);
        lineIndex = 0;
        dialogueIndex = 0;
        print("Starting");
        StartCoroutine("ScrollDialogue", currentDialogue.Dialogue[dialogueIndex].Lines[lineIndex]);
        setName();
        setPortrait();
    }

    /// <summary>
    /// Method for dialogue to slowly scroll, as if being said. Both allows for a more RPG feel and prevents
    /// users from accedentally skipping though dialogue because they pressed the space bar too often
    /// </summary>
    public IEnumerator ScrollDialogue(string t)
    {
        print("Starting Scroll");
        doneScrolling = false;
        currentText = "";
        for (int i = 0; i < t.Length; i++)
        {
            currentText += t[i];
            yield return new WaitForSeconds(dialogueSpeed * 0.0001f);
            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && dialogueParent.activeSelf)
            {
                currentText = t;
                doneScrolling = true;
                break;
            }
        }
        doneScrolling = true;

    }

    /// <summary>
    /// Method for skipping dialogue. Does not altogether skip all the dialogue, rather it scrolls through it really fast as
    /// long as a button is held. This allows dialogue to be quickly skipped without taking away the ability to only skip
    /// a part of the dialogue
    /// </summary>
    public IEnumerator SkipDialogue()
    {
        isSkipping = false;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (lineIndex < currentDialogue.Dialogue[dialogueIndex].Lines.Count - 1)
            {
                lineIndex++;
                currentText = currentDialogue.Dialogue[dialogueIndex].Lines[lineIndex];
                print(currentDialogue.Dialogue[dialogueIndex].Lines[lineIndex]);
            }
            //move to the next person speaking
            else if (dialogueIndex < currentDialogue.Dialogue.Length - 1)
            {
                dialogueIndex++;
                lineIndex = 0;
                currentText = currentDialogue.Dialogue[dialogueIndex].Lines[lineIndex];
                setName();
                setPortrait();
            }
            //if there is no more dialogue, end the dialogue box
            else
            {
                lineIndex = 0;
                dialogueIndex = 0;

                dialogueParent.SetActive(false);
                Party.GetComponent<PartyProperties>().inDialogue = false;
            }
        }
    }

    //set the name of the character speaking, so the player will know who is currently speaking
    void setName()
    {
        if (currentDialogue.Dialogue[dialogueIndex].name == "Blank")
        {
            nameText.text = "";
        }
        else
        {
            nameText.text = currentDialogue.Dialogue[dialogueIndex].name;
        }
    }

    //set the portrait of the character speaking, this is needed so that the player knows who's speaking
    void setPortrait()
    {
        if (false)
        {

        }
        else
        {
            //if no one is speaking, both portraits are false
            if (currentDialogue.Dialogue[dialogueIndex].name == "Blank")
            {
                dialogueImageRight.sprite = Resources.Load("Portraits/blank", typeof(Sprite)) as Sprite;
                dialogueImageLeft.sprite = Resources.Load("Portraits/blank", typeof(Sprite)) as Sprite;
            }

            else if (dialogueImageLeft.sprite.name != currentDialogue.Dialogue[dialogueIndex].name && dialogueImageRight.sprite.name != currentDialogue.Dialogue[dialogueIndex].name)
            {
                //if the name of the character matches a portrait
                if (Resources.Load("Portraits/" + currentDialogue.Dialogue[dialogueIndex].name, typeof(Sprite)) as Sprite != null)
                {
                    //set left
                    if (currentDialogue.Dialogue[dialogueIndex].side == "L")
                    {
                        dialogueImageLeft.sprite = Resources.Load("Portraits/" + currentDialogue.Dialogue[dialogueIndex].name, typeof(Sprite)) as Sprite;
                    }
                    //set right
                    else if (currentDialogue.Dialogue[dialogueIndex].side == "R")
                    {
                        dialogueImageRight.sprite = Resources.Load("Portraits/" + currentDialogue.Dialogue[dialogueIndex].name, typeof(Sprite)) as Sprite;
                    }
                }
                else
                {
                    //set the current image as blank
                    if (currentDialogue.Dialogue[dialogueIndex].side == "R")
                    {
                        dialogueImageRight.sprite = Resources.Load("Portraits/blank", typeof(Sprite)) as Sprite;
                    }
                    else if (currentDialogue.Dialogue[dialogueIndex].side == "L")
                    {
                        dialogueImageLeft.sprite = Resources.Load("Portraits/blank", typeof(Sprite)) as Sprite;
                    }

                }
                
            }          
        }
    }
    public void skipDialogue()
    {
        dialogueParent.SetActive(false);
    }
}
