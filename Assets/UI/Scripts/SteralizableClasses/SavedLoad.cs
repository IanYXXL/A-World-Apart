using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SavedLoad{

    public static BattleState savedHeroStats = new BattleState();

	//This value writes the hero stats to a file for scene changes
	public static void Write(){
		//savedHeroStats.Add(BattleState.current);
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/HeroStats.gd");
		bf.Serialize (file, savedHeroStats);
		file.Close ();
	}

	//This value reads the hero stats to a file for scene changes, more specifically the comabt phase
	public static BattleState Read(){
		if (File.Exists (Application.persistentDataPath + "/HeroStats.gd")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/HeroStats.gd", FileMode.Open);
			savedHeroStats = (BattleState)bf.Deserialize (file);
			file.Close ();
            return savedHeroStats;
        }
        else
        {
            return null;
        }
	}
}
