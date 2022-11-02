using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class MenuText : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{
    [SerializeField]
    private int text_id = 0;

    private Animator anim;
    private MenuController menu_controller;

    
    void Awake(){
        anim = gameObject.GetComponent<Animator>();
        Assert.IsFalse(anim == null, "Cannot Find Animator!");
        menu_controller = GameObject.FindGameObjectWithTag("Menu Controller").gameObject.GetComponent<MenuController>();
        Assert.IsFalse(menu_controller == null, "Cannot Find Menu Controller!");
    }


    public void Clicked(int i){
        if(i == text_id){
            anim.SetBool("Clicked", true);
            OnSelect();
            return;
        }else{
            anim.SetBool("Clicked", false);
            OnDeselect();
            return;
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        // Debug.Log("Down:Menu");
        anim.SetBool("Highlight", false);
        menu_controller.SelectText(text_id);
    }

    public void OnPointerEnter(PointerEventData eventData){
        // Debug.Log("Enter:Menu");
        anim.SetBool("Highlight", true);
    }

    public void OnPointerExit(PointerEventData eventData){
        // Debug.Log("Exit:Menu");
        anim.SetBool("Highlight", false);
    }


    protected virtual void OnSelect(){
        Debug.Log("Selected: " + text_id);
    }
    protected virtual void OnDeselect(){
    }
}