using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private bool _isBotBall = false;
    private bool _goodShot = false;
    private bool _perfectShot = false;
    private int _boardPoints = 0;
    private bool _getValidShot = false;

    // The ball propagates the information regarding the shot to the backboard
    // since we already know if it is a good shot or not tha backboard will add
    // points to the ball only if it is a good shot, then in the last phase,
    // when the final points will be added to the final score, the throwcontroller
    // will check if some points were added to the ball and eventually add them to the final score

    // This reasoning was made to take into account multiple balls in case of different users
    // playing in the same session

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

    public void SetPerfectShot(bool perfectShot)
    {
        _perfectShot = perfectShot;
    }

    public bool GetPerfectShot()
    {
        return _perfectShot;
    }

    public void SetValidShot(bool isValid)
    {
        _getValidShot = isValid;
    }

    public bool GetValidShot()
    {
        return _getValidShot;
    }

    public bool IsBotBall()
    {
        return _isBotBall;
    }
}
