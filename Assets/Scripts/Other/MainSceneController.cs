using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public static MainSceneController instance;

    private void Awake()
    {
        Configure();
    }
    private void Configure()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
       // AudioController.Instance.PlayAudio(AudioType.Soundtrack_2);
    }

    
}

