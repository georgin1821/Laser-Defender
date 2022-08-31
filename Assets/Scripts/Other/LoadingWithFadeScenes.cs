using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingWithFadeScenes : Singleton<LoadingWithFadeScenes>
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject fadePanel;

    string sceneName;

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
        SceneManager.LoadScene(sceneName);

    }

    public void OnAnimFadeInComplete()
    {
        fadePanel.SetActive(false);
    }
     public void setSceneName(string name)
    {
        sceneName = name;
    }
}
