using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroStats {


	public string name;
	private int HP;
	private int Armour;
	private int Attack;
	private int GunAttack;
	private int Speed;
	private int Range;
	private int MoveRange;

	public HeroStats(string Name){
		this.name = Name;
		this.HP = 30;
		this.Armour = 30;
		if (Name == "Lee") {
			this.HP = 8;
			this.Armour = 9;
			this.Attack = 4;
			this.GunAttack = 5;
			this.Speed = 6;
			this.Range = 6;
			this.MoveRange = 5;
		}
		if (Name == "Kroner") {
			this.HP = 10;
			this.Armour = 8;
			this.Attack = 4;
			this.GunAttack = 6;
			this.Speed = 5;
			this.Range = 6;
			this.MoveRange = 5;
		}
		if (Name == "Alexei") {
			this.HP = 12;
			this.Armour = 13;
			this.Attack = 4;
			this.GunAttack = 4;
			this.Speed = 4;
			this.Range = 6;
			this.MoveRange = 5;
		}

	}
	// Class getters
	public int getHP(){return this.HP;}
	public int getArmour(){return this.Armour;}
	public int getAttack(){return this.Attack;}
	public int getGunAttack(){return this.GunAttack;}
	public int getSpeed(){return this.Speed;}
	public int getRange(){return this.Range;}
	public int getMoveRange(){return this.MoveRange;}
	// Class Setters
	public void setHP(int n){this.HP = n;}
	public void setArmour(int n){this.Armour = n;}
	public void setAttack(int n){this.Attack = n;}
	public void setGunAttack(int n){this.GunAttack = n;}
	public void setSpeed(int n){this.Speed = n;}
	public void setRange(int n){this.Range = n;}
	public void setMoveRange(int n){this.MoveRange = n;}

}
