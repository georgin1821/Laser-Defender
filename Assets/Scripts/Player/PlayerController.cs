using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float xMin, xMax, yMin, yMax;
    private Vector3 translation;
    private Camera viewCamera;

    private void Awake()
    {
        viewCamera = Camera.main;
    }

    public void KeyboardMovement(Vector3 _translation)
    {
        translation = _translation;
        this.transform.Translate(translation);

        RestrictPlayerToScreen();
    }
    public void MobileMovement(float speed)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 point = viewCamera.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));

            // Vector3 point = viewCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
            transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);

            RestrictPlayerToScreen();
        }

    }

    public void MoveTowards(float speed)
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
            RestrictPlayerToScreen();

        }

    }
    private void RestrictPlayerToScreen()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        yMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

        float shipSizeXAdjustment = gameObject.GetComponent<Renderer>().bounds.size.x / 4;
        float shipSizeYAdjustment = gameObject.GetComponent<Renderer>().bounds.size.y / 2;

        Vector3 temp = this.transform.position;
        temp.x = Mathf.Clamp(temp.x, xMin + shipSizeXAdjustment, xMax - shipSizeXAdjustment);
        temp.y = Mathf.Clamp(temp.y, yMin + shipSizeYAdjustment, yMax - shipSizeYAdjustment);
        this.transform.position = temp;

    }




}
