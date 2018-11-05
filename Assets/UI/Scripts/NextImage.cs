using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextImage : MonoBehaviour {

    public GameObject portrait;
    public GameObject battle;
    public GameObject tile;
    public GameObject hexegon;
    public GameObject icons;
    public GameObject sheets;

    SpriteRenderer p;
    SpriteRenderer b;
    SpriteRenderer t;
    SpriteRenderer h;
    SpriteRenderer i;
    SpriteRenderer s;

    Object[] _p;
    Object[] _b;
    Object[] _t;
    Object[] _h;
    Object[] _i;
    Object[] _s;

    int index = 0;

    // Use this for initialization
    void Start () {
        p = portrait.GetComponent<SpriteRenderer>();
        b = battle.GetComponent<SpriteRenderer>();
        t = tile.GetComponent<SpriteRenderer>();
        h = hexegon.GetComponent<SpriteRenderer>();
        i = icons.GetComponent<SpriteRenderer>();
        s = sheets.GetComponent<SpriteRenderer>();


        _p = Resources.LoadAll("Portraits", typeof(Sprite));
        _b = Resources.LoadAll("CombatSprites", typeof(Sprite));
        _t = Resources.LoadAll("MapLayovers", typeof(Sprite));
        _h = Resources.LoadAll("Map/Icons", typeof(Sprite));
        _i = Resources.LoadAll("Icons", typeof(Sprite));
        _s = Resources.LoadAll("BattleTiles", typeof(Sprite));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateImages()
    {
        index++;
        print(_p.Length);
        p.sprite = (Sprite)_p[index % _p.Length];
        b.sprite = (Sprite)_b[index % _b.Length];
        t.sprite = (Sprite)_t[index % _t.Length];
        h.sprite = (Sprite)_h[index % _h.Length];
        i.sprite = (Sprite)_i[index % _i.Length];
        s.sprite = (Sprite)_s[index % _s.Length];
    }
}
