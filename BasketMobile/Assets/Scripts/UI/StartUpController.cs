using System.Collections;
using TMPro;
using UnityEngine;

public class StartUpController : MonoBehaviour
{
    [SerializeField] private GameObject _welcomeObject;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private TMP_Text _welcomeText;
    [SerializeField] private SliderController _slideController;
    [SerializeField] private VictoryManager _victoryManager;


    private float _initialFontSize = 80f;
    private float _finalFontSize = 150f;
    private float _fontSizeChangeDuration = 1.0f;

    // Disable the slider controller on awake if it's enabled
    // This is done to avoid the player to move the slider before the game starts
    private void Awake()
    {
        if (_slideController.enabled)
            _slideController.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(Welcome());
    }

    // Coroutine to display the initial countdown
    private IEnumerator Welcome()
    {
        Time.timeScale = 0;

        AudioManager.instance.PlayMusicByName("Countdown");

        _welcomeText.text = "3";
        _welcomeObject.SetActive(true);
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        _welcomeText.text = "2";
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        _welcomeText.text = "1";
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        _welcomeText.fontSize = (int)_finalFontSize;
        _welcomeText.text = "GO!";
        AudioManager.instance.PlayMusicByName("Whistle");
        yield return new WaitForSecondsRealtime(0.5f);

        _welcomeObject.SetActive(false);

        if (_victoryManager != null)
            _victoryManager.StartGame();

        Time.timeScale = 1;
        _pauseButton.SetActive(true);
        _slideController.enabled = true;
    }

    // Coroutine to scale font size gradually over time
    private IEnumerator ScaleFontSizeOverTime(float startSize, float endSize, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            _welcomeText.fontSize = (int)Mathf.Lerp(startSize, endSize, t);

            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        _welcomeText.fontSize = (int)endSize;
    }
}
