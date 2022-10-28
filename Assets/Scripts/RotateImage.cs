using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector2 gemeObjectPosition = (Vector2)Camera.main.ScreenToViewportPoint(transform.position);
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        transform.rotation = Quaternion.Euler(0f, 0f, AngleBetweenTwoPoints(gemeObjectPosition, mousePosition) + 90);
    }

    float AngleBetweenTwoPoints(Vector3 gameObject, Vector3 target)
    {
        Vector3 difference = (gameObject - target);
        difference.Normalize();
        return Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    }
}
