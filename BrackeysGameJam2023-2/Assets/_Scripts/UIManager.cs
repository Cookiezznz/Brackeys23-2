using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverCost;
    public TextMeshProUGUI gameOverSmashed;
    public TextMeshProUGUI gameOverTimeToTerm;

    public GameObject arrestBanner;
    
    private void OnEnable()
    {
   
        GameStateManager.OnGameOver += OnGameOver;

        PlayerArrest.onArrest += ShowArrestBanner;
    }
    
    private void OnDisable()
    {
        GameStateManager.OnGameOver -= OnGameOver;

        PlayerArrest.onArrest -= ShowArrestBanner;

    }

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.OnGameOver += OnGameOver;
    }
    
    void OnGameOver()
    {
        menu.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverCost.text = $"${GameStateManager.Instance.GetGameState().costOfDamageCaused.ToString("00.00")}";
        gameOverSmashed.text = GameStateManager.Instance.GetGameState().objectsSmashed.ToString("0");
        gameOverTimeToTerm.text = GameStateManager.Instance.GetGameState().playTime.ToString("0");
    }

    void ShowArrestBanner()
    {
        arrestBanner.SetActive(true);
    }
    
}
