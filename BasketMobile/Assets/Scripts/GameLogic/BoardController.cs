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

    private float _timerBonusActivation;
    private float _timerBonusDeactivation = 8.0f;
    private float _timerHitDeactivation = 1.0f;

    private bool _normalHighlightActive = false;
    private bool _hitHighlightActive = false;
    private bool _goodShotEntering = false;

    private void Start()
    {
        _timerBonusActivation = UnityEngine.Random.Range(5, 15);
    }

    void Update()
    {
        // Handle the activation and deactivation of normal highlight
        if (!_normalHighlightActive)
        {
            _timerBonusActivation -= Time.deltaTime;
            if (_timerBonusActivation <= 0)
            {
                // Reset the timer and set up the bonus text, highlight, and backboard text
                _timerBonusActivation = UnityEngine.Random.Range(5, 15);
                _bonusText.text = "+" + _possibleBonuses[UnityEngine.Random.Range(0, _possibleBonuses.Count)].ToString();
                _bonusTextObject.SetActive(true);
                _normalHighlight.SetActive(true);
                _backboardText.SetActive(true);
                _normalHighlightActive = true;
            }
        }
        else
        {
            // Handle the deactivation of normal highlight
            _timerBonusDeactivation -= Time.deltaTime;
            if (_timerBonusDeactivation <= 0)
            {
                // Reset the timer and deactivate bonus related objects
                _timerBonusActivation = UnityEngine.Random.Range(5, 15);
                _timerBonusDeactivation = 8.0f;
                _bonusTextObject.SetActive(false);
                _normalHighlight.SetActive(false);
                _backboardText.SetActive(false);
                _hitHighlight.SetActive(false);
                _normalHighlightActive = false;
            }
        }

        // Handle the deactivation of hit highlight
        if (_hitHighlightActive)
        {
            _timerHitDeactivation -= Time.deltaTime;
            if (_timerHitDeactivation <= 0)
            {
                // Reset the timer and deactivate hit highlight
                if (_goodShotEntering)
                {
                    _timerBonusActivation = UnityEngine.Random.Range(5, 15);
                    _timerBonusDeactivation = 8.0f;
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
        }
    }

    // Handle collision with the board
    void OnCollisionEnter(Collision collision)
    {
        if (_normalHighlightActive)
        {
            if (collision.gameObject.tag == "Ball")
            {
                _hitHighlight.SetActive(true);
                _hitHighlightActive = true;

                // Check if it's a good shot and update points
                if (collision.gameObject.transform.GetComponent<BallController>().GetGoodShot() && !_goodShotEntering)
                {
                    collision.gameObject.transform.GetComponent<BallController>().SetBoardPoints(int.Parse(_bonusText.text.Substring(1)));
                    _goodShotEntering = true;
                }
            }
        }
    }
}
