using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Web.UI;

public class MusicLoader : MonoBehaviour {
    public float master = 1.0f;
    public float music = 1.0f;
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
        Windy2;
	// Use this for initialization
	void Start () {
        loadVolume();
		switch (SceneManager.GetActiveScene().name)
        {
            case ("menuTest"):
                {
                    LoadCinMusic();
                    break;
                }
            case ("Map"):
                {
                    LoadA2Music();
                    break;
                }
            case ("menu"):
                {
                    LoadCinMusic();
                    break;
                }
            case ("TestBattle"):
                {
                    LoadC1Music();
                    break;
                }
            case ("Base"):
                {
                    LoadA4Music();
                    break;
                }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void SetMasterVolume(float setMaster){
        master = setMaster;
	}

    public void SetMusicVolume(float setMusic)
    {
        music= setMusic;
    }
    public void loadVolume()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            master = PlayerPrefs.GetFloat("masterVolume");
            
        }
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            music = PlayerPrefs.GetFloat("musicVolume");
            print("Music = " + music.ToString());
        }
    }

    public void LoadMusic(AudioClip audio)
    {
        gameObject.GetComponent<AudioSource>().clip =  audio;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * music;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadA1Music() {
        gameObject.GetComponent<AudioSource>().clip = A1;
		gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA2Music() {
        gameObject.GetComponent<AudioSource>().clip = A2;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * music;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA3Music() {
        gameObject.GetComponent<AudioSource>().clip = A3;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * music;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA4Music() {
        gameObject.GetComponent<AudioSource>().clip = A4;
        gameObject.GetComponent<AudioSource>().volume = 1f * master * music;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadA5Music()
    {
        gameObject.GetComponent<AudioSource>().volume = 1f * master * music;
        gameObject.GetComponent<AudioSource>().clip = A5;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadC1Music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = C1;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadC2Music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = C2;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadCinMusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Cin1;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadAmmoUsageSFX()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = AmmoUsage;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void LoadAreaIntromusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = AreaIntro;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadBossRoomIntromusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = BossRoomIntro;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadClaymoremusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Claymore;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadCrossbowmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Crossbow;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadDoormusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Door;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadEnemySwordHitmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = EnemySwordHit;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadEngiRobotDeploymusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = EngiRobotDeploy;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadGrenademusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Grenade;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadLeesGunmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = LeesGun;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadMagnitizermusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Magnitizer;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadMPPDDmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = MPPDD;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadNailGunmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = NailGun;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRapidFiringWeaponmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = RapidFiringWeapon;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRobotWeaponmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = RobotWeapon;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRoboticEnemymusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = RoboticEnemy;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadSpaceshipmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Spaceship;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadTransmissionmusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Transmission;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadWindymusic()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Windy;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadCrossbow2music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = Crossbow2;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadLeesGun2music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = LeesGun2;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadRoboticEnemy2music()
    {   gameObject.GetComponent<AudioSource>().volume = 1f*master*music;
        gameObject.GetComponent<AudioSource>().clip = RoboticEnemy2;
        gameObject.GetComponent<AudioSource>().Play();
    }
    public void LoadWindy2music()
    {
        gameObject.GetComponent<AudioSource>().volume = 1f * master * music;
        gameObject.GetComponent<AudioSource>().clip = Windy2;
        gameObject.GetComponent<AudioSource>().Play();
    }


}
