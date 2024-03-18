using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _throwSlider;
    [SerializeField] private Slider _scoreSlider;
    [SerializeField] private Slider _boardSlider;
    [SerializeField] private Slider _arrowSlider;
    [SerializeField] private GameObject _fillArea;
    [SerializeField] private GameObject _fillAreaArrow;
    [SerializeField] private float _growSpeed = 1.2f;
    [SerializeField] private float _timeLimit = 2f;
    [SerializeField] private BallThrower _thrower;

    private Vector2 _lastTouchPosition;
    private bool _touchingScreen = false;
    private float _timeSinceTouching = 0f;
    private bool _touchEnabled = true;
    private bool _firstTouchDone = false;

    // Component which handles the touch input and the different sliders.
    // When the gesture is complete or the time limit for it is reached, it
    // passess the difference between the sliders to the BallThrower to compute the kind of throw.

    // The arrow has been placed on another slider, called _arrowSlider, 
    // following the same value as the throw slider. This was done to let the arrow appear above the 
    // text indicating the perfect score and the backboard score, as in the original
    // game, positioning its game component at a lower level in the canvas hierarchy.

    void Update()
    {
        // Continuously check for touch input if touch is enabled inside the script
        if (_touchEnabled)
            HandleTouch();
    }

    void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInitialTouch();
        }
        else if (Input.GetMouseButton(0) && _firstTouchDone)
        {
            HandleOnGoingTouch();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            HandleTouchRelease();
        }
    }

    void HandleInitialTouch()
    {
        _firstTouchDone = true;
        _lastTouchPosition = Input.mousePosition;
        _fillArea.SetActive(true);
        _fillAreaArrow.SetActive(true);
    }

    void HandleOnGoingTouch()
    {
        Vector2 currentTouchPosition = Input.mousePosition;
        float deltaY = Mathf.Max(0, currentTouchPosition.y - _lastTouchPosition.y);
        float normalizedDeltaY = deltaY / Screen.height * _growSpeed;

        float sliderValue = _throwSlider.value + normalizedDeltaY;
        _throwSlider.value = Mathf.Clamp01(sliderValue);
        _arrowSlider.value = _throwSlider.value;

        // Check if user gesture has surpassed a certain threshold
        if (_throwSlider.value >= 0.1 && !_touchingScreen)
            _touchingScreen = true;

        // If screen is being touched for a long time, launch the ball
        if (_touchingScreen)
        {
            _timeSinceTouching += Time.deltaTime;
            if (_timeSinceTouching >= _timeLimit)
                ComputeDifferences();
        }

        _lastTouchPosition = currentTouchPosition;
    }

    void HandleTouchRelease()
    {
        // Launch the ball if the user already went beyond the initial threshold with the gesture
        if (_touchingScreen)
            ComputeDifferences();
        // Reset touch otherwise
        else
            ResetTouch();
    }

    // Pass the differences to the thrower to actually throw the ball
    void ComputeDifferences()
    {
        float differenceScore = _throwSlider.value - _scoreSlider.value;
        float differenceBoard = _throwSlider.value - _boardSlider.value;

        _thrower.ThrowBall(differenceScore, differenceBoard);

        // Disable touch until slider is reset by the other components after the throw
        _touchEnabled = false;
        _firstTouchDone = false;
    }

    // Reset touch variables and slider values, without computing new objective values
    public void ResetTouch()
    {
        _touchingScreen = false;
        _timeSinceTouching = 0f;
        _throwSlider.value = 0;
        _arrowSlider.value = 0;
        _fillArea.SetActive(false);
        _fillAreaArrow.SetActive(false);
        _firstTouchDone = false;
    }

    // Public method to reset the slider after the throw is completed. Used by the camera after its animation
    public void ResetSlider()
    {
        // Enable touch and reset variables
        _touchEnabled = true;
        _firstTouchDone = false;
        _touchingScreen = false;
        _timeSinceTouching = 0f;
        _throwSlider.value = 0;
        _arrowSlider.value = 0;

        // Set random values for score and board sliders. The perfect score values is placed between the 20% and 70% of the slider
        _scoreSlider.value = Random.Range(0.2f, 0.7f);
        _boardSlider.value = _scoreSlider.value + 0.2f;

        // Deactivate fill area objects. 
        _fillArea.SetActive(false);
        _fillAreaArrow.SetActive(false);
    }
}
