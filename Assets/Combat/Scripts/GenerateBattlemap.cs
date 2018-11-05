using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GenerateBattlemap : MonoBehaviour {

    private RectangularSquareGridGenerator gridGen;
    private Transform cellsParent;
    public Transform ObstaclesParent;
    public Transform UnitParent;

    //templates for Tiles, Obstacles, and enemy units, respectively
    public GameObject squarePrefab;
    public GameObject obstaclePrefab;
    public GameObject enemyPrefab;

    //Dimensions of the map to be generated, this will be taken from a json list
    public int height = 1;
    public int width = 1;
    private string mapName = "default";
    private int enemyLayoutID = 0;

    //holds the layout of the map, obstacles, and enemies for use when intializing the map
    private Maps currentMap;
    private ObstacleLayout obstacleLayout;
    private EnemyLayout enemyLayout;

    //holds the individual Obstacles, Tiles, and Enemies to be added to the map
    private List<Obstacles> obs = new List<Obstacles>();
    private List<Tiles> tiles = new List<Tiles>();
    private List<Enemy> enemies = new List<Enemy>();

    //lists of map objects
    private Tiles[] tileList;
    private Maps[] mapList;
    private Obstacles[] obsList;
    private ObstacleLayout[] obsLayoutList;
    private Enemy[] enemyList;
    private EnemyLayout[] enemyLayoutList;

    //names of the Tiles, Obstacles, and Enemy infoblocks used for the map
    private List<string> usedEnemies;
    private List<string> usedObstacles;
    private List<string> usedTiles;

    BattleState battleState;

    //initialize map
    private void Awake()
    {
        gridGen = new RectangularSquareGridGenerator();
        battleState = SavedLoad.Read();
        cellsParent = transform;
        squarePrefab = Resources.Load("Prefabs/SquareTile", typeof(GameObject)) as GameObject;

        //Load the map by name
        LoadMap();
        //create the basic dimensions of the map with the tile template
        CreateGrid();

        //populate the map
        PopulateTiles();
        PopulateObstacles();
        PopulateEnemies();
        SetHeroStats();
    }

    //set grid height, width, and create the grid
    public void CreateGrid()
    {
        gridGen.Height = height;
        gridGen.Width = width;
        gridGen.SquarePrefab = squarePrefab;
        gridGen.CellsParent = cellsParent;
        gridGen.GenerateGrid();
    }

    /// <summary>
    /// Load map onto the existing grid
    ///  This function loads all the map-related json into arrays, so that they can be used to generate the map later
    ///  It also loads the layouts in the json into their respective layout variables, and in general sets all variables
    ///  into what they need to be in order for the map generation to commence
    /// </summary>
    void LoadMap()
    {
        mapList = LoadMaps();
        tileList = LoadTiles();
        obsList = LoadObstacles();
        obsLayoutList = LoadObstacleLayout();
        enemyList = LoadEnemies();
        enemyLayoutList = LoadEnemyLayout();
        int mapIndex = findElementOfName<Maps>(battleState.mapName, mapList);

        print("Map Index Found!");

        currentMap = mapList[mapIndex];
        int obstacleID = currentMap.obstaclePossible[Random.Range((int)0, currentMap.obstaclePossible.Length)];
        obstacleLayout = obsLayoutList[obstacleID];
        enemyLayout = enemyLayoutList[battleState.enemyID-1];
        height = currentMap.height;
        width = currentMap.width;

        loadList<Tiles>(currentMap.tiles, tiles, tileList);
        loadList<Obstacles>(obsLayoutList[obstacleID].types, obs, obsList);
        loadList<Enemy>(enemyLayoutList[enemyLayoutID].types, enemies, enemyList);
    }

    //load obstacle json
    Obstacles[] LoadObstacles()
    {
        string obs = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Obstacles.json"));
        Obstacles[] obsList = JsonHelper.getJsonArray<Obstacles>(obs);
        return obsList;
    }

    //load tiles json
    Tiles[] LoadTiles()
    {
        string tiles = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Tiles.json"));
        Tiles[] tileList = JsonHelper.getJsonArray<Tiles>(tiles);
        return tileList;
    }

    //load map json
    Maps[] LoadMaps()
    {
        string maps = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Maps.json"));
        Maps[] mapList = JsonHelper.getJsonArray<Maps>(maps);
        return mapList;
    }

    //load Obstacle Layout json
    ObstacleLayout[] LoadObstacleLayout()
    {
        string obsLayout = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "ObstacleLayout.json"));
        ObstacleLayout[] layoutList = JsonHelper.getJsonArray<ObstacleLayout>(obsLayout);
        return layoutList;
    }

    //load Enemy json
    Enemy[] LoadEnemies()
    {
        string enemies = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Enemies.json"));
        Enemy[] list = JsonHelper.getJsonArray<Enemy>(enemies);
        return list;
    }

    //load Enemy Layout json
    EnemyLayout[] LoadEnemyLayout()
    {
        string enemies = System.IO.File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "EnemyLayout.json"));
        EnemyLayout[] list = JsonHelper.getJsonArray<EnemyLayout>(enemies);
        return list;
    }

    //helper for loadlist
    int findElementOfName<T>(string name, T[] array) where T : Assets
    {
        int index = 0;
        foreach (T elem in array)
        {
            if (elem.name == name)
            {
                break;
            }
            index++;
        }
        return index;
    }

    //load list of asset information
    void loadList<T>(string[] names, List<T> list, T[] array) where T : Assets
    {
        foreach (string elem in names)
        {
            int index = findElementOfName<T>(elem, array);
            list.Add(array[index]);
        }
    }


    /* populate list with the tile layout
     * This function is goes through every tile and switched the sprite with a sprite indicated in out maps json
     * this allows for easy generation and alteration of maps to look more natural and random, but still keep
     * a catered feel. This also means we can use one scene to generate every type of encounter
     */
    void PopulateTiles()
    {
        print("Populating... ");
        int gridIndex = 0;
        foreach (Transform child in cellsParent)
        {
            Cell cell = child.GetComponent<Cell>();
            cell.cellNumber = (height - gridIndex % height) + gridIndex/ height + 5;
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            Sprite[] newSpriteSheet;
            //load tile sprite onto tile
            if (gridIndex < currentMap.tileLayout.Length)
            {     
                newSpriteSheet = Resources.LoadAll<Sprite>(tiles[currentMap.tileLayout[gridIndex]].spriteSheet);
                try
                {
                    sr.sprite = newSpriteSheet[tiles[currentMap.tileLayout[gridIndex]].sprite];
                }
                catch { }
            }
            //if nothing is indicated, load the default tile, with random variation
            else
            {
                newSpriteSheet = Resources.LoadAll<Sprite>(tiles[0].spriteSheet);
                try
                {
                    int insert = Random.Range(0, 9);
                    //this will insert the default tile(which is the first specified in the list (so as to reduce work and clutter in the json)
                    //for example, if there are 300 tiles and we fill in 100 in the json, this will fill the rest of them
                    //the random tile is just for flavor, so it's not just a blank field
                    if (insert != 0)
                    {
                        sr.sprite = newSpriteSheet[tiles[0].sprite];
                    }
                    else
                    {
                        sr.sprite = newSpriteSheet[tiles[tiles.Count-1].sprite];
                    }
                    
                }
                catch
                {
                }

                
            }
            gridIndex++;

        }
    }

    /* populate map with obstacles from the Obstacle layout
     * This function places every obstacle (specfied by (x, y, type)) indicated in the obstacle layout json
     * this layout is randomly picked from a preset list, indicated in the maps json
     * This removes the bugs that may occur from truely random obstacles
     * The function also replaces the sprites on the obstacle template so we can have different
     * types of obstacles visually, that have the same mechanical effect
     */
    void PopulateObstacles()
    {
        Sprite[] newSpriteSheet;
        List<Sprite> obsSprites = new List<Sprite>();

        //populate a list of sprites to be used
        foreach (string name in obstacleLayout.types)
        {
            int obsIndex = findElementOfName<Obstacles>(name, obsList);
            newSpriteSheet = Resources.LoadAll<Sprite>(obsList[obsIndex].spriteSheet) as Sprite[];
            print("Obstacle: " + newSpriteSheet[0]);
            obsSprites.Add(newSpriteSheet[obsList[obsIndex].sprite]);
        }

        //place the obstacles and replace thier sprites
        for (int i = 0; i< obstacleLayout.layout.Length; i+=3)
        {
            int x = obstacleLayout.layout[i];
            int y = obstacleLayout.layout[i+1];
            Sprite s = obsSprites[obstacleLayout.layout[i+2]];

            Transform cell = cellsParent.GetChild(x + y * currentMap.height);
            cell.GetComponent<Cell>().IsTaken = true;

            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.GetComponent<SpriteRenderer>().sprite = s;
            obstacle.transform.position = cell.position + new Vector3(0, 0, 0.1f);
            obstacle.transform.parent = ObstaclesParent.transform;
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = cell.GetComponent<Cell>().cellNumber;
        }
    }

    /* populate map with a set of enemies, indicated in map json
     * This function takes the id of the list of enemies specified in the map json, gets the list in
     * the enemies layout json with the id, and populates the map with enemies. This allows for the 
     * easy placement of enemies from a pregenerated list, which keeps things varied, but not too random
     * The function will also load up the enemy's stats and sprite and place it into the prefab
     */
    void PopulateEnemies()
    {
        Sprite[] newSpriteSheet;
        List<Sprite> enemySprites = new List<Sprite>();
        List<Sprite> enemySpritesMasks = new List<Sprite>();
        List<Enemy> enemyStats = new List<Enemy>();
        
        //populate types of enemies to be used into a list
        foreach (string name in enemyLayout.types)
        {
            int enemyIndex = findElementOfName<Enemy>(name, enemyList);
            newSpriteSheet = Resources.LoadAll<Sprite>(enemyList[enemyIndex].spriteSheet);

            enemySprites.Add(newSpriteSheet[enemyList[enemyIndex].sprite]);

            newSpriteSheet = Resources.LoadAll<Sprite>(enemyList[enemyIndex].spriteSheet + " - Copy");
            enemySpritesMasks.Add(newSpriteSheet[enemyList[enemyIndex].sprite]);

            enemyStats.Add(enemyList[enemyIndex]);
        }
        //place enemies onto the map, set their stats, and thier sprite
        print("Layout Length: " + enemyLayout.layout.Length);
        for (int i = 0; i < enemyLayout.layout.Length; i += 3)
        {
            int x = enemyLayout.layout[i];
            int y = enemyLayout.layout[i + 1];
            Sprite s = enemySprites[enemyLayout.layout[i + 2]];
            Sprite sm = enemySpritesMasks[enemyLayout.layout[i + 2]];
            Enemy e = enemyStats[enemyLayout.layout[i + 2]];

            Transform cell = cellsParent.GetChild(x + y * currentMap.height);

            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = s;
            enemy.transform.Find("Marker").GetComponent<SpriteRenderer>().sprite = sm;
            enemy.transform.position = cell.position;
            enemy.transform.parent = UnitParent.transform;
            enemy.GetComponent<Unit>().Cell = cell.GetComponent<Cell>();

            enemy.GetComponent<Unit>().HitPoints = e.hp;
            enemy.GetComponent<Unit>().Armor = e.armor;
            enemy.GetComponent<Unit>().AttackFactor = e.attack;
            enemy.GetComponent<Unit>().GunAttack = e.gun;
            enemy.GetComponent<Unit>().AttackRange = e.range;
            enemy.GetComponent<Unit>().MovementPoints = e.movement;
            enemy.GetComponent<Unit>().UnitName = e.name;
            enemy.GetComponent<Unit>().Speed = e.speed;

            enemy.GetComponent<Unit>().Initialize();
            foreach (SpriteRenderer sr in enemy.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.sortingOrder = enemy.GetComponent<Unit>().Cell.cellNumber;
            }
        }
    }

    void SetHeroStats()
    {
        Unit kroner = UnitParent.transform.Find("Kroner").GetComponent<Unit>();
        Unit alexei = UnitParent.transform.Find("Alexei").GetComponent<Unit>();
        Unit lee = UnitParent.transform.Find("Lee").GetComponent<Unit>();

        lee.HitPoints = battleState.Lee.getHP();
        lee.Armor = battleState.Lee.getArmour();
        lee.AttackFactor = battleState.Lee.getAttack();
        lee.GunAttack = battleState.Lee.getGunAttack();
        lee.Speed = battleState.Lee.getSpeed();
        lee.AttackRange = battleState.Lee.getRange();
        lee.MovementPoints = battleState.Lee.getMoveRange();

        kroner.HitPoints = battleState.Kroner.getHP();
        kroner.Armor = battleState.Kroner.getArmour();
        kroner.AttackFactor = battleState.Kroner.getAttack();
        kroner.GunAttack = battleState.Kroner.getGunAttack();
        kroner.Speed = battleState.Kroner.getSpeed();
        kroner.AttackRange = battleState.Kroner.getRange();
        kroner.MovementPoints = battleState.Kroner.getMoveRange();

        alexei.HitPoints = battleState.Alexei.getHP();
        alexei.Armor = battleState.Alexei.getArmour();
        alexei.AttackFactor = battleState.Alexei.getAttack();
        alexei.GunAttack = battleState.Alexei.getGunAttack();
        alexei.Speed = battleState.Alexei.getSpeed();
        alexei.AttackRange = battleState.Alexei.getRange();
        alexei.MovementPoints = battleState.Alexei.getMoveRange();

    }
}
