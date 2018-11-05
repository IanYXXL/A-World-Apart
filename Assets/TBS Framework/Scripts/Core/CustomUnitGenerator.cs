using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomUnitGenerator : MonoBehaviour, IUnitGenerator
{
    public Transform UnitsParent;
    public Transform CellsParent;

    public Dictionary<string, Dictionary<string, AudioClip>> unitAudio;
    public SFXLoader sfx;
    /// <summary>
    /// Returns units that are already children of UnitsParent object.
    /// </summary>
    /// 

    public virtual void Awake()
    {
        sfx = GameObject.Find("SFX Source").GetComponent<SFXLoader>();
        unitAudio = new Dictionary<string, Dictionary<string, AudioClip>>() {
            {"Lee", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Kroner", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Alexei", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Eyebot", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Robot", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Boar", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"K8", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Morphe", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Soldier", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Soldier1", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Soldier2", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Soldier3", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Soldier4", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Usarmy1", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Usarmy2", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Usarmy3", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) },
            {"Usarmy4", (new Dictionary<string, AudioClip>(){ { "atkHp", sfx.Crossbow }, { "atkArmor", sfx.Crossbow2 }, { "gun", sfx.LeesGun }, { "aoe", sfx.Grenade },{ "move", sfx.Walk1 },{ "death", sfx.Grenade } }) }

        };
    }
    public List<Unit> SpawnUnits(List<Cell> cells)
    {
        List<Unit> ret = new List<Unit>();
        for (int i = 0; i < UnitsParent.childCount; i++)
        {
            var unit = UnitsParent.GetChild(i).GetComponent<Unit>();
            if(unit !=null)
            {
                var cell = cells.OrderBy(h => Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
                if (!cell.IsTaken)
                {
                    cell.IsTaken = true;
                    unit.Cell = cell;
                    unit.transform.position = cell.transform.position;
                    unit.Initialize();
                    ret.Add(unit);//Unit gets snapped to the nearest cell
                    print(unit.UnitName);
                    print(unitAudio[unit.UnitName].ToString());
                    unit.atkArmorAud = unitAudio[unit.UnitName]["atkArmor"];
                    unit.atkHpAud = unitAudio[unit.UnitName]["atkHp"];
                    unit.gunAud = unitAudio[unit.UnitName]["gun"];
                    unit.aoeAud = unitAudio[unit.UnitName]["aoe"];
                    unit.moveAud = unitAudio[unit.UnitName]["move"];
                    unit.deathAud = unitAudio[unit.UnitName]["death"];

                }
                else
                {
                    Destroy(unit.gameObject);
                }//If the nearest cell is taken, the unit gets destroyed.
            }
            else
            {
                Debug.LogError("Invalid object in Units Parent game object");
            }
            
        }
        return ret;
    }

    public void SnapToGrid()
    {
        List<Transform> cells = new List<Transform>();

        foreach(Transform cell in CellsParent)
        {
            cells.Add(cell);
        }

        foreach(Transform unit in UnitsParent)
        {
            var closestCell = cells.OrderBy(h => Math.Abs((h.transform.position - unit.transform.position).magnitude)).First();
            if (!closestCell.GetComponent<Cell>().IsTaken)
            {
                Vector3 offset = new Vector3(0,0, closestCell.GetComponent<Cell>().GetCellDimensions().z);
                unit.position = closestCell.transform.position - offset;
            }//Unit gets snapped to the nearest cell
        }
    }
}

