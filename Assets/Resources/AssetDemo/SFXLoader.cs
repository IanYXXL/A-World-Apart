using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLoader : MonoBehaviour {
    public float master = 1.0f;
    public float SFX = 1.0f;

    public AudioClip A1,A2,A3,A4,A5,C1,C2,Cin1;
    public AudioClip AmmoUsage,
        AreaIntro,
        BossRoomIntro,
        Claymore,
        Crossbow,
        Door,
        EnemySwordHit,
        EngiRobotDeploy,
        Grenade,
        LeesGun,
        Magnitizer,
        MPPDD,
        NailGun,
        RapidFiringWeapon,
        RobotWeapon,
        RoboticEnemy,
        Spaceship,
        Transmission,
        Windy,
        Crossbow2,
        LeesGun2,
        RoboticEnemy2,
        Walk1,
        Windy2;
	// Use this for initialization
	void Start () {
        loadVolume();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetMasterVolume(float setMaster){
        master = setMaster;
	}

    public void SetSFXVolume(float setSFX)
    {
        SFX = setSFX;
    }
    public void loadVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            master = PlayerPrefs.GetFloat("masterVolume");
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFX = PlayerPrefs.GetFloat("SFXVolume");
            print("SFX = " + SFX.ToString());
        }
    }

    public void LoadSFX(AudioClip audio, float delay)
    {
        gameObject.GetComponent<AudioSource>().clip = audio;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * SFX;
        gameObject.GetComponent<AudioSource>().PlayDelayed(delay);
    }

    public void LoadA1Music() {
        gameObject.GetComponent<AudioSource>().clip = A1;
		gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA2Music() {
        gameObject.GetComponent<AudioSource>().clip = A2;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * SFX;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA3Music() {
        gameObject.GetComponent<AudioSource>().clip = A3;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * SFX;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA4Music() {
        gameObject.GetComponent<AudioSource>().clip = A4;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * SFX;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA5Music()
    {
        gameObject.GetComponent<AudioSource>().volume = 1f * master * SFX;
        gameObject.GetComponent<AudioSource>().clip = A5;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadC1Music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = C1;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadC2Music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = C2;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadCinMusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Cin1;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadAmmoUsageSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = AmmoUsage;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadAreaIntroSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = AreaIntro;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadBossRoomIntroSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = BossRoomIntro;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadClaymoreSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Claymore;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadCrossbowSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Crossbow;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadDoorSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Door;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadEnemySwordHitSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = EnemySwordHit;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadEngiRobotDeploySFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = EngiRobotDeploy;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadGrenadeSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Grenade;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadLeesGunSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = LeesGun;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadMagnitizerSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Magnitizer;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadMPPDDSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = MPPDD;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadNailGunSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = NailGun;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRapidFiringWeaponSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = RapidFiringWeapon;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRobotWeaponSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = RobotWeapon;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRoboticEnemySFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = RoboticEnemy;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadSpaceshipSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Spaceship;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadTransmissionSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Transmission;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadWindySFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Windy;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadCrossbow2SFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = Crossbow2;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadLeesGun2SFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = LeesGun2;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRoboticEnemy2SFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*SFX;
        gameObject.GetComponent<AudioSource>().clip = RoboticEnemy2;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadWindy2SFX()
    {
        gameObject.GetComponent<AudioSource>().volume = 1f * master * SFX;
        gameObject.GetComponent<AudioSource>().clip = Windy2;
        gameObject.GetComponent<AudioSource>().Play();
    }


}
