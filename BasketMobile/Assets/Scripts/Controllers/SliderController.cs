using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _throwSlider;
    [SerializeField] private Slider _scoreSlider;
    [SerializeField] private Slider _boardSlider;
    [SerializeField] private Slider _arrowSlider;
    [SerializeField] private GameObject _fillArea;
    [SerializeField] private GameObject _fillAreaArrow;
    [SerializeField] private float _growSpeed = 1.2f;
    [SerializeField] private float _timeLimit = 2f;
    [SerializeField] private BallThrower _thrower;

    private Vector2 _lastTouchPosition;
    private bool _touchingScreen = false;
    private float _timeSinceTouching = 0f;
    private bool _touchEnabled = true;
    private bool _firstTouchDone = false;


    void Update()
    {
        if (_touchEnabled)
            HandleTouch();
    }

    void HandleTouch()
    {
        // Check for initial touch
        if (Input.GetMouseButtonDown(0))
        {
            _firstTouchDone = true;
            _lastTouchPosition = Input.mousePosition;
            _fillArea.SetActive(true);
            _fillAreaArrow.SetActive(true);
        }
        // Handle ongoing touch
        else if (Input.GetMouseButton(0) && _firstTouchDone)
        {
            Vector2 currentTouchPosition = Input.mousePosition;
            float deltaY = currentTouchPosition.y - _lastTouchPosition.y;
            deltaY = Mathf.Max(0, deltaY);
            float normalizedDeltaY = deltaY / Screen.height * _growSpeed;
            float sliderValue = _throwSlider.value + normalizedDeltaY;
            _throwSlider.value = Mathf.Clamp01(sliderValue);
            _arrowSlider.value = _throwSlider.value;

            if (_throwSlider.value >= 0.1 && !_touchingScreen)
                _touchingScreen = true;

            if (_touchingScreen)
            {
                _timeSinceTouching += Time.deltaTime;
                if (_timeSinceTouching >= _timeLimit)
                {
                    LaunchBall();
                }
            }
            _lastTouchPosition = currentTouchPosition;
        }
        // Handle touch release
        else if (Input.GetMouseButtonUp(0))
        {
            if (_touchingScreen)
                LaunchBall();
            else
                ResetTouch();
        }
    }

    void LaunchBall()
    {
        float differenceScore = _throwSlider.value - _scoreSlider.value;
        float differenceBoard = _throwSlider.value - _boardSlider.value;
        Debug.Log("Difference: " + differenceScore);

        _thrower.ThrowBall(differenceScore, differenceBoard);
        _touchEnabled = false;
        _firstTouchDone = false;
    }

    void ResetTouch()
    {
        _touchingScreen = false;
        _timeSinceTouching = 0f;
        _throwSlider.value = 0;
        _arrowSlider.value = 0;
        _fillArea.SetActive(false);
        _fillAreaArrow.SetActive(false);
        _firstTouchDone = false;
    }

    // Public method to reset the slider
    public void ResetSlider()
    {
        _touchEnabled = true;
        _firstTouchDone = false;
        _touchingScreen = false;
        _timeSinceTouching = 0f;
        _throwSlider.value = 0;
        _arrowSlider.value = 0;
        _scoreSlider.value = UnityEngine.Random.Range(0.2f, 0.7f);
        _boardSlider.value = _scoreSlider.value + 0.2f;
        _fillArea.SetActive(false);
        _fillAreaArrow.SetActive(false);
    }
}
