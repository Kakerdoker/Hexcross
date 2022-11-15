using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sound_Handler : MonoBehaviour
{
    AudioSource audioSourceAction, audioSourceHover, audioSourceAmbience, audioSourceMusic;
    UI_Stats_Handler uiStats;
        
    public AudioClip[] audioClip = new AudioClip[1];
    float timePassHover = 0f;
    public int musicVolumePercent, sfxVolumePercent;

    void Start()
    {   
        if(PlayerPrefs.GetInt("fristTimeCheck") != 1){
            PlayerPrefs.SetInt("fristTimeCheck", 1);
            PlayerPrefs.SetInt("sfxVolumePercent", 100);
            PlayerPrefs.SetInt("musicVolumePercent", 100);
            PlayerPrefs.SetInt("fpsLimit", 60);
        }

        musicVolumePercent = PlayerPrefs.GetInt("musicVolumePercent");
        sfxVolumePercent = PlayerPrefs.GetInt("sfxVolumePercent");

        if(GameObject.Find("Audio Source Ambience")){audioSourceAmbience = GameObject.Find("Audio Source Ambience").GetComponent<AudioSource>();
        audioSourceAmbience.volume = 0.1f * (float)musicVolumePercent / 100f;}

        if(GameObject.Find("Audio Source Music")){audioSourceMusic = GameObject.Find("Audio Source Music").GetComponent<AudioSource>();
        audioSourceMusic.volume = 1f * (float)musicVolumePercent / 100f;}

        if(GameObject.Find("Audio Source Action")){audioSourceAction = GameObject.Find("Audio Source Action").GetComponent<AudioSource>();
        audioSourceAction.volume = 0.05f * (float)sfxVolumePercent / 100f;}

        audioSourceHover = GameObject.Find("Audio Source Hover").GetComponent<AudioSource>();
        audioSourceHover.volume = 0.04f * (float)sfxVolumePercent / 100f;

        uiStats = gameObject.GetComponent<UI_Stats_Handler>();

        sfxVolumePercent = PlayerPrefs.GetInt("sfxVolumePercent");
        musicVolumePercent = PlayerPrefs.GetInt("musicVolumePercent");
    }

    public void changeMusicVolumeAndPlaySample(){
        audioSourceAmbience.volume = 0.1f * (float)musicVolumePercent / 100f;
        audioSourceMusic.volume = 1f * (float)musicVolumePercent / 100f;
        audioSourceMusic.Play();
    }

    public void changeSfxVolumeAndPlaySample(){
        audioSourceHover.volume = 0.04f * (float)sfxVolumePercent / 100f;
        audioSourceAction.volume = 0.05f * (float)sfxVolumePercent / 100f;
        playSoundWithRandomPitch(1);
    }

    public void playHoverButNotIfYouWon(){
        if(timePassHover > 0.05f && !uiStats.win){
            audioSourceHover.Play();
            timePassHover = 0f;
        }
    }

    public void playHover(){
        if(timePassHover > 0.05f){
            audioSourceHover.Play();
            timePassHover = 0f;
        }
    }

    System.Random random = new System.Random(Environment.TickCount);

    void playSoundWithRandomPitch(int audioIndex){
        audioSourceAction.pitch = (Convert.ToSingle(random.NextDouble()) / 10f) + 0.95f;
        audioSourceAction.Stop();
        audioSourceAction.clip = audioClip[audioIndex];
        audioSourceAction.Play();
    }
    void playSoundNormal(int audioIndex){
        audioSourceAction.Stop();
        audioSourceAction.clip = audioClip[audioIndex];
        audioSourceAction.Play();
    }



    public void playCancel(){
        if(!uiStats.win){
            playSoundWithRandomPitch(0);
        }
    }

    public void playConfirmPerfect(){
        if(!uiStats.win){
            playSoundWithRandomPitch(1);
        }
    }

    public void playConfirmError(){
        if(!uiStats.win){
            playSoundWithRandomPitch(2);
        }
    }

    public void playUncrumblePaper(){
        playSoundNormal(3);
    }

    public void playCrumblePaper(){
        playSoundNormal(4);
    }
    public void playButtonClick(){
        playSoundWithRandomPitch(5);
    }
    public void playGameCompleted(){
        playSoundNormal(6);
    }
    public void playSadTrombone(){
        playSoundNormal(7);
    }
    public void playNewHighscore(){
        playSoundNormal(8);
    }
    public void playSweep(){
        playSoundNormal(9);
    }
    public void playSweepReverese(){
        playSoundNormal(8);
    }

    

    void Update()
    {
        timePassHover += Time.deltaTime;
    }
}
