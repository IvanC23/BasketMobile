using System.Collections;
using TMPro;
using UnityEngine;

public class StartUpController : MonoBehaviour
{
    [SerializeField] private GameObject _welcomeObject;
    [SerializeField] private TMP_Text _welcomeText;
    [SerializeField] private SliderController _slideController;

    private float _initialFontSize = 80f;
    private float _finalFontSize = 150f;
    private float _fontSizeChangeDuration = 1.0f;

    // Disable the slider controller on awake if it's enabled
    private void Awake()
    {
        if (_slideController.enabled)
            _slideController.enabled = false;
    }

    private void Start()
    {
        // Start the welcome coroutine when the scene starts
        StartCoroutine(Welcome());
    }

    // Coroutine to display the welcome message
    private IEnumerator Welcome()
    {
        // Freeze time to display the welcome message
        Time.timeScale = 0;

        // Show "3" with scaling font size animation
        _welcomeText.text = "3";
        _welcomeObject.SetActive(true);
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        // Show "2" with scaling font size animation
        _welcomeText.text = "2";
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        // Show "1" with scaling font size animation
        _welcomeText.text = "1";
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        // Set final font size and display "GO!"
        _welcomeText.fontSize = (int)_finalFontSize;
        _welcomeText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

        // Hide the welcome message, resume time, and enable the slider controller
        _welcomeObject.SetActive(false);
        Time.timeScale = 1;
        _slideController.enabled = true;
    }

    // Coroutine to scale font size gradually over time
    private IEnumerator ScaleFontSizeOverTime(float startSize, float endSize, float duration)
    {
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            // Calculate the progress of the animation
            float t = timeElapsed / duration;

            // Interpolate font size based on progress
            _welcomeText.fontSize = (int)Mathf.Lerp(startSize, endSize, t);

            // Increment time elapsed
            timeElapsed += Time.unscaledDeltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final font size is set
        _welcomeText.fontSize = (int)endSize;
    }
}
