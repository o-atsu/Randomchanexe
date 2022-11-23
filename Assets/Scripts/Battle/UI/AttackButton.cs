using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using Battle;

/*
攻撃ボタンにアタッチするクラス
    SortButtonsによって生成, 初期化される. 
    クリック時にFPlayerのAttackEventを呼び出し
        Child ... 0: 選択時のエフェクト, 1: アイコン, 2: 攻撃名
*/
public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{

    private int atk_index;
    private string txt;
    private Animator anim;
    private FPlayer fplayer;
    private GameObject selected;


    public void init(string atk, string name, Sprite icon, int i){
        anim = gameObject.GetComponent<Animator>();
        fplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<FPlayer>();
        Debug.Log(GameObject.FindGameObjectWithTag("Player"));
        Assert.IsFalse(anim == null, "Cannot Find Animator!");
        Assert.IsFalse(fplayer == null, "Cannot Find Player Tag!");

        selected = transform.GetChild(0).gameObject;
        Image img = transform.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI tmp = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        img.sprite = icon;
        tmp.text = name;
        txt = name;
        atk_index = i;

    }

    public string GetText(){ return txt; }

    void OnDisable(){
        anim.SetBool("Highlight", false);
    }

    public void OnPointerDown(PointerEventData eventData){
        anim.SetBool("Highlight", false);
        Task atk = fplayer.AttackEvent(atk_index);
    }

    public void OnPointerEnter(PointerEventData eventData){
        anim.SetBool("Highlight", true);
    }

    public void OnPointerExit(PointerEventData eventData){
        anim.SetBool("Highlight", false);
    }

}
