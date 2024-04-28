using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer MusicMixer, EffectMixer;

    [SerializeField] private float _footstepCooldown = 0.35f;
    [SerializeField] private float _lastFootstepTime = -1f;

    [Range(-80, 20)] public float effectVol, masterVol;

    public AudioSource
        SwordAS,
        Sword2AS,
        EnemeyDieAS,
        ObjectiveCompletedAS,
        AmbienceMusicAS,
        HealingAS,
        PlayerDamageAS,
        PlayerDieAS,
        FailAS,
        TrapSoundAS,
        TeleportAS,
        EnemyDamageAS,
        JumpAS,
        FootStep1AS,
        FootStep2AS,
        FootStep3AS,
        FootStep4AS,
        NotEnoughManaAS,
        BackgroundMusicAS,
        DashAS;

    public Slider masterSlider, effectSlider;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        masterSlider.value = masterVol;
        effectSlider.value = effectVol;

        masterSlider.minValue = -80;
        masterSlider.maxValue = 20;

        effectSlider.minValue = -80;
        effectSlider.maxValue = 20;

        masterSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
    }
    

    public void PlayAudio(AudioSource audio)
    {
        audio.Play();
    }

    public void PlayRandomFootstep()
    {
        if (Time.time - _lastFootstepTime >= _footstepCooldown)
        {
            int rand = Random.Range(1, 5);
            AudioSource clipToPlay = null;
            switch (rand)
            {
                case 1:
                    clipToPlay = FootStep1AS;
                    break;
                case 2:
                    clipToPlay = FootStep2AS;
                    break;
                case 3:
                    clipToPlay = FootStep3AS;
                    break;
                case 4:
                    clipToPlay = FootStep4AS;
                    break;
            }

            PlayAudio(clipToPlay);
            _lastFootstepTime = Time.time;
        }
    }
    
    
    public void SetMasterVolume(float volume)
    {
        masterVol = volume;
        MusicMixer.SetFloat("masterVolume", volume);
    }

    public void SetEffectVolume(float volume)
    {
        effectVol = volume;
        EffectMixer.SetFloat("effectVolume", volume);
    }

}