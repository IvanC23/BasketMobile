using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsController : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;
    private int _points = 0;
    public void AddPoints(int points)
    {
        _points += points;
        _pointsText.text = _points.ToString();
    }
}
