using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinchToZoom : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private bool _isZooming;

    private float _currentScale;
    private float scaleRate = 2;
    private float _temp;

    public float minScale, maxScale;
    // Start is called before the first frame update
    private void Start()
    {
        _currentScale = transform.localScale.x;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isZooming)
        {
            if (Input.touchCount == 2)
            {
                transform.localScale = new Vector3(_currentScale, _currentScale);
                float distance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                if (_temp > distance){
                    if(_currentScale < minScale) return;
                    _currentScale -= Time.deltaTime * scaleRate;
                }
                else if (_temp < distance)
                {
                    if(_currentScale > maxScale) return;

                    _currentScale += Time.deltaTime * scaleRate;
                }

                _temp = distance;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount == 1)
        {
            _isZooming = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isZooming = false;
    }
}
