using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [SerializeField] private float _gameTime = 60f;
    [SerializeField] private TMP_Text _playerScoreText;
    [SerializeField] private TMP_Text _playerScoreFinishText;
    [SerializeField] private SliderController _sliderController;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private GameObject _userInfoBox;

    private RewardsManager _rewardsManager;
    private bool _gameOn = false;

    // Component created to manage the end of the practice game, when the time is over it disable some of the elements of the game interface,
    // enabling a finish panel created for the practice mode, and showing the score of the player.

    void Start()
    {
        _rewardsManager = FindObjectOfType<RewardsManager>();
    }
    public void StartGame()
    {
        _gameOn = true;
    }

    void Update()
    {
        if (_gameOn)
        {
            _gameTime -= Time.deltaTime;
            if (_gameTime <= 0)
            {
                _gameOn = false;
                FinishGame();
            }
        }
    }

    void FinishGame()
    {
        Time.timeScale = 0;
        _playerScoreFinishText.text = _playerScoreText.text;
        _pauseButton.SetActive(false);
        _sliderController.ResetTouch();
        _sliderController.enabled = false;
        _backgroundPanel.SetActive(true);
        _finishPanel.SetActive(true);
        _userInfoBox.SetActive(false);
        if (_rewardsManager != null)
        {
            _rewardsManager.AddReward();
        }

    }

}
