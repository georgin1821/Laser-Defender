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

        //if (GamePlayController.instance.state == GameState.PLAY)
        {
            //if near player range stops aniamtion and moves towards player
            if (Vector3.Distance(Player.instance.transform.position, this.transform.position) > playerRange)
            {
               // Debug.Log("Coins forward movement");
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
                //Debug.Log("Coins collides Player");
                Destroy(gameObject);
                SoundEffectController.instance.CollectGoldSound();

                int currentCoins = GameManager.Instance.coins;
                GameManager.Instance.coins += 100;
                GameUIController.instance.UpdateCoins(currentCoins, GameManager.Instance.coins);
            }
        }

    }
