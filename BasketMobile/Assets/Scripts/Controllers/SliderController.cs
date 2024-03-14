using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _throwSlider;
    [SerializeField] private GameObject _fillArea;
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
        {
            HandleTouch();
        }

    }

    void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstTouchDone = true;
            _lastTouchPosition = Input.mousePosition;
            _fillArea.SetActive(true);
        }
        else if (Input.GetMouseButton(0) && _firstTouchDone)
        {
            Vector2 currentTouchPosition = Input.mousePosition;
            float deltaY = currentTouchPosition.y - _lastTouchPosition.y;

            deltaY = Mathf.Max(0, deltaY);

            float normalizedDeltaY = deltaY / Screen.height * _growSpeed;

            float sliderValue = _throwSlider.value + normalizedDeltaY;
            _throwSlider.value = Mathf.Clamp01(sliderValue);

            if (_throwSlider.value >= 0.1 && !_touchingScreen)
            {
                _touchingScreen = true;
            }

            if (_touchingScreen)
            {
                _timeSinceTouching += Time.deltaTime;
                if (_timeSinceTouching >= _timeLimit)
                {
                    _thrower.ThrowBall();
                    _touchEnabled = false;
                    _firstTouchDone = false;
                }
            }
            _lastTouchPosition = currentTouchPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_touchingScreen)
            {
                _thrower.ThrowBall();
                _touchEnabled = false;
                _firstTouchDone = false;
            }
            else
            {
                _touchingScreen = false;
                _timeSinceTouching = 0f;
                _throwSlider.value = 0;
                _fillArea.SetActive(false);
                _firstTouchDone = false;
            }
        }
    }



    public void ResetSlider()
    {
        _touchEnabled = true;
        _firstTouchDone = false;
        _touchingScreen = false;

        _timeSinceTouching = 0f;
        _throwSlider.value = 0;
        _fillArea.SetActive(false);
    }
}
