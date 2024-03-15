using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndChecker : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameObject _goodShotEffect;
    [SerializeField] private GameObject _perfectShotEffect;
    [SerializeField] private GameObject _textPoint;

    private TMP_Text _textPointText;

    private Color _perfectColor = new Color(0.02071772f, 0.6603774f, 0.0f);
    private Color _niceShotColor = Color.yellow;
    private Color _boardShotColor = new Color(0.4677148f, 0f, 0.6588235f);



    void Start()
    {
        _textPointText = _textPoint.GetComponent<TMP_Text>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (other.gameObject.GetComponent<BallController>().GetPerfectShot())
            {

                _textPointText.text = "PERFECT SHOT!\n+3";
                _textPointText.color = _perfectColor;

                _textPoint.SetActive(true);
                _perfectShotEffect.SetActive(true);
            }
            else
            {
                if (other.gameObject.GetComponent<BallController>().GetBoardPoints() != 0)
                {
                    _textPointText.text = "BOARD SHOT!\n+" + other.gameObject.GetComponent<BallController>().GetBoardPoints();
                    _textPointText.color = _boardShotColor;

                    _textPoint.SetActive(true);
                }
                else
                {
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
    IEnumerator DisableTextPointAfterAnimation()
    {
        // Get the Animation component attached to the _textPoint object
        Animator anim = _textPoint.GetComponent<Animator>();

        // Wait for the duration of the animation
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Disable the _textPoint object
        _textPoint.SetActive(false);
    }
}
