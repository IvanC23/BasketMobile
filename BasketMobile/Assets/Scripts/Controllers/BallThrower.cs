using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private Transform _basketball;
    [SerializeField] private Transform _ringCenter;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _throwingDuration = 2f;
    [SerializeField] private float _throwingHeight = 1.2f;
    [SerializeField] private CameraController _cameraController;

    private Rigidbody _basketballRigidbody;
    private Vector3 _basketBallThrowPosition;
    private float _throwingTimer = 0f;
    private bool _ballThrowed = false;
    void Start()
    {
        _basketBallThrowPosition = _basketball.position;
        _basketballRigidbody = _basketball.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (_ballThrowed)
        {
            _throwingTimer += Time.fixedDeltaTime;
            float timePassed = _throwingTimer / _throwingDuration;

            _basketball.position = Vector3.Lerp(_basketBallThrowPosition, _ringCenter.position, timePassed) + Vector3.up * _throwingHeight * Mathf.Sin(timePassed * Mathf.PI);

            if (_throwingTimer >= 0.5)
            {
                _basketballRigidbody.isKinematic = false;
            }

            if (_throwingTimer >= _throwingDuration)
            {
                _ballThrowed = false;
                //_basketballRigidbody.AddForce(Vector3.down,ForceMode.Impulse);
                //StartCoroutine(WaitAfterApex());
            }
        }
    }

    public void ResetBall(Vector3 newBallPosition)
    {
        _throwingTimer = 0f;
        _basketballRigidbody.isKinematic = true;
        _basketball.position = newBallPosition;
    }

    public void ThrowBall()
    {
        _ballThrowed = true;
        _cameraController.ThrowBall();
    }
}
