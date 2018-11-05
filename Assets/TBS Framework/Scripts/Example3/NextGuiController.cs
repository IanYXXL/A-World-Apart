using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NextGuiController : MonoBehaviour
{
    public CellGrid CellGrid;
    public GameObject UnitsParent;

    public Button NextTurnButton;

    //UI Panels
    public GameObject InfoPanel;
    public GameObject GameOverPanel;
    public Canvas Canvas;
    public GameObject ActionPanel;
    public GameObject HpBar;

    //instantiated instance of the UI Panels
    private GameObject _infoPanel;
    private GameObject _gameOverPanel;
    private GameObject _actionPanel;

    //current unit whos turn it is
    private GenericUnit currentUnit = new GenericUnit();

    //Turn order Display, it's a static 6 long array because that's how many portraits there are
    private GameObject[] turnOrderPortraits = new GameObject[6];

    //list of al hp bars
    GameObject[] _bar;

    private RectTransform canvasTransform;

    void Start()
    {
        //set the turn order portraits
        turnOrderPortraits[0] = Canvas.transform.Find("TurnOrder").Find("PortraitFrameMain").gameObject;
        turnOrderPortraits[1] = Canvas.transform.Find("TurnOrder").Find("Frame1").gameObject;
        turnOrderPortraits[2] = Canvas.transform.Find("TurnOrder").Find("Frame2").gameObject;
        turnOrderPortraits[3] = Canvas.transform.Find("TurnOrder").Find("Frame3").gameObject;
        turnOrderPortraits[4] = Canvas.transform.Find("TurnOrder").Find("Frame4").gameObject;
        turnOrderPortraits[5] = Canvas.transform.Find("TurnOrder").Find("Frame5").gameObject;
        GameOverPanel.SetActive(false);
        canvasTransform = Canvas.GetComponent<RectTransform>();
        CellGrid.GameStarted += OnGameStarted;
        CellGrid.TurnEnded += OnTurnEnded;
        CellGrid.GameEnded += OnGameEnded;


    }

    private void Update()
    {
        
        //This just makes sure all the HP bars are following the player, and to turn them off when the unit is no longer on screen
        for (int i = 0; i < CellGrid.Units.Count; i++)
        {
            Unit u = CellGrid.Units[i];
            if (!u.transform.GetComponentInChildren<SpriteRenderer>().isVisible)
            {
                _bar[i].SetActive(false);
            }
            else
            {
                _bar[i].SetActive(true);
            }
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(u.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((ViewportPosition.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f)),
            ((ViewportPosition.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.275f)));

            //set bars in canvas position
            _bar[i].GetComponent<RectTransform>().localPosition = WorldObject_ScreenPosition;
            RectTransform hpRect = _bar[i].transform.Find("HPBar").GetComponent<RectTransform>();
            RectTransform armorRect = _bar[i].transform.Find("ArmorBar").GetComponent<RectTransform>();

            hpRect.offsetMin = new Vector2(hpRect.offsetMin.x, 1.5f + 56.5f * (1 - (float)u.HitPoints / u.TotalHitPoints));
            armorRect.offsetMin = new Vector2(armorRect.offsetMin.x, 1.5f + 56.5f * (1 - (float)u.Armor / u.TotalArmor));
            _bar[i].transform.Find("HPText").GetComponent<Text>().text = u.HitPoints.ToString();
            _bar[i].transform.Find("ArmorText").GetComponent<Text>().text = u.Armor.ToString();

            if (NextTurnButton.interactable != ((CellGrid).CurrentPlayer is HumanPlayer))
            {
                NextTurnButton.interactable = ((CellGrid).CurrentPlayer is HumanPlayer);
            }
            

        }
        for (int i = CellGrid.Units.Count;  i < _bar.Length; i++)
        {
            _bar[i].SetActive(false);
        }
    }

    private void OnGameStarted(object sender, EventArgs e)
    {
        foreach (Transform unit in UnitsParent.transform)
        {
            unit.GetComponent<Unit>().UnitHighlighted += OnUnitHighlighted;
            unit.GetComponent<Unit>().UnitDehighlighted += OnUnitDehighlighted;
            unit.GetComponent<Unit>().UnitDestroyed += OnUnitDestroyed;
            unit.GetComponent<Unit>().UnitAttacked += OnUnitAttacked;
            unit.GetComponent<Unit>().UnitSelected += OnUnitSelected;
        }
        //at game start, instantiate the Hp bars
        print("Unit: " + CellGrid.Units.Count);
        _bar = new GameObject[CellGrid.Units.Count];
        for (int i = 0; i < CellGrid.Units.Count; i++)
        {
            _bar[i] = Instantiate(HpBar);
            _bar[i].transform.parent = Canvas.transform.Find("HPBarParent");
        }
        CellGrid.TurnEnded += OnNextTurn;

    }

    //what to do when end turn occurs
    private void OnTurnEnded(object sender, EventArgs e)
    {
        
        //NextTurnButton.interactable = ((sender as CellGrid).CurrentPlayer is HumanPlayer);
        
        UpdateTurnUI();
    }

    //When game ends, open the end screen
    private void OnGameEnded(object sender, EventArgs e)
    {
        GameOverPanel.SetActive(true);
        GameOverPanel.transform.Find("InfoText").GetComponent<Text>().text = "Player " + ((sender as CellGrid).CurrentPlayerNumber + 1) + "\nwins!";

        GameOverPanel.transform.Find("DismissButton").GetComponent<Button>().onClick.AddListener(DismissPanel);

        GameOverPanel.GetComponent<RectTransform>().SetParent(Canvas.GetComponent<RectTransform>(), false);

    }

    //highlight an attacked unit for feedback
    private void OnUnitAttacked(object sender, AttackEventArgs e)
    {
        if (!(CellGrid.CurrentPlayer is HumanPlayer)) return;

        OnUnitDehighlighted(sender, e);

        if ((sender as Unit).HitPoints <= 0) return;

        OnUnitHighlighted(sender, e);
    }

    //destroy the unit and associated infopanel
    private void OnUnitDestroyed(object sender, AttackEventArgs e)
    {
        Destroy(_infoPanel);
        print("unitdestroyed");
    }
    //we don't want infopanel showing up when the character isn't selected
    private void OnUnitDehighlighted(object sender, EventArgs e)
    {
        Destroy(_infoPanel);
        print("unitdehighlighted");

        LoadInfoPanel(currentUnit);
    }

    //We want to load a seperate infopanenl if the highlighted unit is different
    private void OnUnitHighlighted(object sender, EventArgs e)
    {
        print("unithighlighted");
        var unit = sender as GenericUnit;
        if (unit.UnitName == currentUnit.UnitName)
        {

        }
        else
        {
            Destroy(_infoPanel);

            LoadInfoPanel(unit);
        }
        if (unit.PlayerNumber != currentUnit.PlayerNumber && currentUnit.GetAvailableAttacks(CellGrid.Cells).Contains(unit.Cell))
        {
            unit.Cell.Mark(Cell.HighlightState.AttackSelected);
        }
        else if (unit.PlayerNumber != currentUnit.PlayerNumber)
        {
            unit.Cell.Mark(Cell.HighlightState.Attackable);
        }
        else
        {
            unit.Cell.Mark(Cell.HighlightState.Friendly);
        }
        

    }

    //destroy the info panel because its another character's turn
    private void OnNextTurn(object sender, EventArgs e)
    {
        Destroy(_infoPanel);


    }

    //keep the info panel up if a character is selected
    private void OnUnitSelected(object sender, EventArgs e)
    {
        var unit = sender as GenericUnit;
        if (unit.UnitName != currentUnit.UnitName)
        {
            currentUnit = unit;
            
            LoadInfoPanel(unit);
        }


    }

    //function for loading the stats of the current highlighted/selected unit on the infopanel
    //this is to make sure that unit information is up to date
    private void LoadInfoPanel(GenericUnit unit)
    {
        _infoPanel = Instantiate(InfoPanel);

        float hpScale = (float)((float)(unit).HitPoints / (float)(unit).TotalHitPoints);

        _infoPanel.transform.Find("Name").GetComponent<Text>().text = unit.UnitName;
        _infoPanel.transform.Find("Health").Find("Text").GetComponent<Text>().text = unit.HitPoints.ToString() + "/" + unit.TotalHitPoints.ToString();
        _infoPanel.transform.Find("Attack").Find("Text").GetComponent<Text>().text = unit.AttackFactor.ToString();
        _infoPanel.transform.Find("Armor").Find("Text").GetComponent<Text>().text = unit.Armor.ToString() + "/" + unit.TotalArmor;
        _infoPanel.transform.Find("Range").Find("Text").GetComponent<Text>().text = unit.AttackRange.ToString();
        _infoPanel.transform.Find("Speed").Find("Text").GetComponent<Text>().text = unit.Speed.ToString();

        _infoPanel.GetComponent<RectTransform>().SetParent(Canvas.GetComponent<RectTransform>(), false);
    }

    public void DismissPanel()
    {
        Destroy(_gameOverPanel);
    }
    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    //this function updates the order of the current character in play and which units are next
    //this allows the play to always know who's turn it is, and who will come next
    //adds strategy to the element
    private void UpdateTurnUI()
    {
        for (int i = 0; i<turnOrderPortraits.Length; i++)
        {
            turnOrderPortraits[i].transform.Find("Portrait").GetComponent<Image>().sprite = CellGrid.unitTurnOrder[((i+1)% CellGrid.unitTurnOrder.Count)].transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void DismissCombat()
    {
        if(UnitsParent.transform.GetChild(0).GetComponent<Unit>().PlayerNumber == 0)
        {
            //GameObject.Find("MapController").GetComponent<AudioSource>().mute = false;
            SceneManager.LoadScene("Map");
        }
        else
        {
            SceneManager.LoadScene("menu");
        }
        
    }

}

