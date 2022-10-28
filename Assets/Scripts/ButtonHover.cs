using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Animator shieldAnimator1;
    public Animator shieldAnimator2;
    public Animator shieldAnimator3;
    public Animator shieldAnimator4;

    Image shieldRenderer1;
    Image shieldRenderer2;
    Image shieldRenderer3;
    Image shieldRenderer4;

    Sprite shield1;
    Sprite shield2;
    Sprite shield3;
    Sprite shield4;

    private void Awake()
    {
        shieldRenderer1 = shieldAnimator1.GetComponent<Image>();
        shieldRenderer2 = shieldAnimator2.GetComponent<Image>();
        shieldRenderer3 = shieldAnimator3.GetComponent<Image>();
        shieldRenderer4 = shieldAnimator4.GetComponent<Image>();

        shield1 = shieldRenderer1.sprite;
        shield2 = shieldRenderer2.sprite;
        shield3 = shieldRenderer3.sprite;
        shield4 = shieldRenderer4.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shieldAnimator1.enabled = true;
        shieldAnimator2.enabled = true;
        shieldAnimator3.enabled = true;
        shieldAnimator4.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        shieldAnimator1.enabled = false;
        shieldAnimator2.enabled = false;
        shieldAnimator3.enabled = false;
        shieldAnimator4.enabled = false;

        shieldRenderer1.sprite = shield1;
        shieldRenderer2.sprite = shield2;
        shieldRenderer3.sprite = shield3;
        shieldRenderer4.sprite = shield4;
    }
}
