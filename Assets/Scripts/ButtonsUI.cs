using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsUI : MonoBehaviour
{
    [SerializeField] private GameObject StartGameButton;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject RulesButton;
    [SerializeField] private GameObject RulesImage;
    [SerializeField] private GameObject GameCanvas;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;
    public void BackToMenu()
    {
        
        GameCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void Retry()
    {
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
        GameCanvas.SetActive(true);
    }

    public void CloseRules()
    {
        StartGameButton.SetActive(true);
        ExitButton.SetActive(true);
        RulesButton.SetActive(true);
        RulesImage.SetActive(false);
    }

    public void OpenRules()
    {
        StartGameButton.SetActive(false);
        ExitButton.SetActive(false);
        RulesButton.SetActive(false);
        RulesImage.SetActive(true);
    }

    public void StartGame() 
    {
        LosePanel.SetActive(false);
        WinPanel.SetActive(false);
        GameCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }

   
}
