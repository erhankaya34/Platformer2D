using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Animator _animator;
    public bool isOnOptionMenu;
    public GameObject KeyBindingsPanel;
    public bool isOnKeyBindings;
    
    public Slider masterVolumeSlider; 
    public Slider effectVolumeSlider; 




    private void Start()
    {
        isOnOptionMenu = false;
        isOnKeyBindings = false;
    }

    public void Update()
    {
        HideOptions();
        HideKeyBindings();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        _animator.SetBool("Show", true);
        isOnOptionMenu = true;
    }

    public void HideOptions()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) && isOnOptionMenu)
        {
            _animator.SetBool("Show", false);
            isOnOptionMenu = false;
        }
    }

    public void ShowKeyBindings()
    {
        KeyBindingsPanel.SetActive(true);
        isOnKeyBindings = true;
    }

    public void HideKeyBindings()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) && isOnKeyBindings)
        {
            KeyBindingsPanel.SetActive(false);
            isOnKeyBindings = false;
        }
    }
    
    public void OnEffectVolumeChanged()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetEffectVolume(effectVolumeSlider.value);
        }
    }
    
    public void OnMasterVolumeChanged()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMasterVolume(masterVolumeSlider.value);
        }
    }
}