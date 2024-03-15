using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUpController : MonoBehaviour
{
    [SerializeField] private GameObject _welcomeObject;
    [SerializeField] private TMP_Text _welcomeText;
    [SerializeField] private Animator _welcomeAnimator;
    [SerializeField] private SliderController _slideController;


    void Start()
    {
        StartCoroutine(Welcome());
    }

    private IEnumerator Welcome()
    {
        Time.timeScale = 0;
        _welcomeText.text = "3";
        _welcomeObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        _welcomeText.text = "2";
        yield return new WaitForSecondsRealtime(1.0f);
        _welcomeText.text = "1";
        yield return new WaitForSecondsRealtime(1.0f);
        _welcomeAnimator.enabled = false;
        _welcomeText.fontSize = 80;
        _welcomeText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);
        _welcomeObject.SetActive(false);
        Time.timeScale = 1;
        _slideController.enabled = true;

    }
}
