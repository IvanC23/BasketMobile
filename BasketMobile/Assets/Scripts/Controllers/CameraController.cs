using System.Collections;
using System.Collections.Generic;
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


    private Vector3 _cameraStartPosition;
    private Vector3 _cameraUpPosition;
    private Vector3 _cameraCloseRingPosition;

    private bool _throwingBall = false;

    private float _goUpTimer = 0f;
    private float _goRingTimer = 0f;
    private bool _coroutineStarted = false;



    void Start()
    {
        _cameraStartPosition = _cameraTransform.position;
        _cameraUpPosition = _cameraTransform.position + Vector3.up * 1.5f;
        _cameraCloseRingPosition = _ringPosition.position;
    }

    void FixedUpdate()
    {
        if (_throwingBall)
        {
            ApproachRing();
        }
    }

    public void ThrowBall()
    {
        _throwingBall = true;
    }

    private void ApproachRing()
    {
        GoUp();
    }

    private void GoUp()
    {
        _goUpTimer += Time.fixedDeltaTime;
        float _timeGoingUp = _goUpTimer / _goUpDuration;

        //In the first half of the throw, we go up with the camera reaching the same height of the ball
        if (_timeGoingUp <= 1)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraStartPosition, _cameraUpPosition, _timeGoingUp);
        }
        else
        {
            GoClose();
        }
    }

    private void GoClose()
    {
        //In the second half, we approach the ring, using a timer which is 2 times the duration of the second half of the throw
        //This way, we can use this timer to approach the final point of the throw until half of the way
        _goRingTimer += Time.fixedDeltaTime;
        float _timeGoingRing = _goRingTimer / _goRingDuration;
        if (_timeGoingRing <= 0.5f)
        {
            _cameraTransform.position = Vector3.Lerp(_cameraUpPosition, _cameraCloseRingPosition, _timeGoingRing);
        }
        else
        {
            if (!_coroutineStarted)
            {
                _coroutineStarted = true;
                StartCoroutine(FinishThrow());
            }
        }
    }

    private void SetUpCamera()
    {
        _goRingTimer = 0f;
        _goUpTimer = 0f;
        _throwingBall = false;

        _cameraTransform.position = _cameraStartPosition;

        _ballThrower.ResetBall(_cameraTransform.position - Vector3.up * 0.6f + _cameraTransform.forward * 1.3f);

        _sliderController.ResetSlider();
        _coroutineStarted = false;
    }

    private IEnumerator FinishThrow()
    {
        yield return new WaitForSeconds(_waitingTime);
        SetUpCamera();
    }
}
