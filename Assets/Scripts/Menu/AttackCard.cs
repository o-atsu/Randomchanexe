using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class AttackCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{

    public static Dictionary<string, string> icon_file = new Dictionary<string, string>(){
        {"大剣","Sword"},
        {"パンチ","Punch"},
        {"ビーム","Beam"},
        {"Xビット","Xbit"},
        {"ファイアーブレード","Fireblade"}
    };

    private bool is_set;
    private int icon_id;
    private string txt;
    private CustomizeAttack custom;
    private Animator anim;


    public async void init(string atk, string name, int i, bool isset){
        anim = gameObject.GetComponent<Animator>();
        custom = transform.parent.parent.gameObject.GetComponent<CustomizeAttack>();

        Image img = transform.GetChild(0).gameObject.GetComponent<Image>();
        TextMeshProUGUI tmp = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(IconData.GetIconPath(icon_file[atk]));
        Sprite icon = await handle.Task;

        img.sprite = icon;
        tmp.text = txt = name;
        icon_id = i;
        is_set = isset;

        custom.SubscribeCard(this.gameObject, isset);
    }

    public int GetId(){ return icon_id; }

    public string GetText(){ return txt; }

    public void ToggleSet(){ is_set = !is_set; }

    public void OnPointerDown(PointerEventData eventData){
        // anim.SetBool("Highlight", false);
        custom.SelectCard(icon_id, is_set);
    }

    public void OnPointerEnter(PointerEventData eventData){
        // anim.SetBool("Highlight", true);
    }

    public void OnPointerExit(PointerEventData eventData){
        // anim.SetBool("Highlight", false);
    }

    public void OnSelect(){
        
    }
    public void OnDeselect(){
        
    }

    public async Task OnChange(string atk_name){
        Image img = transform.GetChild(0).gameObject.GetComponent<Image>();
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(IconData.GetIconPath(icon_file[atk_name]));
        Sprite icon = await handle.Task;

        img.sprite = icon;
    }
}
