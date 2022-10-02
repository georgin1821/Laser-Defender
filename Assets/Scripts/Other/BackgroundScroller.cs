using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;

    [SerializeField] float introSpeed;

    bool isStatring;

    public Vector3 pos;

    private void Awake()
    {
        GamePlayController.OnScrollingBGEnabled += OnScrollingBGEnabledHandler;
    }

    private void OnDestroy()
    {
        GamePlayController.OnScrollingBGEnabled -= OnScrollingBGEnabledHandler;
    }

    private void OnScrollingBGEnabledHandler()
    {
        IntroScrollingBG();
    }

    void Update()
    {

        if (transform.position.y < -14.66)
        {
            transform.transform.position = new Vector3(transform.position.x, 14.66f, transform.position.z);
        }

        if (isStatring) return;
        transform.Translate(Vector3.down * backgroundScrollSpeed * Time.deltaTime);
    }

     void IntroScrollingBG()
    {
        StopAllCoroutines();
        StartCoroutine(IntroScrollingRoutine());
    }
    IEnumerator IntroScrollingRoutine()
    {
        isStatring = true;
        float time = 3;
        while (time > 0)
        {

            introSpeed -= Time.deltaTime * 7f;
            transform.Translate(Vector3.down * introSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        isStatring = false;
    }
}
