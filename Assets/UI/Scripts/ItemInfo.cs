using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//This script states the type of item and what values that it passes to the player, will be used for item prefabs//
public class ItemInfo : MonoBehaviour {


	public string name;									// Name of the item
	public int type;									// What type the item is
	public string description;							// A Description of the item, lore can be included
	public int level;									// The level of the item, retrived from LevelInfo
	public float HP;									// The change in the Hitpoint stat of the assigned character
	public float AR;									// The change in the Attack Range stat of the assigned character
	public float AF;									// The change in the Attack Factor stat of the assigned character
	public float DF;									// The change in the Defence Factor stat of the assigned character
	public float SP;									// The change in the Speed stat of the assigned character
	public float MV;									// The change in the Movement Points stat of the assigned character
	public int activePeriod;							// The time that the item is in effect
	public int cooldown;								// The max cooldown of the specific item
	private int cdi;									//	cooldown index
	public int amount;									// The number of uses for the item
	private LevelInfo ls;								//	The LevelInfo script used for or level value 
	public Unit character;								//	The assigned unit
	public List<Buff> buffs;							// The list of buffs applied from the item


	// Use this for initialization, it is the pipeline for the item creation.
	void Start () {
		//ls = Instantiate (gameObject).GetComponentInChildren <LevelInfo>();
		ls = gameObject.GetComponentInChildren <LevelInfo>();
		getLevel ();
		setType ();
		//createBuff ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Moved the cooldown index down by one, if it reaches zero, then the timer is reset
	void cooldowntick(){
		cdi--;
		if (cdi == 0) {
			cdi = cooldown;
		}
	}
	// Automatically sets the type of the item if the developer accidentally added uneccisarry values
	void setType(){
		if (type == 0) {	//Item is a one time consumable that is removed
			cooldown = 0;
			cdi = 0;
			amount = 0;
		}
		if (type == 1) {	//Item is a reusable consumable with possible cooldown

		}
		if (type == 2) {	//Item is a passive item that cannot be interacted with
			cooldown = 0;
			cdi = 0;
			amount = 0;
		}
	}
	// Retrieves the value of the item's assigned level
	void getLevel(){
		level = ls.getlevel ();
		HP = HP * ((float)level+1);
		AR = AR * ((float)level+1);						
		AF = AF * ((float)level+1);							
		DF = DF * ((float)level+1);							
		SP = SP * ((float)level+1);								
		MV = MV * ((float)level+1);

	}
	//Set character
	// TODO have it so that if the item is not assigned to a character, it does not assign to anything
	void setCharacter(){
		//sets the new characte if the item is not applied to anything
	}
	//Applies the item changes to the assigned character
	void applyBuff(){
		foreach (Buff b in buffs) {
			b.Apply (character);
		}
	}
	//Removes the item properties to the assigned character.
	void removeBuff(){
		foreach(Buff b in buffs){
			b.Undo (character);
		}
	}
	//This takes the item values and translates them into a list of buffs.
    /*
	void createBuff(){
		List<Buff> temp = new List<Buff>();
		//Passive item buff creations
		if (type == 2) {
			if (HP != 0) {
				temp.Add (new HealingBuff (1, (int)HP));
			}
			if (AR != 0) {
				//TODO create an attack range buff to increase attack range;
			}
			if (AF != 0) {
				temp.Add (new AttackBuff (-1, (int)AF));
			}
			if (DF != 0) {
				temp.Add (new DefenceBuff (-1, (int)DF));
			}
			if (SP != 0) {
				//TODO create an speed buff to increase attack range;
			}
			if (MV != 0) {
				//TODO create an movementpoints buff to increase attack range;
			}
		}
		//Consumable items
		//TODO fix the Consumables by changing them to our version of the stats, not the built in stats
		if (type < 2) {
			if (HP != 0) {
				temp.Add (new HealingBuff (activePeriod, (int)HP));
			}
			if (AR != 0) {
				//TODO create an attack range buff to increase attack range;
			}
<<<<<<< HEAD
				temp.Add (new AttackBuff (activePeriod, AF));
=======
			if (AF != 0) {
				temp.Add (new AttackBuff (activePeriod, (int)AF));
>>>>>>> 381173cdf8c0b7cc6dfabeac94c326eef8d1ea55
			}
			if (DF != 0) {
				temp.Add (new DefenceBuff (activePeriod, (int)DF));
			}
			if (SP != 0) {
				//TODO create an speed buff to increase attack range;
			}
			if (MV != 0) {
				//TODO create an movementpoints buff to increase attack range;
			}
		}
		//return temp buffs to the buffs array to be applied
		buffs = temp;
	}*/
}