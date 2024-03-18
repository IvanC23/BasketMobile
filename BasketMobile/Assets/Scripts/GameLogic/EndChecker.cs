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

    // Component attached to a collider placed in the basket, to check if the ball enters the basket and give points to the player.
    // It also activates the effects and the text that appears when the player makes a good shot.

    // Colors for different shot types
    private Color _perfectColor = new Color(0.02071772f, 0.6603774f, 0.0f); // Perfect shot color
    private Color _niceShotColor = Color.yellow; // Nice shot color
    private Color _boardShotColor = new Color(0.4677148f, 0f, 0.6588235f); // Board shot color

    private void Start()
    {
        _textPointText = _textPoint.GetComponent<TMP_Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is a ball
        if (other.gameObject.tag == "Ball")
        {
            // Here we set the ball as a valid shot to propagate the information when the ball will be
            // checked in the final phase before adding the points to the player score.
            // If the ball didn't trigger this collider, it means that it didn't enter the basket, so it's not a valid shot.

            other.gameObject.GetComponent<BallController>().SetValidShot(true);


            // If the ball is not a bot ball, we can show the effects to the player, distinguishing based on the type of shot and
            // eventually the double points status if the player is in fireball mode.

            if (!other.gameObject.GetComponent<BallController>().IsBotBall())
            {
                // PERFECT SHOT
                if (other.gameObject.GetComponent<BallController>().GetPerfectShot())
                {
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
                    if (other.gameObject.GetComponent<BallController>().GetBoardPoints() != 0)
                    {
                        // BOARD SHOT
                        if (_pointsController.IsOnDoublePoints())
                            _textPointText.text = "BOARD SHOT!\n+" + other.gameObject.GetComponent<BallController>().GetBoardPoints() * 2;
                        else
                            _textPointText.text = "BOARD SHOT!\n+" + other.gameObject.GetComponent<BallController>().GetBoardPoints();

                        _textPointText.color = _boardShotColor;
                        _textPoint.SetActive(true);
                    }
                    else
                    {
                        // NICE SHOT
                        if (_pointsController.IsOnDoublePoints())
                            _textPointText.text = "NICE SHOT!\n+4";
                        else
                            _textPointText.text = "NICE SHOT!\n+2";
                        _textPointText.color = _niceShotColor;
                        _textPoint.SetActive(true);
                    }
                    _goodShotEffect.SetActive(true);
                }
                StartCoroutine(DisableTextPointAfterAnimation());
            }

        }
    }

    // Coroutine used to disable the effect object after its animation is finished
    private IEnumerator DisableTextPointAfterAnimation()
    {
        Animator anim = _textPoint.GetComponent<Animator>();

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        _textPoint.SetActive(false);
    }
}
