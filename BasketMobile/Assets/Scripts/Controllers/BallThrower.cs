using System.Collections;
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
    private bool _ballThrown = false;

    void Start()
    {
        // Initialize the throw position and get the Rigidbody component
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

        // Enable physics after a certain time
        if (_throwingTimer >= 0.5f)
            _basketballRigidbody.isKinematic = false;

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
    }

    // Trigger the ball throw
    public void ThrowBall()
    {
        _ballThrown = true;
        // Trigger camera movement associated with ball throw
        _cameraController.ThrowBall();
    }
}
