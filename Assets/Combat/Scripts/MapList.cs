using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Maps : Assets
{
    //public string name;
    public int height;
    public int width;
    public string[] tiles;
    public int[] obstaclePossible;
    public int[] tileLayout;
}