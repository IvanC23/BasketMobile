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
    [SerializeField] private PointsControllerBot _pointsController;
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

    // In this script we position and throw the ball
    // In the last phase, when the ball is repositioned, we compute the final score
    // also based on eventual bonus points given by the backboard and pass them to the PointsController

    private void Start()
    {
        // Initialize variables and positions
        _basketball.LookAt(_botTransform);
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
        _numberOfPositions = _positionsParent.childCount;

        StartCoroutine(ResetBall(_botTransform.position - Vector3.up * 0.6f + _botTransform.forward * 1.3f));
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
    public void ThrowBall(int typeOfThrow)
    {
        _height = _throwingHeight + 2.0f * UnityEngine.Random.Range(0.01f, 0.7f);
        _ballController.SetPerfectShot(false);
        _ballController.SetGoodShot(false);
        if (typeOfThrow == 0)
        {
            // Perfect shot
            //Debug.Log("Perfect shot");
            _shotScore = 3;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _ballController.SetPerfectShot(true);
            _objectivePosition = _ringCenter.position;
        }
        else if (typeOfThrow == 1)
        {
            // Nice shot
            //Debug.Log("Nice shot");
            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _niceShots.GetChild(Random.Range(0, _numOfNiceShots)).position;
        }
        else if (typeOfThrow == 2)
        {
            // Board shot
            //Debug.Log("Board shot");
            _shotScore = 2;
            _finalDecelerationGoodShots = true;
            _ballController.SetGoodShot(true);
            _objectivePosition = _boardShots.GetChild(Random.Range(0, _numOfBoardShots)).position;
        }
        else if (typeOfThrow == 3)
        {
            // Bad shot strong
            //Debug.Log("Bad shot strong");
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsStrong.GetChild(Random.Range(0, _numOfShotsStrong)).position;
        }
        else
        {
            // Bad shot weak
            //Debug.Log("Bad shot weak");
            _finalDecelerationGoodShots = false;
            _objectivePosition = _badShotsWeak.GetChild(Random.Range(0, _numOfShotsWeak)).position;
        }

        _ballThrown = true;
        // Trigger camera movement associated with ball throw
        _basketballRigidbody.isKinematic = false;
        _basketballRigidbody.AddTorque(Vector3.right * _rotationForce);

        StartCoroutine(ArtificialWaitAndReposition());

        IEnumerator ArtificialWaitAndReposition()
        {
            yield return new WaitForSeconds(_artificialWaitDuration);

            _botTransform.position = _positionsParent.GetChild(Random.Range(0, _numberOfPositions)).position;
            _botTransform.LookAt(_ringCenter);
            _botTransform.rotation = Quaternion.Euler(0, _botTransform.rotation.eulerAngles.y, 0);

            StartCoroutine(ResetBall(_botTransform.position - Vector3.up * 0.6f + _botTransform.forward * 1.3f));
        }
    }

    // Reset ball position and pass the score to the PointsController
    private IEnumerator ResetBall(Vector3 newBallPosition)
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

        _botAnimation.SetTrigger("Jump");

        yield return new WaitForSeconds(_jumpAnimation.length / 2f);

        ThrowBall(GetWeightedRandomIndex(_weightsForTypeOfThrow));

    }

    // Utility function to get a weighted random index for decideing which type of throw to perform
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

}
