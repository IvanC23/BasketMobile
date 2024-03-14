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
    //private bool _ballThrowed = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastTouchPosition = Input.mousePosition;
            _fillArea.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
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
            _lastTouchPosition = currentTouchPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_touchingScreen)
            {
                _thrower.ThrowBall();
                this.enabled = false;
            }
            else
            {
                _touchingScreen = false;
                _timeSinceTouching = 0f;
                _throwSlider.value = 0;
                _fillArea.SetActive(false);
            }
        }

        if (_touchingScreen)
        {
            _timeSinceTouching += Time.deltaTime;
            if (_timeSinceTouching >= _timeLimit)
            {
                _thrower.ThrowBall();
                this.enabled = false;
            }
        }
    }



    public void ResetSlider()
    {
        _touchingScreen = false;
        _timeSinceTouching = 0f;
        _throwSlider.value = 0;
        _fillArea.SetActive(false);

        this.enabled = true;
    }
}
