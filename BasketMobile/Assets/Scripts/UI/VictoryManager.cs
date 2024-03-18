using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private float _gameTime = 60f;
    [SerializeField] private TMP_Text _botScoreText;
    [SerializeField] private TMP_Text _botScoreFinishPageText;
    [SerializeField] private TMP_Text _playerScoreText;
    [SerializeField] private TMP_Text _playerScoreFinishText;
    [SerializeField] private Animator _botIconAnimator;
    [SerializeField] private Animator _userIconAnimator;
    [SerializeField] private SliderController _sliderController;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _lostText;
    [SerializeField] private GameObject _drawText;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _backgroundPanel;
    [SerializeField] private GameObject _finishPage;
    [SerializeField] private GameObject _botIcon;
    [SerializeField] private GameObject _userIcon;
    [SerializeField] private GameObject _rewardIcon;

    private RewardsManager _rewardsManager;
    private bool _gameOn = false;

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
        _botScoreFinishPageText.text = _botScoreText.text;
        _pauseButton.SetActive(false);
        _sliderController.ResetTouch();
        _sliderController.enabled = false;
        _backgroundPanel.SetActive(true);
        _finishPage.SetActive(true);
        _botIcon.SetActive(false);
        _userIcon.SetActive(false);


        if (int.Parse(_playerScoreText.text) > int.Parse(_botScoreText.text))
        {
            _userIconAnimator.enabled = true;
            _winText.SetActive(true);
            if (_rewardsManager != null)
            {
                _rewardsManager.AddReward();
            }
            _rewardIcon.SetActive(true);
        }
        else if (int.Parse(_playerScoreText.text) < int.Parse(_botScoreText.text))
        {
            _botIconAnimator.enabled = true;
            _lostText.SetActive(true);
        }
        else
        {
            _userIconAnimator.enabled = true;
            _botIconAnimator.enabled = true;
            _drawText.SetActive(true);
        }
    }

}
