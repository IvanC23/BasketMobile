using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private Image _fillImage;
    [SerializeField] private GameObject _X2Indicator;
    [SerializeField] private GameObject _fireEffect;
    [SerializeField] private GameObject _fireText;
    [SerializeField] private float _decreaseSpeed = 0.01f;

    private float _fillAmount = 0;
    private bool _isOnDoublePoints = false;

    private int _points = 0;

    // Method to add points to the total and manage fill bar
    public void AddPoints(int points)
    {
        // Double points if active
        if (_isOnDoublePoints)
        {
            points *= 2;
        }

        // Manage fill bar based on the points
        ManageFillBar(points);

        // Update total points and UI text
        _points += points;
        _pointsText.text = _points.ToString();
    }

    // Method to manage the fill bar based on the points earned
    void ManageFillBar(float points)
    {
        // Increase fill amount for perfect and good shots
        if (points == 3 || points > 0)
        {
            if (!_isOnDoublePoints)
            {
                _fillAmount = Mathf.Clamp01(_fillAmount + (points == 3 ? 0.3f : 0.2f));
                if (_fillAmount >= 1)
                {
                    _fillAmount = 1;
                    _isOnDoublePoints = true;
                    _X2Indicator.SetActive(true);
                    _fireEffect.SetActive(true);
                    _fireText.SetActive(true);
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
        // Decrease fill amount for wrong shots
        else
        {
            if (!_isOnDoublePoints)
            {
                _fillAmount = Mathf.Clamp01(_fillAmount - 0.1f);
                if (_fillAmount <= 0)
                {
                    _fillAmount = 0;
                }
                _fillImage.fillAmount = _fillAmount;
            }
            // Reset fill and indicators if double points were active
            else
            {
                _fillAmount = 0;
                _isOnDoublePoints = false;
                _X2Indicator.SetActive(false);
                _fireEffect.SetActive(false);
                _fireText.SetActive(false);
                _fillImage.fillAmount = _fillAmount;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease fill amount if double points are inactive
        if (!_isOnDoublePoints)
        {
            if (_fillAmount > 0)
            {
                _fillAmount = Mathf.Clamp01(_fillAmount - _decreaseSpeed * Time.deltaTime * 2);
                if (_fillAmount <= 0)
                {
                    _fillAmount = 0;
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
        // Decrease fill amount faster if double points are active
        else
        {
            if (_fillAmount > 0)
            {
                _fillAmount = Mathf.Clamp01(_fillAmount - _decreaseSpeed * Time.deltaTime * 6);
                if (_fillAmount <= 0)
                {
                    _fillAmount = 0;
                    _isOnDoublePoints = false;
                    _X2Indicator.SetActive(false);
                    _fireEffect.SetActive(false);
                    _fireText.SetActive(false);
                }
                _fillImage.fillAmount = _fillAmount;
            }
        }
    }
    public bool IsOnDoublePoints()
    {
        return _isOnDoublePoints;
    }
}
