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
    private float _height;
    private bool _finalDecelerationGoodShots = false;
    private int _numOfShotsWeak;
    private int _numOfShotsStrong;
    private int _numOfNiceShots;
    private int _numOfBoardShots;
    private int _shotScore = 0;

    // In this script we position and throw the ball
    // In the last phase, when the ball is repositioned, we compute the final score
    // also based on eventual bonus points given by the backboard and pass them to the PointsController

    private void Start()
    {
        // Initialize variables and positions
        _basketball.LookAt(_cameraTransform);
        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);
        _basketBallThrowPosition = _basketball.position;
        _basketballRigidbody = _basketball.GetComponent<Rigidbody>();
        _ballController = _basketball.GetComponent<BallController>();
        _objectivePosition = _ringCenter.position;

        // Get the number of positions for different shot types
        _numOfShotsWeak = _badShotsWeak.childCount;
        _numOfShotsStrong = _badShotsStrong.childCount;
        _numOfNiceShots = _niceShots.childCount;
        _numOfBoardShots = _boardShots.childCount;
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

        // Calculate position based on interpolation and sinusoidal motion
        _basketball.position = Vector3.Lerp(_basketBallThrowPosition, _objectivePosition, timePassed) +
                                Vector3.up * _height * Mathf.Sin(timePassed * Mathf.PI);

        // Reset after throwing duration
        if (_throwingTimer >= _throwingDuration)
        {
            _ballThrown = false;
            if (_finalDecelerationGoodShots)
                _basketballRigidbody.velocity *= _finalDeceleration;
        }
    }

    // Trigger the ball throw
    public void ThrowBall(float differenceScore, float differenceBoard)
    {
        _height = _throwingHeight + 2.0f * differenceScore;
        _ballController.SetPerfectShot(false);
        _ballController.SetGoodShot(false);

        if (Mathf.Abs(differenceScore) < 0.04f)
        {
            // Perfect shot
            Debug.Log("Perfect shot");
            _shotScore = 3;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _ballController.SetPerfectShot(true);
            _objectivePosition = _ringCenter.position;
        }
        else if (Mathf.Abs(differenceScore) < 0.1f)
        {
            // Nice shot
            Debug.Log("Nice shot");
            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _niceShots.GetChild(Random.Range(0, _numOfNiceShots)).position;
        }
        else if (Mathf.Abs(differenceBoard) < 0.05f)
        {
            // Board shot
            Debug.Log("Board shot");
            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _boardShots.GetChild(Random.Range(0, _numOfBoardShots)).position;
        }
        else if (differenceScore > 0)
        {
            // Bad shot strong
            Debug.Log("Bad shot strong");
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsStrong.GetChild(Random.Range(0, _numOfShotsStrong)).position;
        }
        else
        {
            // Bad shot weak
            Debug.Log("Bad shot weak");
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsWeak.GetChild(Random.Range(0, _numOfShotsWeak)).position;
        }

        _ballThrown = true;
        // Trigger camera movement associated with ball throw
        _cameraController.ThrowBall();
        _basketballRigidbody.isKinematic = false;
        _basketballRigidbody.AddTorque(Vector3.right * _rotationForce);
    }

    // Reset ball position and pass the score to the PointsController
    public void ResetBall(Vector3 newBallPosition)
    {
        if (_ballController.GetBoardPoints() != 0)
            _pointsController.AddPoints(_ballController.GetBoardPoints());
        else
            _pointsController.AddPoints(_shotScore);

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
}
