using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] TextMeshProUGUI pauseMenuTitle;
    [SerializeField] Button optionsButton;
    [SerializeField] Button exitButton;

    [SerializeField] private Button OptionsExitButton;


    // Start is called before the first frame update
    void Start()
    {
        SetUIVisibility(true);
        SetupPauseMenuListeners();
    }

    void SetUIVisibility(bool visibility)
    {
        pauseMenuTitle.gameObject.SetActive(visibility);
        optionsButton.gameObject.SetActive(visibility);
        exitButton.gameObject.SetActive(visibility);
        OptionsMenu.SetActive(!visibility);
    }

    void SetupPauseMenuListeners()
    {
        optionsButton.onClick.AddListener(delegate {
            SetUIVisibility(false);
        });

        exitButton.onClick.AddListener(delegate { Application.Quit(); });

        OptionsExitButton.onClick.AddListener(delegate
        {
            SetUIVisibility(true);
        });
    }
}
