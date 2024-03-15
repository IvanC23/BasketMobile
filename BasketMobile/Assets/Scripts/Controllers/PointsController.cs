using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private Image _fillImage;
    [SerializeField] private GameObject _X2Indicator;
    [SerializeField] private GameObject _fireEffect;
    [SerializeField] private float _decreaseSpeed = 0.01f;

    private float _fillAmount = 0;
    private bool _isOnDoublePoints = false;

    private int _points = 0;
    public void AddPoints(int points)
    {
        ManageFillBar(points);

        if (_isOnDoublePoints)
        {
            points *= 2;
        }

        _points += points;
        _pointsText.text = _points.ToString();
    }

    void ManageFillBar(float points)
    {
        if (points == 3)
        {
            //Perfect shot
            if (!_isOnDoublePoints)
            {
                _fillAmount = _fillAmount + 0.3f;
                if (_fillAmount > 1)
                {
                    _fillAmount = 1;
                    _isOnDoublePoints = true;
                    _X2Indicator.SetActive(true);
                    _fireEffect.SetActive(true);
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
        else if (points > 0)
        {
            //Good shot
            if (!_isOnDoublePoints)
            {
                _fillAmount = _fillAmount + 0.2f;
                if (_fillAmount > 1)
                {
                    _fillAmount = 1;
                    _isOnDoublePoints = true;
                    _X2Indicator.SetActive(true);
                    _fireEffect.SetActive(true);
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
        else
        {
            //Wrong shot
            if (!_isOnDoublePoints)
            {
                _fillAmount = _fillAmount - 0.1f;
                if (_fillAmount < 0)
                {
                    _fillAmount = 0;
                }
                _fillImage.fillAmount = _fillAmount;
            }
            else
            {
                _fillAmount = 0;
                _isOnDoublePoints = false;
                _X2Indicator.SetActive(false);
                _fireEffect.SetActive(false);
                _fillImage.fillAmount = _fillAmount;
            }

        }
    }

    void Update()
    {
        if (!_isOnDoublePoints)
        {
            if (_fillAmount > 0)
            {
                _fillAmount = _fillAmount - _decreaseSpeed * Time.deltaTime;
                if (_fillAmount < 0)
                {
                    _fillAmount = 0;
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
        else
        {
            if (_fillAmount > 0)
            {
                _fillAmount = _fillAmount - _decreaseSpeed * Time.deltaTime * 3;
                if (_fillAmount < 0)
                {
                    _fillAmount = 0;
                    _isOnDoublePoints = false;
                    _X2Indicator.SetActive(false);
                    _fireEffect.SetActive(false);
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
    }
}
