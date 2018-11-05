using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleState{



	public static BattleState current;
    public bool finalBattle = false;
    public string mapName = "default";
    public int enemyID = 5;
	public HeroStats Kroner;
	public HeroStats Lee;
	public HeroStats Alexei;

	public BattleState(){
		Kroner = new HeroStats ("Kroner");
        Kroner.setHP(9);
        Kroner.setArmour(7);
        Kroner.setAttack(8);
        Kroner.setRange(1);
        Kroner.setMoveRange(6);
        Kroner.setSpeed(5);
        Kroner.setGunAttack(6);
		Lee = new HeroStats ("Lee");
        Lee.setHP(6);
        Lee.setArmour(13);
        Lee.setAttack(9);
        Lee.setRange(3);
        Lee.setMoveRange(4);
        Lee.setSpeed(11);
        Lee.setGunAttack(6);
        Alexei = new HeroStats ("Alexei");
        Alexei.setHP(10);
        Alexei.setArmour(11);
        Alexei.setAttack(7);
        Alexei.setRange(2);
        Alexei.setMoveRange(5);
        Alexei.setSpeed(3);
        Alexei.setGunAttack(5);

    }

}
