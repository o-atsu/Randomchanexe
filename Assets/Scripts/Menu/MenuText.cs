using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

/*
メニューの選択テキストにアタッチするクラス
    インスタンスのIDを一意に決定
    選択, 非選択されたときの処理は継承先クラスで記述
    マウスの接触とクリックを検知し, AnimatorのTriggerに反映する
*/
public class MenuText : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{
    [SerializeField]
    private int text_id = 0;// 禁止：65535

    private Animator anim;
    private MenuController menu_controller;

    
    void Awake(){
        anim = gameObject.GetComponent<Animator>();
        Assert.IsFalse(anim == null, "Cannot Find Animator!");
        menu_controller = GameObject.FindGameObjectWithTag("Menu Controller").gameObject.GetComponent<MenuController>();
        Assert.IsFalse(menu_controller == null, "Cannot Find Menu Controller!");
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

    public int GetId(){ return text_id; }


    public virtual void OnSelect(){
        // Debug.Log("Selected: " + text_id);
        anim.SetBool("Clicked", true);
    }
    public virtual void OnDeselect(){
        anim.SetBool("Clicked", false);
    }
}
