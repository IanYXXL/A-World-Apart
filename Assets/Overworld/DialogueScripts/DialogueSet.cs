using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]

//Sets of Dialogue for each event
public class DialogueSet {
    public int id;
    public int[] requirement;
    public Dialogue[] Dialogue;
}