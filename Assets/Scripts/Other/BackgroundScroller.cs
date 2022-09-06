using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Vector2 offset;
    Vector3 startingPosition;


    public float speed;

    private void Awake()
    {
        // GamePlayController.OnGameStateChange += OnGameStateChangeHandler;
        startingPosition = transform.position;
    }
    public Vector3 pos;
    private void OnDestroy()
    {
       // GamePlayController.OnGameStateChange -= OnGameStateChangeHandler;
    }
    void Start()
    {
        
        offset = new Vector2(0, 0);
    }

    void Update()
    {
       // offset = new Vector2(0, backgroundScrollSpeed);

        // myMaterial.mainTextureOffset += offset * Time.deltaTime;
        transform.Translate(Vector3.down * backgroundScrollSpeed * Time.deltaTime);

        if(transform.position.y < -27)
        {
            transform.transform.position = startingPosition;
        }
    }

    //public void OnGameStateChangeHandler(GameState state)
    //{
    //    if (state == GameState.INIT)
    //    {
    //        StartCoroutine(IntroScrollingRoutine());
    //    }
    //}

    IEnumerator IntroScrollingRoutine()
    {
        float time = 3;
        yield return new WaitForSeconds(.5f);
        while (time > 0)
        {

            speed -= Time.deltaTime * 6f;
            offset = new Vector2(0, speed);
            time -= Time.deltaTime;
            yield return null;
        }
        offset = new Vector2(0, backgroundScrollSpeed);

    }
}
