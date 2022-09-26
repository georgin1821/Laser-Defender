using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsBehavior : MonoBehaviour
{
    [SerializeField] float forwardSpeed;
    [SerializeField] float playerRange;
    [SerializeField] float smoothTime;

    private Vector3 velocity = Vector3.zero;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        {
            if (Vector3.Distance(Player.instance.transform.position, this.transform.position) > playerRange)
            {
                transform.Translate(-Vector3.up * forwardSpeed * Time.deltaTime);
            }
            else
            {
                anim.SetTrigger("idleCoin");
                Vector3 target = Player.instance.transform.position;

                transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            AudioController.Instance.PlayAudio(AudioType.CollectGold);
            int currentCoins = GameDataManager.Instance.coins;
            GameDataManager.Instance.coins += 100;
            GamePlayController.instance.levelCoins += 100;
            GameUIController.instance.UpdateCoins(currentCoins, GameDataManager.Instance.coins);
        }
    }

}
