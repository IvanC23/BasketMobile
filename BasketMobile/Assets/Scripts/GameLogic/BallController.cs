using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private bool _isBotBall = false;
    private bool _goodShot = false;
    private bool _perfectShot = false;
    private int _boardPoints = 0;
    private bool _getValidShot = false;

    // The ball propagates the information regarding the shot to the backboard
    // since we already know if it is a good shot or not when we throw the ball, the backboard will add
    // points to the ball only if it is a good shot, which will be used
    // by the component which will be checked by the ThrowController to understand if, following a good shot,
    // the ball also touched the backboard.

    // This reasoning was made to take into account multiple balls in case of different users
    // playing in the same session, to bind the possible extra points given by the backboard only to the ball
    // that actually touched it.


    // GoodShot tell us if the ball thrown will have a good trajectory, based on the difference on the slider
    // with respect to the objective.

    public void SetGoodShot(bool goodShot)
    {
        _goodShot = goodShot;
    }

    public bool GetGoodShot()
    {
        return _goodShot;
    }

    // Board points give us the points eventually added to the ball by a collision with the backboard

    public void SetBoardPoints(int points)
    {
        _boardPoints = points;
    }

    public int GetBoardPoints()
    {
        return _boardPoints;
    }

    // PerfectShot tells us if the ball thrown will have a perfect trajectory, based on the difference on the slider

    public void SetPerfectShot(bool perfectShot)
    {
        _perfectShot = perfectShot;
    }

    public bool GetPerfectShot()
    {
        return _perfectShot;
    }

    // Valid shot is checked at the end of the throw, and tell us if the ball actually collided with a point placed inside the
    // ring, effectively scoring a point. This function was created for the challenge mode, in which also a theoretically good shot
    // could be invalid if the ball throwed by the bot bumps into the player's ball, with the ball going out of the ring.

    public void SetValidShot(bool isValid)
    {
        _getValidShot = isValid;
    }

    public bool GetValidShot()
    {
        return _getValidShot;
    }

    // This function is used by some of the components in the scene to distinguish the bot's ball from the player's ball.
    // For example, the EndChecker will start an effect after a valid point only if the ball with which it interacted was thrown
    // by the player.
    public bool IsBotBall()
    {
        return _isBotBall;
    }
}
