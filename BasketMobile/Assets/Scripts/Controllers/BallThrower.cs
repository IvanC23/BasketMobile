using System;
using System.Collections;
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

    private Rigidbody _basketballRigidbody;
    private BallController _ballController;
    private Vector3 _basketBallThrowPosition;
    private Vector3 _objectivePosition;
    private float _throwingTimer = 0f;
    private bool _ballThrown = false;
    private float _height;

    private bool _finalDecelerationGoodShots = false;

    [SerializeField] private Transform _badShotsWeak;
    private int _numOfShotsWeak;
    [SerializeField] private Transform _badShotsStrong;
    private int _numOfShotsStrong;
    [SerializeField] private Transform _niceShots;
    private int _numOfNiceShots;
    [SerializeField] private Transform _boardShots;
    private int _numOfBoardShots;


    private int _shotScore = 0;



    void Start()
    {
        // Initialize the throw position and get the Player's Ball components
        _basketball.LookAt(_cameraTransform);

        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);

        _basketBallThrowPosition = _basketball.position;
        _basketballRigidbody = _basketball.GetComponent<Rigidbody>();
        _ballController = _basketball.GetComponent<BallController>();

        //SettingUp positions for the shots
        _objectivePosition = _ringCenter.position;
        _numOfShotsWeak = _badShotsWeak.childCount;
        _numOfShotsStrong = _badShotsStrong.childCount;
        _numOfNiceShots = _niceShots.childCount;
        _numOfBoardShots = _boardShots.childCount;
    }

    void FixedUpdate()
    {
        // If ball is thrown, update its position
        if (_ballThrown)
            UpdateBallPosition();
    }

    // Update the position of the thrown ball
    void UpdateBallPosition()
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
            {
                _basketballRigidbody.velocity *= _finalDeceleration;
            }
            // Uncomment the lines below to apply additional force after reaching the apex
            // _basketballRigidbody.AddForce(Vector3.down, ForceMode.Impulse);
        }
    }

    // Trigger the ball throw
    public void ThrowBall(float differenceScore, float differenceBoard)
    {
        _height = _throwingHeight + 2.0f * differenceScore;

        if (Mathf.Abs(differenceScore) < 0.05f)
        {
            //Perfect shot

            _shotScore = 3;

            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);

            _objectivePosition = _ringCenter.position;
            Debug.Log("Perfect shot");
        }
        else if (Mathf.Abs(differenceScore) < 0.1f)
        {
            //Nice shot

            _shotScore = 2;

            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);

            _objectivePosition = _niceShots.GetChild(UnityEngine.Random.Range(0, _numOfNiceShots)).position;
            Debug.Log("Nice shot");
        }
        else if (Mathf.Abs(differenceBoard) < 0.04f)
        {
            //Board shot

            _shotScore = 2;

            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);

            _objectivePosition = _boardShots.GetChild(UnityEngine.Random.Range(0, _numOfBoardShots)).position;
            Debug.Log("Board shot");
        }
        else if (differenceScore > 0)
        {
            //Bad shot strong

            _finalDecelerationGoodShots = false;
            _ballController.SetGoodShot(false);

            _objectivePosition = _badShotsStrong.GetChild(UnityEngine.Random.Range(0, _numOfShotsStrong)).position;
            Debug.Log("Bad shot strong");
        }
        else
        {
            //Bad shot weak

            _finalDecelerationGoodShots = false;
            _ballController.SetGoodShot(false);

            _objectivePosition = _badShotsWeak.GetChild(UnityEngine.Random.Range(0, _numOfShotsWeak)).position;
            Debug.Log("Bad shot weak");
        }

        _ballThrown = true;
        // Trigger camera movement associated with ball throw
        _cameraController.ThrowBall();
        _basketballRigidbody.isKinematic = false;
        _basketballRigidbody.AddTorque(Vector3.right * _rotationForce);
    }

    // Reset ball position
    public void ResetBall(Vector3 newBallPosition)
    {
        if (_ballController.GetBoardPoints() != 0)
        {
            _pointsController.AddPoints(_ballController.GetBoardPoints());
        }
        else
        {
            _pointsController.AddPoints(_shotScore);
        }

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
