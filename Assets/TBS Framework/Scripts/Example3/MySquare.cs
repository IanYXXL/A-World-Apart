using System;
using UnityEngine;

public class MySquare : Square
{
    public void Start()
    {
        transform.Find("Highlighter").GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    public override Vector3 GetCellDimensions()
    {
        var ret = GetComponent<SpriteRenderer>().bounds.size;
        return ret*0.98f;
    }

    /// <summary>
    /// Method for highlighting cells depending on the situation. Used to provide good user feedback
    /// </summary>
    public override void Mark(HighlightState s)
    {
        if (s == HighlightState.None)
        {
            SetColor(new Color(1, 1, 1, 0));
        }
        else if (s == HighlightState.Attackable)
        {
            SetColor(new Color(1f, 0.1f, 0.1f, 0.3f));
        }
        else if (s == HighlightState.AttackSelected)
        {
            SetColor(new Color(0.9f, 0.1f, 0.1f, 0.5f));
        }
        else if (s == HighlightState.Highlighted)
        {
            SetColor(new Color(0.8f, 0.8f, 0.8f, 0.5f));
        }
        else if (s == HighlightState.Path)
        {
            SetColor(new Color(0, 1, 0, 0.5f));
        }
        else if (s == HighlightState.Reachable)
        {
            SetColor(new Color(1, 0.92f, 0.16f, 0.5f));
        }
        else if (s == HighlightState.Friendly)
        {
            SetColor(new Color(0, .8f, 1f, 0.3f));
        }
        else if (s == HighlightState.FriendlySelected)
        {
            SetColor(new Color(0, .7f, 1f, 0.5f));
        }
    }

    public override void UnMark()
    {
        SetColor(new Color(1,1,1,0));
    }

    private void SetColor(Color color)
    {
        var highlighter = transform.Find("Highlighter");
        var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
}
