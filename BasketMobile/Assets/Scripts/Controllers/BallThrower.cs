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
    [SerializeField] private CameraController _cameraController;
    private Rigidbody _basketballRigidbody;
    private Vector3 _basketBallThrowPosition;
    private float _throwingTimer = 0f;
    private bool _ballThrown = false;

    void Start()
    {
        // Initialize the throw position and get the Rigidbody component
        _basketball.LookAt(_cameraTransform);

        Vector3 rotation = _basketball.rotation.eulerAngles;
        rotation.x = 0;
        _basketball.rotation = Quaternion.Euler(rotation);

        _basketBallThrowPosition = _basketball.position;
        _basketballRigidbody = _basketball.GetComponent<Rigidbody>();
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
        _basketball.position = Vector3.Lerp(_basketBallThrowPosition, _ringCenter.position, timePassed) +
                                Vector3.up * _throwingHeight * Mathf.Sin(timePassed * Mathf.PI);

        // Reset after throwing duration
        if (_throwingTimer >= _throwingDuration)
        {
            _ballThrown = false;
            // Uncomment the lines below to apply additional force after reaching the apex
            // _basketballRigidbody.AddForce(Vector3.down, ForceMode.Impulse);
            // StartCoroutine(WaitAfterApex());
        }
    }

    // Reset ball position
    public void ResetBall(Vector3 newBallPosition)
    {
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

    // Trigger the ball throw
    public void ThrowBall()
    {
        _ballThrown = true;
        // Trigger camera movement associated with ball throw
        _cameraController.ThrowBall();
        _basketballRigidbody.isKinematic = false;
        _basketballRigidbody.AddTorque(Vector3.right * _rotationForce);

    }
}
