using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    Material myMaterial;
    Vector2 offset;

    public float speed;

    private void Awake()
    {
        GamePlayController.OnGameStateChange += OnGameStateChangeHandler;
    }

    private void OnDestroy()
    {
        GamePlayController.OnGameStateChange -= OnGameStateChangeHandler;
    }
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        
        offset = new Vector2(0, 0);
    }

    void Update()
    {
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }

    public void OnGameStateChangeHandler(GameState state)
    {
        if (state == GameState.INIT)
        {
            StartCoroutine(IntroScrollingRoutine());
        }
    }

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
