using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersBehavior : MonoBehaviour
{
    [SerializeField] float speed;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] float playerRange;
    [SerializeField] float smoothTime;
    [SerializeField] float forwardSpeed;


    [SerializeField] GameObject gainPowerVFX;
    // [SerializeField] AudioClip gainPowerClip;

    void Update()
    {
       // transform.Translate(-Vector3.up * speed * Time.deltaTime);
                if (Vector3.Distance(Player.instance.transform.position, this.transform.position) > playerRange)
                {
                    transform.Translate(-Vector3.up * forwardSpeed * Time.deltaTime);
                }
                else
                {
                    Vector3 target = Player.instance.transform.position;

                    transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, Mathf.Infinity, Time.deltaTime);
                }
            }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            AudioController.Instance.PlayAudio(AudioType.PowerUp);
            if (gainPowerVFX != null)
            {
            GameObject vfx = Instantiate(gainPowerVFX, Player.instance.transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f);

            }
        }
    }


}
