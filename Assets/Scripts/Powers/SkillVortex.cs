using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillVortex : MonoBehaviour
{
    public GameObject skill, button;
    float coolDownTime = 120f;
    float coolDownTimeer = 0.0f;
    [HideInInspector] public bool isCooldown = true;


    public IEnumerator ApplySkill1()
    {
        coolDownTimeer = coolDownTime;
        Image image = button.GetComponent<Image>();
        image.gameObject.SetActive(true);
        SoundEffectController.instance.SkillUIPress();

        Instantiate(skill, transform.position, Quaternion.identity);
        if (isCooldown)
        {
            isCooldown = false;
            while (coolDownTimeer > 0)
            {
                coolDownTimeer -= Time.deltaTime;

                image.fillAmount = coolDownTimeer / coolDownTime;
                yield return null;
            }

        }
        isCooldown = true;
        image.fillAmount = 100;
        image.gameObject.SetActive(false);

    }

    public void ApplySkill()
    {
        StartCoroutine(ApplySkill1());
    }
}
