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
    private int _totalPoints = 0;

    // Component responsable for managing the points of the player, and the fireball feature, with the related bar.

    // Method to add points to the total and manage fill bar. This method will be called by BallThrower when the throw is completed
    // and the ball replaced.
    public void AddPoints(int points)
    {
        // Double points if on fireball mode.
        if (_isOnDoublePoints)
        {
            points *= 2;
        }

        // Manage fill bar used for the fireball mode.
        ManageFillBar(points);

        // Update total points displayed on the UI.
        _totalPoints += points;
        _pointsText.text = _totalPoints.ToString();
    }

    // Method to manage the increase and decrease of the fill bar based on the scored points and the time passed.
    void ManageFillBar(float points)
    {
        // Increase fill amount for perfect and good shots.
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
        // Decrease fill amount for wrong shots or disable the fireball mode if it was active.
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

    // In the update function, if the fill bar is not empty, decrease the fill amount based on the time passed.
    // If we are in fireball mode, decrease the fill amount 3 times faster.
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
