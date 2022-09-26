using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Highlighted", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Highlighted", false);
    }
}
