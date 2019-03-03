﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject soundsMenu;
    public GameObject controlsMenu;
    public GameObject startMenuButton;
    public GameObject hud;

    public Slider masterVolume;
    public Slider backgroundVolume;
    public Slider fxVolume;

    public GameObject helpingObject;
    public Animator helpingTextAnim;
    public Text helpfulText;
    private bool animPlaying;
    private float helpingTimer = 0f;

    public TextMeshProUGUI bigGameOverText;
    public TextMeshProUGUI littleGameOverText;
    public GameObject gameOverMenu;

    public Text healthText;
    public Text ammoText;
    public Image boostMeter;

    private bool pauseEnable = false;
    private bool outOfMenus = false;

    public AudioManager audioManager;

    private bool fromStartMenu = false;

    public static UIManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        Time.timeScale = 0;
    }

    void Start()
    {
        helpingTextAnim = helpingObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animPlaying)
        {
            if (helpingTimer > 0f)
            {
                helpingTimer -= Time.deltaTime;
                if (helpingTimer < 0f)
                {
                    helpingTextAnim.SetTrigger("IN");
                    helpingTextAnim.ResetTrigger("OUT");
                    helpingTimer = 0f;
                    animPlaying = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseEnable && outOfMenus)
        {
            Settings();
            pauseEnable = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseEnable && outOfMenus)
        {
            OptionsBack();
            pauseEnable = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    private void FixedUpdate()
    {
        healthText.text = "" + PlayerStats.Instance.Health;
        ammoText.text = "" + PlayerStats.Instance.CurrentAmmo;

        boostMeter.fillAmount = PlayerStats.Instance.ThrusterCharge;
    }

    public void NewGame()
    {
        startMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        outOfMenus = true;
    }
    public void Continue()
    {
        startMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1;
        outOfMenus = true;
    }
    public void Options()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        startMenuButton.SetActive(false);
        fromStartMenu = true;
    }
    public void Settings()
    {
        optionsMenu.SetActive(true);
        fromStartMenu = false;
        startMenuButton.SetActive(true);
    }

    public void OptionsBack()
    {
        optionsMenu.SetActive(false);
        soundsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        if (fromStartMenu)
        {
            startMenu.SetActive(true);
        }
    }

    public void MasterVolume()
    {
        audioManager.backGroudSource.volume = masterVolume.value;
        audioManager.shotsSource.volume = masterVolume.value;
    }
    public void BackGroundMusic()
    {
        audioManager.backGroudSource.volume = backgroundVolume.value;
    }
    public void FXVolume()
    {
        audioManager.shotsSource.volume = fxVolume.value;
    }

    public void SoundMenu()
    {
        soundsMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }
    public void ControlsMenu()
    {
        controlsMenu.SetActive(true);
        soundsMenu.SetActive(false);
    }

    public void HelpText(string message)
    {
        animPlaying = true;
        helpfulText.text = message;
        helpingTextAnim.SetTrigger("OUT");
        helpingTextAnim.ResetTrigger("IN");
        helpingTimer = 10f;
    }
    public void StartMenu()
    {

    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            bigGameOverText.text = "Congradulations!";
            littleGameOverText.text = "You won!";
            gameOverMenu.SetActive(true);
        }
        else
        {
            bigGameOverText.text = "Game Over!";
            littleGameOverText.text = "Try again.";
            gameOverMenu.SetActive(true);
        }
    }

}
