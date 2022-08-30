using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FadeScenes : Singleton<FadeScenes>
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject fadePanel;

    override protected void Awake()
    {
        fadePanel.SetActive(true);
        base.Awake();
        FadeAnimator.OnEventFadeInComplete += OnAnimFadeInComplete;
        FadeAnimator.OnEventFadeOutComplete += OnAnimFadeOutComplete;
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();
        FadeAnimator.OnEventFadeInComplete -= OnAnimFadeInComplete;
        FadeAnimator.OnEventFadeOutComplete -= OnAnimFadeOutComplete;
    }
    public void FadeOut()
    {
        fadePanel.SetActive(true);
        anim.SetTrigger("FadeOut");
    }

    public void OnAnimFadeOutComplete()
    {
        Debug.Log("LevelSelect");
        //fadePanel.SetActive(false);
        SceneManager.LoadScene("LevelSelect");

    }

    public void OnAnimFadeInComplete()
    {
        fadePanel.SetActive(false);
    }

}
