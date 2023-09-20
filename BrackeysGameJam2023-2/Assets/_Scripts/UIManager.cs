using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Sections")]
    public GameObject HUD;
    public GameObject menu;
    [Header("Pause UI")] 
    public GameObject pauseMenu;
    public GameObject pauseMenuDefaultActiveButton;

    [Header("Banners")]
    public GameObject arrestBanner;
    public GameObject winBanner;
    [Header("Game Over UI")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverCost;
    public TextMeshProUGUI gameOverSmashed;
    public TextMeshProUGUI gameOverTimeToTerm;
    public GameObject gameOverMenuContinue;
    [Header("Components")]
    public EventSystem eventSystem;
    
    private void OnEnable()
    {
   
        GameStateManager.OnGameOver += OnGameOver;
        GameStateManager.OnGamePaused += Pause;

        PlayerArrest.onArrest += ShowArrestBanner;
        PlayerController.onWin += ShowWinBanner;

    }

    private void OnDisable()
    {
        GameStateManager.OnGameOver -= OnGameOver;
        GameStateManager.OnGamePaused -= Pause;


        PlayerArrest.onArrest -= ShowArrestBanner;
        PlayerController.onWin -= ShowWinBanner;

    }

    
    void OnGameOver()
    {
        menu.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverCost.text = $"${GameStateManager.Instance.GetGameState().costOfDamageCaused.ToString("00.00")}";
        gameOverSmashed.text = GameStateManager.Instance.GetGameState().objectsSmashed.ToString("0");
        gameOverTimeToTerm.text = GameStateManager.Instance.GetGameState().playTime.ToString("0");
        eventSystem.SetSelectedGameObject(gameOverMenuContinue);
    }

    void Pause(bool toggle)
    {
        //Toggle the hud the opposite of pause (On when unpaused)
        HUD.SetActive(!toggle);
        //Toggle Pause UI
        menu.SetActive(toggle);
        pauseMenu.SetActive(toggle);
        //If paused, select default button.
        if(toggle)
            eventSystem.SetSelectedGameObject(pauseMenuDefaultActiveButton);
    }

    void ShowArrestBanner()
    {
        arrestBanner.SetActive(true);
    }
    void ShowWinBanner()
    {
        winBanner.SetActive(true);
    }

}
