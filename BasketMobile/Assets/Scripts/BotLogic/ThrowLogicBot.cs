using System.Collections;
using UnityEngine;

public class ThrowLogicBot : MonoBehaviour
{
    [SerializeField] private Transform _basketball;
    [SerializeField] private Transform _ringCenter;
    [SerializeField] private Transform _botTransform;
    [SerializeField] private float _throwingDuration = 2f;
    [SerializeField] private float _artificialWaitDuration = 2f;
    [SerializeField] private float _throwingHeight = 1.2f;
    [SerializeField] private float _rotationForce = 500f;
    [SerializeField] private float _finalDeceleration = 0.3f;
    [SerializeField] private Transform _badShotsWeak;
    [SerializeField] private Transform _badShotsStrong;
    [SerializeField] private Transform _niceShots;
    [SerializeField] private Transform _boardShots;
    [SerializeField] private PointsControllerBot _pointsControllerBot;
    [SerializeField] private Animator _botAnimation;
    [SerializeField] private AnimationClip _jumpAnimation;
    [SerializeField] private Transform _positionsParent;
    [SerializeField] private float[] _weightsForTypeOfThrow = new float[5] { 0.15f, 0.25f, 0.2f, 0.2f, 0.2f };


    private Rigidbody _basketballRigidbody;
    private BallController _ballController;
    private Vector3 _basketBallThrowPosition;
    private Vector3 _objectivePosition;
    private float _throwingTimer = 0f;
    private bool _ballThrown = false;
    private float _height;
    private bool _finalDecelerationGoodShots = false;
    private int _numOfShotsWeak;
    private int _numOfShotsStrong;
    private int _numOfNiceShots;
    private int _numOfBoardShots;
    private int _numberOfPositions;
    private int _shotScore = 0;

    // This component reposition the ball with respect to the bot position and throws it
    // It follows the same logic as the BallThrower, but it is used by the bot, so it emulates the results
    // of the sliderController and artificially waits before throwing again without moving a camera.

    private void Start()
    {
        InitializeBasketballAndBot();

        InitializeNumberOfShotsAndBotPoints();

        StartCoroutine(ResetBall(_botTransform.position - Vector3.up * 0.6f + _botTransform.forward * 1.3f));
    }

    private void FixedUpdate()
    {
        // The throw follows the same logic as the BallThrower and uses the same final points
        if (_ballThrown)
            UpdateBallPosition();
    }

    private void UpdateBallPosition()
    {
        _throwingTimer += Time.fixedDeltaTime;
        float timePassed = _throwingTimer / _throwingDuration;

        _basketball.position = Vector3.Lerp(_basketBallThrowPosition, _objectivePosition, timePassed) +
                                Vector3.up * _height * Mathf.Sin(timePassed * Mathf.PI);

        if (_throwingTimer >= _throwingDuration)
        {
            _ballThrown = false;
            if (_finalDecelerationGoodShots)
                _basketballRigidbody.velocity *= _finalDeceleration;
        }
    }

    // Trigger the ball throw following the same logic as the BallThrower
    public void ThrowBall(int typeOfThrow)
    {
        _height = _throwingHeight + 2.0f * UnityEngine.Random.Range(0.01f, 0.7f);
        _ballController.SetPerfectShot(false);
        _ballController.SetGoodShot(false);

        if (typeOfThrow == 0)
        {
            // PERFECT SHOT

            _shotScore = 3;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _ballController.SetPerfectShot(true);
            _objectivePosition = _ringCenter.position;
        }
        else if (typeOfThrow == 1)
        {
            // NICE SHOT

            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _niceShots.GetChild(Random.Range(0, _numOfNiceShots)).position;
        }
        else if (typeOfThrow == 2)
        {
            // BACKBOARD SHOT

            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _boardShots.GetChild(Random.Range(0, _numOfBoardShots)).position;
        }
        else if (typeOfThrow == 3)
        {
            // BAD SHOT STRONG
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsStrong.GetChild(Random.Range(0, _numOfShotsStrong)).position;
        }
        else
        {
            // BAD SHOT WEAK

            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsWeak.GetChild(Random.Range(0, _numOfShotsWeak)).position;
        }

        _ballThrown = true;
        _basketballRigidbody.isKinematic = false;
        _basketballRigidbody.AddTorque(Vector3.right * _rotationForce);

        StartCoroutine(ArtificialWaitAndRepositionBot());
    }

    private IEnumerator ArtificialWaitAndRepositionBot()
    {
        yield return new WaitForSeconds(_artificialWaitDuration);

        // We reposition the bot in one of the available positions and then reset the ball in order to throw it again

        _botTransform.position = _positionsParent.GetChild(Random.Range(0, _numberOfPositions)).position;
        _botTransform.LookAt(_ringCenter);
        _botTransform.rotation = Quaternion.Euler(0, _botTransform.rotation.eulerAngles.y, 0);

        StartCoroutine(ResetBall(_botTransform.position - Vector3.up * 0.6f + _botTransform.forward * 1.3f));
    }

    // Reset ball position and pass the score to the PointsController
    private IEnumerator ResetBall(Vector3 newBallPosition)
    {
        PassPointsToPointsController();

        _ballController.SetValidShot(false);
        _ballController.SetBoardPoints(0);
        _shotScore = 0;
        _throwingTimer = 0f;

        _basketballRigidbody.isKinematic = true;
        _basketball.position = newBallPosition;
        _basketball.LookAt(_botTransform);
        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);
        _basketball.rotation = new Quaternion(0, _basketball.rotation.y, _basketball.rotation.z, _basketball.rotation.w);
        _basketBallThrowPosition = _basketball.position;

        // Before executing a throw, we activate a jump animation for the bot, to give a visual feedback to the user

        _botAnimation.SetTrigger("Jump");

        yield return new WaitForSeconds(_jumpAnimation.length / 2f);

        AudioManager.instance.PlayMusicByName("Throw");

        ThrowBall(GetWeightedRandomIndex(_weightsForTypeOfThrow));

    }

    void PassPointsToPointsController()
    {
        if (_ballController.GetValidShot())
        {
            if (_ballController.GetBoardPoints() != 0)
                _pointsControllerBot.AddPoints(_ballController.GetBoardPoints());
            else
                _pointsControllerBot.AddPoints(_shotScore);
        }
        else
        {
            _pointsControllerBot.AddPoints(0);
        }
    }

    // Utility function to get a weighted random index for deciding which type of throw to perform
    // thanks to this we can give different weights to the different types of throws, making them more or less likely to happen
    // and adjusting the difficulty of the bot
    int GetWeightedRandomIndex(float[] weights)
    {
        float totalWeight = 0f;
        foreach (float weight in weights)
        {
            totalWeight += weight;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return i;
            }
        }

        return weights.Length - 1;
    }

    // We get the number of positions for the different types of shots and for the bot
    void InitializeNumberOfShotsAndBotPoints()
    {
        _numOfShotsWeak = _badShotsWeak.childCount;
        _numOfShotsStrong = _badShotsStrong.childCount;
        _numOfNiceShots = _niceShots.childCount;
        _numOfBoardShots = _boardShots.childCount;
        _numberOfPositions = _positionsParent.childCount;
    }

    void InitializeBasketballAndBot()
    {
        _basketball.LookAt(_botTransform);
        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);
        _basketBallThrowPosition = _basketball.position;
        _basketballRigidbody = _basketball.GetComponent<Rigidbody>();
        _ballController = _basketball.GetComponent<BallController>();
        _objectivePosition = _ringCenter.position;
    }

}
