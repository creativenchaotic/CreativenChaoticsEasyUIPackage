using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    //Options Menu Buttons
    [SerializeField] private Button LeftDifficultyArrow;
    [SerializeField] private Button RightDifficultyArrow;
    [SerializeField] private TextMeshProUGUI DifficultyText;
    [SerializeField] private Slider MusicVolSlider;
    [SerializeField] private Slider SFXVolSlider;
    [SerializeField] private Button OptionsExitButton;

    /// <summary>
    /// Difficulty level variables come from my game Popup Master. Feel free to tweak this if you plan to include 
    /// difficulty levels or something similar in your projects!
    /// </summary>
    private int DifficultyLevel;
    private string DifficultyLevelTextToSet;

    // Start is called before the first frame update
    void Start()
    {
        SetDefaultValues();
        SetDifficulty();

        SetUIVisibility(true);
        OptionsMenuListeners();


      
    }

    void OptionsMenuListeners()
    {
        ///Difficulaty arrows currently do not do anything. Add implementations as needed
        LeftDifficultyArrow.onClick.AddListener(delegate {
            DifficultyLevel -= 1;
            SetDifficulty();
            SetPlayerPreferences();
        });

        RightDifficultyArrow.onClick.AddListener(delegate {
            DifficultyLevel += 1;
            SetDifficulty();
            SetPlayerPreferences();
        });
        //-------------------------------------------------------------------------------

        MusicVolSlider.onValueChanged.AddListener(delegate {
            SetPlayerPreferences();
        });

        SFXVolSlider.onValueChanged.AddListener(delegate {
            SetPlayerPreferences();
        });

        OptionsExitButton.onClick.AddListener(delegate {
            SetUIVisibility(false);
        });
    }

    private void SetUIVisibility(bool visibility)
    {
        this.gameObject.SetActive(visibility);
    }

    /// <summary>
    /// Function that handles setting the difficulty for the game. Feel free to tweak this or replace this as needed.
    /// </summary>
    
    void SetDifficulty()
    {

        if (DifficultyLevel<0){
            DifficultyLevel = 0;
        }
        else if (DifficultyLevel > 2)
        {
            DifficultyLevel = 2;
        }

        switch (DifficultyLevel)
        {
            case 0:
                DifficultyLevelTextToSet = "Clicker Amateur (2:00)";
                break;
            case 1:
                DifficultyLevelTextToSet = "Clicker Guru (1:00)";
                break;
            case 2:
                DifficultyLevelTextToSet = "Clicker Master (00:30)";
                break;
        }

        DifficultyText.SetText(DifficultyLevelTextToSet);
    }

    void SetPlayerPreferences()
    {
        PlayerPrefs.SetInt("music volume", (int)(MusicVolSlider.value * 100));
        PlayerPrefs.SetInt("sfx volume", (int)(SFXVolSlider.value * 100));
        PlayerPrefs.SetInt("difficulty level", DifficultyLevel); //<--- Currently not in use
    }

    void SetDefaultValues()
    {
        DifficultyLevel = 0;
        MusicVolSlider.value = 0.5f;
        SFXVolSlider.value = 0.5f;
    }
}
