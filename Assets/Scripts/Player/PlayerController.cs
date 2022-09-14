using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float xMin, xMax, yMin, yMax;
    private Camera viewCamera;
    public float speed;

    public bool mobileRelease;
    Vector2 dir;

    private void Awake()
    {
        viewCamera = Camera.main;

        speed = speed / 1000;
    }

    private void Start()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        yMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;

    }
    private void Update()
    {
        if (GamePlayController.instance.state == GameState.PLAY)
        {
            Move();
            RestrictPlayerToScreen();
        }
    }

    private void Move()
    {
        if (mobileRelease)
        {
            MobileInput();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = viewCamera.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * 3000 * Time.deltaTime);
            }
        }

        Vector3 translation = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        translation *= speed * 2000 * Time.deltaTime;
        this.transform.Translate(translation);

    }

    private void MobileInput()
    {

        if (Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                dir = touch.deltaPosition;
                transform.Translate(dir * speed, Space.World);

                if (dir.x > 0)
                {
                    // transform.rotation = Quaternion.Euler(0, -50, 0);
                }
                else if (dir.x < 0)
                {
                    // transform.rotation = Quaternion.Euler(0, 50, 0);
                }
                else if (dir.x == 0)
                {
                    // transform.rotation = Quaternion.Euler(0, 0, 0);

                }
            }
        }
        // transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    private void RestrictPlayerToScreen()
    {
        Vector3 temp = this.transform.position;
        temp.x = Mathf.Clamp(temp.x, xMin , xMax );
        temp.y = Mathf.Clamp(temp.y, yMin , yMax );

        this.transform.position = temp;
    }

}
