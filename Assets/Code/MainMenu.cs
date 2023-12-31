using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //UI Groups
    [SerializeField] public GameObject MainMenuButtons;
    [SerializeField] public GameObject GameLogo;
    [SerializeField] public GameObject OptionsMenu;

    //Main Menu Buttons
    [SerializeField] public Button StartGameButton;
    [SerializeField] public Button EndGameButton;
    [SerializeField] public Button OptionsButton;

    [SerializeField] private Button OptionsExitButton;

    void Start()
    {
        SetUIVisibility(true);

        //Listeners
        MainMenuListeners();
    }

    void Update()
    {
        
    }

    private void SetUIVisibility(bool visibilityValue)
    {
        MainMenuButtons.SetActive(visibilityValue);
        GameLogo.SetActive(visibilityValue);
        OptionsMenu.SetActive(!visibilityValue);
    }

    void ExitGame()
    {
        Application.Quit();
    }

    void MainMenuListeners()
    {
        //Main Menu Button Listeners
        StartGameButton.onClick.AddListener(delegate {
            SceneManager.LoadScene("Level");//Replace name of the scene with whatever scene you want when starting the game
        });

        OptionsButton.onClick.AddListener(delegate {
            SetUIVisibility(false);//Options menu is set up based on visibility instead of being a separate scene. If you want it to be a separate scene use code similar to above.
        });

        EndGameButton.onClick.AddListener(delegate
        {
            ExitGame();
        });

        OptionsExitButton.onClick.AddListener(delegate {
            SetUIVisibility(true);
        });
    }

}
