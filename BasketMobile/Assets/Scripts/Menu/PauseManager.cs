using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private SliderController _sliderController;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pauseMenu;

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseButton.SetActive(false);
        _sliderController.ResetTouch();
        _sliderController.enabled = false;
        _pausePanel.SetActive(true);
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseButton.SetActive(true);
        _sliderController.enabled = true;
        _pausePanel.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
}
