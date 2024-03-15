using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool _goodShot = false;
    private int _boardPoints = 0;

    public void SetGoodShot(bool goodShot)
    {
        _goodShot = goodShot;
    }

    public bool GetGoodShot()
    {
        return _goodShot;
    }

    public void SetBoardPoints(int points)
    {
        _boardPoints = points;
    }

    public int GetBoardPoints()
    {
        return _boardPoints;
    }
}
