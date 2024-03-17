using System.Collections;
using TMPro;
using UnityEngine;

public class EndChecker : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PointsController _pointsController;
    [SerializeField] private GameObject _goodShotEffect;
    [SerializeField] private GameObject _perfectShotEffect;
    [SerializeField] private GameObject _textPoint;

    private TMP_Text _textPointText;

    // Colors for different shot types
    private Color _perfectColor = new Color(0.02071772f, 0.6603774f, 0.0f); // Perfect shot color
    private Color _niceShotColor = Color.yellow; // Nice shot color
    private Color _boardShotColor = new Color(0.4677148f, 0f, 0.6588235f); // Board shot color

    private void Start()
    {
        // Get the TMP_Text component of the _textPoint object
        _textPointText = _textPoint.GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a ball
        if (other.gameObject.tag == "Ball")
        {
            // Check if it's a perfect shot
            if (other.gameObject.GetComponent<BallController>().GetPerfectShot())
            {
                // Set text, color, and activate objects for a perfect shot
                if (_pointsController.IsOnDoublePoints())
                    _textPointText.text = "PERFECT SHOT!\n+6";
                else
                    _textPointText.text = "PERFECT SHOT!\n+3";

                _textPointText.color = _perfectColor;
                _textPoint.SetActive(true);
                _perfectShotEffect.SetActive(true);
            }
            else
            {
                // If not perfect, check if it's a board shot or a nice shot
                if (other.gameObject.GetComponent<BallController>().GetBoardPoints() != 0)
                {
                    // Set text, color, and activate objects for a board shot
                    if (_pointsController.IsOnDoublePoints())
                        _textPointText.text = "BOARD SHOT!\n+" + other.gameObject.GetComponent<BallController>().GetBoardPoints() * 2;
                    else
                        _textPointText.text = "BOARD SHOT!\n+" + other.gameObject.GetComponent<BallController>().GetBoardPoints();

                    _textPointText.color = _boardShotColor;
                    _textPoint.SetActive(true);
                }
                else
                {
                    // Set text, color, and activate objects for a nice shot
                    if (_pointsController.IsOnDoublePoints())
                        _textPointText.text = "NICE SHOT!\n+4";
                    else
                        _textPointText.text = "NICE SHOT!\n+2";
                    _textPointText.color = _niceShotColor;
                    _textPoint.SetActive(true);
                }
                _goodShotEffect.SetActive(true);
            }

            // Start the coroutine to disable the _textPoint object after the animation is finished
            StartCoroutine(DisableTextPointAfterAnimation());
        }
    }

    // Coroutine to disable the _textPoint object after the animation
    private IEnumerator DisableTextPointAfterAnimation()
    {
        // Get the Animator component attached to the _textPoint object
        Animator anim = _textPoint.GetComponent<Animator>();

        // Wait for the duration of the animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Disable the _textPoint object
        _textPoint.SetActive(false);
    }
}
