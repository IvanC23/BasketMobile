using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider _throwSlider;
    [SerializeField] private GameObject _fillArea;
    private Vector2 _lastTouchPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 currentTouchPosition = Input.mousePosition;
            float deltaY = currentTouchPosition.y - _lastTouchPosition.y;
            float normalizedDeltaY = deltaY / Screen.height; // Normalize deltaY based on screen height

            if (normalizedDeltaY != 0)
            {
                float sliderValue = _throwSlider.value + normalizedDeltaY;
                _throwSlider.value = Mathf.Clamp01(sliderValue); // Clamp the slider value between 0 and 1
            }

            _lastTouchPosition = currentTouchPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _fillArea.SetActive(false);
        }
    }
}
