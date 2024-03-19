using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject _normalHighlight;
    [SerializeField] private GameObject _hitHighlight;
    [SerializeField] private GameObject _bonusTextObject;
    [SerializeField] private GameObject _backboardText;
    [SerializeField] private TMP_Text _bonusText;

    [SerializeField] private List<int> _possibleBonuses;

    private float _timerNormalHighlightOn;
    private float _timerNormalHighlightOff = 8.0f;
    private float _timerHitDeactivation = 1.0f;

    private bool _normalHighlightActive = false;
    private bool _hitHighlightActive = false;
    private bool _goodShotEntering = false;

    private void Start()
    {
        _timerNormalHighlightOn = UnityEngine.Random.Range(5, 15);
    }

    void Update()
    {
        // Handle the activation and deactivation of normal highlight
        if (!_normalHighlightActive)
        {
            _timerNormalHighlightOn -= Time.deltaTime;
            if (_timerNormalHighlightOn <= 0)
            {
                NormalHighlightOn();
            }
        }
        else
        {
            // Handle the deactivation of normal highlight
            _timerNormalHighlightOff -= Time.deltaTime;
            if (_timerNormalHighlightOff <= 0)
            {
                NormalHighlightOff();
            }
        }

        // Handle the deactivation of hit highlight, if it was already activated by the interaction with a ball
        if (_hitHighlightActive)
        {
            _timerHitDeactivation -= Time.deltaTime;
            if (_timerHitDeactivation <= 0)
            {
                DeactivateHitHighlight();
            }
        }
    }


    // Reset the timer and set up the bonus text, highlight, and backboard text
    void NormalHighlightOn()
    {
        _timerNormalHighlightOn = UnityEngine.Random.Range(5, 15);
        _bonusText.text = "+" + _possibleBonuses[UnityEngine.Random.Range(0, _possibleBonuses.Count)].ToString();
        _bonusTextObject.SetActive(true);
        _normalHighlight.SetActive(true);
        _backboardText.SetActive(true);
        _normalHighlightActive = true;
    }

    // Reset the timer and deactivate bonus related objects
    void NormalHighlightOff()
    {
        _timerNormalHighlightOn = UnityEngine.Random.Range(5, 15);
        _timerNormalHighlightOff = 8.0f;
        _bonusTextObject.SetActive(false);
        _normalHighlight.SetActive(false);
        _backboardText.SetActive(false);
        _hitHighlight.SetActive(false);
        _normalHighlightActive = false;
    }

    // Deactivate the hit highlight and reset the timer
    void DeactivateHitHighlight()
    {
        // Here we distinguish between a deactivation caused simply by the deactivation timer of the hit effect
        // or by a good shot entering the ring.
        if (_goodShotEntering)
        {
            _timerNormalHighlightOn = UnityEngine.Random.Range(5, 15);
            _timerNormalHighlightOff = 8.0f;
            _bonusTextObject.SetActive(false);
            _normalHighlight.SetActive(false);
            _backboardText.SetActive(false);
            _normalHighlightActive = false;
            _goodShotEntering = false;
        }
        _hitHighlight.SetActive(false);
        _hitHighlightActive = false;
        _timerHitDeactivation = 1.0f;
    }

    // Handle collision of the ball with the backboard
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            AudioManager.instance.PlayMusicByName("Backboard");

            if (_normalHighlightActive)
            {

                /// HitHighlight activation
                _hitHighlight.SetActive(true);
                _hitHighlightActive = true;

                // Check if it's a good shot, update points and notify the backboard that the ball is supposed to enter
                // so that when DeactivateHitHighlight is called, the backboard will know if the ball was entering the ring
                if (collision.gameObject.transform.GetComponent<BallController>().GetGoodShot() && !_goodShotEntering)
                {
                    collision.gameObject.transform.GetComponent<BallController>().SetBoardPoints(int.Parse(_bonusText.text.Substring(1)));
                    _goodShotEntering = true;
                }
            }
        }
    }
}
