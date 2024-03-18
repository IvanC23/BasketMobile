using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _ringPosition;
    [SerializeField] private Transform _ballPosition;
    [SerializeField] private BallThrower _ballThrower;
    [SerializeField] private SliderController _sliderController;
    [SerializeField] private float _goUpDuration = 1.0f;
    [SerializeField] private float _goRingDuration = 1.0f;
    [SerializeField] private float _waitingTime = 2f;
    [SerializeField] private Transform _positionsParent;
    private int _numberOfPositions;


    private Vector3 _cameraStartPosition;
    private Vector3 _cameraUpPosition;
    private Vector3 _cameraCloseRingPosition;

    private bool _throwingBall = false;

    private float _goUpTimer = 0f;
    private float _goRingTimer = 0f;
    private bool _coroutineStarted = false;

    // This component manages the camera of the player, positioning it in the various places preset in the scene
    // and moving it to the throwing position when the ball is thrown. It also resets the other components 
    // responsable for the throw and the slider, after the throw.

    private void Start()
    {
        _cameraCloseRingPosition = _ringPosition.position;
        _numberOfPositions = _positionsParent.childCount;
        ResetCamera();
    }

    private void FixedUpdate()
    {
        // Check if currently throwing ball and  eventually handle camera movement
        if (_throwingBall)
            StartCameraMovement();
    }

    // Approach the ring
    private void StartCameraMovement()
    {
        GoUp();
    }

    // Move the camera up to the throwing position
    private void GoUp()
    {
        _goUpTimer += Time.fixedDeltaTime;
        float timeGoingUp = _goUpTimer / _goUpDuration;

        // In the first half of the throw, camera goes up
        if (timeGoingUp <= 1)
            _cameraTransform.position = Vector3.Lerp(_cameraStartPosition, _cameraUpPosition, timeGoingUp);
        else
            ApproachRing();
    }

    // Move the camera closer to the ring
    private void ApproachRing()
    {
        _goRingTimer += Time.fixedDeltaTime;
        float timeGoingRing = _goRingTimer / _goRingDuration;

        // In the second half, approach the ring
        if (timeGoingRing <= 0.5f)
            _cameraTransform.position = Vector3.Lerp(_cameraUpPosition, _cameraCloseRingPosition, timeGoingRing);
        else
        {
            // If not already started, start coroutine to finish the throw after a predefined time
            if (!_coroutineStarted)
            {
                _coroutineStarted = true;
                StartCoroutine(FinishThrow());
            }
        }
    }

    // Reset camera and related objects after the throw
    private void ResetCamera()
    {
        // Reset timers and flags
        _goRingTimer = 0f;
        _goUpTimer = 0f;
        _throwingBall = false;
        _coroutineStarted = false;

        // Randomly set camera position from available positions
        _cameraTransform.position = _positionsParent.GetChild(Random.Range(0, _numberOfPositions)).position;
        _cameraStartPosition = _cameraTransform.position;
        _cameraUpPosition = _cameraTransform.position + Vector3.up * 1.5f;

        // Look at the ring
        _cameraTransform.LookAt(_ringPosition);
        _cameraTransform.rotation = Quaternion.Euler(0, _cameraTransform.rotation.eulerAngles.y, 0);

        // Reset ball position and slider
        _ballThrower.ResetBall(_cameraTransform.position - Vector3.up * 0.6f + _cameraTransform.forward * 1.3f);
        _sliderController.ResetSlider();

    }

    // Coroutine to wait for some time before resetting the camera
    private IEnumerator FinishThrow()
    {
        yield return new WaitForSeconds(_waitingTime);
        ResetCamera();
    }

    // Triggered by the BallThrower when the ball is thrown
    public void ThrowBall()
    {
        _throwingBall = true;
    }
}
