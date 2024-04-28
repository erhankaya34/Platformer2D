using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject KeyBindingsPanel;
    public GameObject pauseMenu;
    private bool isPaused = false;
    
    public Slider masterVolumeSlider; 
    public Slider effectVolumeSlider; 

    private void Awake()
    {
        KeyBindingsPanel.SetActive(false);
        pauseMenu.SetActive(false);

        // AudioManager'dan ayarladığımız ses seviyelerini oto çekip yeni leveldaki sliderlara atadık
        // if (AudioManager.instance != null)
        // {
        //     masterVolumeSlider.value = AudioManager.instance.masterVol;
        //     effectVolumeSlider.value = AudioManager.instance.effectVol;
        // }
    }


    void Update()
    {
        CheckPauseToggle();
    }

    private void CheckPauseToggle()
    {
        // Escape'e basıldığında ve key bindings açıksa, key bindings'ı gizle.
        // Aksi takdirde, pause durumunu toggle et.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (KeyBindingsPanel.activeSelf)
            {
                HideKeyBindings();
            }
            else
            {
                TogglePause();
            }
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        KeyBindingsPanel.SetActive(false); // Emin olmak için key bindings panelini de gizle.

        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ShowKeyBindings()
    {
        KeyBindingsPanel.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void HideKeyBindings()
    {
        KeyBindingsPanel.SetActive(false);
        pauseMenu.SetActive(true);
    }
    
    public void ExitGame()
    {
        Application.Quit();
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