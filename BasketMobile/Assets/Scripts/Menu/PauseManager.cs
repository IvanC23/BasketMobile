using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private SliderController _sliderController;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _pauseMenu;

    // Component used to hide different UI elements when the game is paused
    // and to show the pause menu, which allows the player to resume, return to the main menu, or restart the game


    // Pausing the game, we also disable the component binded to the slider, to avoid interactions with the screen
    // activating its animations

    public void Pause()
    {
        AudioManager.instance.PlayMusicByName("Click");
        Time.timeScale = 0;
        _pauseButton.SetActive(false);
        _sliderController.ResetTouch();
        _sliderController.enabled = false;
        _pausePanel.SetActive(true);
        _pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        AudioManager.instance.PlayMusicByName("Click");
        Time.timeScale = 1;
        _pauseButton.SetActive(true);
        _sliderController.enabled = true;
        _pausePanel.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    public void Return()
    {
        AudioManager.instance.StopMusicByName("Burning");
        AudioManager.instance.PlayMusicByName("Click");
        SceneManager.LoadScene("Menu");
    }

    public void Rematch()
    {
        AudioManager.instance.StopMusicByName("Burning");
        AudioManager.instance.PlayMusicByName("Click");
        SceneManager.LoadScene("Challenge");
    }
    public void Restart()
    {
        AudioManager.instance.StopMusicByName("Burning");
        AudioManager.instance.PlayMusicByName("Click");
        SceneManager.LoadScene("Practice");
    }
}
