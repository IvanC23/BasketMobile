using System.Collections;
using TMPro;
using UnityEngine;

public class StartUpControllerSolo : MonoBehaviour
{
    [SerializeField] private GameObject _welcomeObject;
    [SerializeField] private TMP_Text _welcomeText;
    [SerializeField] private SliderController _slideController;
    [SerializeField] private FinishManager _finishManager;


    private float _initialFontSize = 80f;
    private float _finalFontSize = 150f;
    private float _fontSizeChangeDuration = 1.0f;

    // Component with the same usage of StartUpController created for the practice mode.
    private void Awake()
    {
        if (_slideController.enabled)
            _slideController.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(Welcome());
    }

    private IEnumerator Welcome()
    {
        Time.timeScale = 0;

        _welcomeText.text = "3";
        _welcomeObject.SetActive(true);
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        _welcomeText.text = "2";
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        _welcomeText.text = "1";
        yield return ScaleFontSizeOverTime(_initialFontSize, _finalFontSize, _fontSizeChangeDuration);

        _welcomeText.fontSize = (int)_finalFontSize;
        _welcomeText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

        _welcomeObject.SetActive(false);

        if (_finishManager != null)
            _finishManager.StartGame();

        Time.timeScale = 1;
        _slideController.enabled = true;
    }

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
