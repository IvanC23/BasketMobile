using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private Transform _basketball;
    [SerializeField] private Transform _ringCenter;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _throwingDuration = 2f;
    [SerializeField] private float _throwingHeight = 1.2f;
    [SerializeField] private float _rotationForce = 500f;
    [SerializeField] private float _finalDeceleration = 0.3f;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PointsController _pointsController;
    [SerializeField] private Transform _badShotsWeak;
    [SerializeField] private Transform _badShotsStrong;
    [SerializeField] private Transform _niceShots;
    [SerializeField] private Transform _boardShots;

    private Rigidbody _basketballRigidbody;
    private BallController _ballController;
    private Vector3 _basketBallThrowPosition;
    private Vector3 _objectivePosition;
    private float _throwingTimer = 0f;
    private bool _ballThrown = false;
    private bool _weakShot = false;
    private float _height;
    private bool _finalDecelerationGoodShots = false;
    private int _numOfShotsWeak;
    private int _numOfShotsStrong;
    private int _numOfNiceShots;
    private int _numOfBoardShots;
    private int _shotScore = 0;

    // This component reposition the ball with respect to the camera and throws it
    // In the last phase, when the ball is being repositioned, the component compute the final score and
    // pass it to the PointsController, which will update the score on the UI and eventually enter the fireball mode.

    private void Start()
    {
        InitializeBallPositionAndRotation();
        InitializeNumberOfShotsPoints();
    }

    private void FixedUpdate()
    {
        // If ball is thrown, update its position
        if (_ballThrown)
            UpdateBallPosition();
    }

    // Update the position of the thrown ball
    private void UpdateBallPosition()
    {
        _throwingTimer += Time.fixedDeltaTime;
        float timePassed = _throwingTimer / _throwingDuration;

        // Calculate position based on interpolation to reach the highest position and then go down
        _basketball.position = Vector3.Lerp(_basketBallThrowPosition, _objectivePosition, timePassed) +
                                Vector3.up * _height * Mathf.Sin(timePassed * Mathf.PI);

        // After the throwing duration set, we disable the update of the ball and make its body decelerate
        // to give a more realistic feeling to the throw if it was a good one
        if (_throwingTimer >= _throwingDuration)
        {
            _ballThrown = false;
            if (_finalDecelerationGoodShots)
                _basketballRigidbody.velocity *= _finalDeceleration;

            if (_weakShot)
            {
                AudioManager.instance.PlayMusicByName("MissShot");
                _weakShot = false;
            }
        }
    }

    // Compute the type of throw of the ball, resetting the final point to reach based on the values passed from the slider
    public void ThrowBall(float differenceScore, float differenceBoard)
    {
        // The height value of the shot is computer based on the difference between the slider and the objective
        // to add randomness to the shots
        _height = _throwingHeight + 2.0f * differenceScore;

        // Reset the ball state
        _ballController.SetPerfectShot(false);
        _ballController.SetGoodShot(false);

        if (Mathf.Abs(differenceScore) < 0.04f)
        {
            // PERFECT SHOT

            _shotScore = 3;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _ballController.SetPerfectShot(true);
            _objectivePosition = _ringCenter.position;
        }
        else if (Mathf.Abs(differenceScore) < 0.1f)
        {
            // NICE SHOT

            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _niceShots.GetChild(Random.Range(0, _numOfNiceShots)).position;
        }
        else if (Mathf.Abs(differenceBoard) < 0.05f)
        {
            // BACKBOARD SHOT

            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _boardShots.GetChild(Random.Range(0, _numOfBoardShots)).position;
        }
        else if (differenceScore > 0)
        {
            // We distinguish between bad shots done with the slider of the user
            // passing above the objective slider for strong shots and below for weak shots, to give a different
            // visual feedback to the experience.

            // BAD SHOT STRONG
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsStrong.GetChild(Random.Range(0, _numOfShotsStrong)).position;
        }
        else
        {
            // BAD SHOT WEAK
            _weakShot = true;
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsWeak.GetChild(Random.Range(0, _numOfShotsWeak)).position;
        }

        _ballThrown = true;
        AudioManager.instance.PlayMusicByName("Throw");

        _basketballRigidbody.isKinematic = false;
        _basketballRigidbody.AddTorque(Vector3.right * _rotationForce);

        // After the ball is thrown, we pass the information to the camera to follow it accordingly
        _cameraController.ThrowBall();
    }

    // Reset ball position and pass the score to the PointsController
    public void ResetBall(Vector3 newBallPosition)
    {
        PassPointsToPointsController();

        _ballController.SetValidShot(false);
        _ballController.SetBoardPoints(0);
        _shotScore = 0;
        _throwingTimer = 0f;
        _basketballRigidbody.isKinematic = true;
        _basketball.position = newBallPosition;

        _basketball.LookAt(_cameraTransform);
        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);
        _basketball.rotation = new Quaternion(0, _basketball.rotation.y, _basketball.rotation.z, _basketball.rotation.w);
        _basketBallThrowPosition = _basketball.position;
    }

    // Initialize the ball position and rotation
    void InitializeBallPositionAndRotation()
    {
        _basketball.LookAt(_cameraTransform);
        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);
        _basketBallThrowPosition = _basketball.position;
        _basketballRigidbody = _basketball.GetComponent<Rigidbody>();
        _ballController = _basketball.GetComponent<BallController>();
        _objectivePosition = _ringCenter.position;
    }

    // We pass the points only if the shot is considered to be valid and check if the ball touched the backboard
    void PassPointsToPointsController()
    {
        if (_ballController.GetValidShot())
        {
            if (_ballController.GetBoardPoints() != 0)
                _pointsController.AddPoints(_ballController.GetBoardPoints());
            else
                _pointsController.AddPoints(_shotScore);
        }
        else
        {
            _pointsController.AddPoints(0);
        }
    }
    // We get the number of positions for the different types of shots
    void InitializeNumberOfShotsPoints()
    {
        _numOfShotsWeak = _badShotsWeak.childCount;
        _numOfShotsStrong = _badShotsStrong.childCount;
        _numOfNiceShots = _niceShots.childCount;
        _numOfBoardShots = _boardShots.childCount;
    }
}
