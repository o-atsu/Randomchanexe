using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

/*
攻撃入れ替え画面で表示する攻撃カードにアタッチするクラス
    SortAttacksによって生成, 初期化される
        Child ... 0: 選択時のエフェクト, 1: アイコン, 2: 攻撃名
    マウスの接触とクリックを検知し, AnimatorのTriggerに反映する
*/
public class AttackCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{

    private bool is_set;
    private int icon_id;
    private string txt;
    private CustomizeAttack custom;
    private Animator anim;


    public void init(string atk, string name, Sprite icon, int i, bool isset){
        anim = gameObject.GetComponent<Animator>();
        custom = transform.parent.parent.gameObject.GetComponent<CustomizeAttack>();

        Image img = transform.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI tmp = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();


        img.sprite = icon;
        tmp.text = name;
        txt = name;
        icon_id = i;
        is_set = isset;

        custom.SubscribeCard(this.gameObject, isset);
    }

    public int GetId(){ return icon_id; }

    public string GetText(){ return txt; }
    
    public Sprite GetIcon(){ return transform.GetChild(1).gameObject.GetComponent<Image>().sprite; }

    void OnDisable(){
        anim.SetBool("Highlight", false);
        anim.SetBool("Select", false);
    }

    public void OnPointerDown(PointerEventData eventData){
        anim.SetBool("Highlight", false);
        custom.SelectCard(icon_id, is_set);
    }

    public void OnPointerEnter(PointerEventData eventData){
        anim.SetBool("Highlight", true);
    }

    public void OnPointerExit(PointerEventData eventData){
        anim.SetBool("Highlight", false);
    }

    public void OnSelect(){
        anim.SetBool("Select", true);
    }
    public void OnDeselect(){
        anim.SetBool("Select", false);
    }

    public void OnChange(Sprite icon){
        Image img = transform.GetChild(1).gameObject.GetComponent<Image>();

        img.sprite = icon;
    }
}
