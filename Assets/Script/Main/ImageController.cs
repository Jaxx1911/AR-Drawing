using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isDragging = false;
    private Vector3 offset;

    private void Update()
    {
        Debug.Log("chạm đi");
        // Kiểm tra sự kiện cảm ứng
        if (Input.touchCount == 1)
        {
            Debug.LogError("chạm đi");
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Lấy vị trí cảm ứng khi chạm vào
                    offset = transform.position - Camera.main.ScreenToWorldPoint(touch.position);
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    // Di chuyển vật thể theo vị trí cảm ứng
                    if (isDragging)
                    {
                        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                        transform.position = new Vector3(touchPosition.x + offset.x, touchPosition.y + offset.y, transform.position.z);
                    }
                    break;

                case TouchPhase.Ended:
                    // Kết thúc kéo
                    isDragging = false;
                    break;
            }
        }
    }
}
