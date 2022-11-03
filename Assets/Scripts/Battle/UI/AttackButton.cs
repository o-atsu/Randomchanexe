using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using Battle;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{

    public static Dictionary<string, string> icon_file = new Dictionary<string, string>(){
        {"大剣","Sword"},
        {"パンチ","Punch"},
        {"ビーム","Beam"},
        {"Xビット","Xbit"},
        {"ファイアーブレード","Fireblade"}
    };

    private int atk_index;
    private string txt;
    private Animator anim;
    private FPlayer fplayer;


    public async void init(string atk, string name, int i){
        anim = gameObject.GetComponent<Animator>();
        fplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<FPlayer>();

        Image img = transform.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI tmp = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(IconData.GetIconPath(icon_file[atk]));
        Sprite icon = await handle.Task;

        img.sprite = icon;
        tmp.text = name;
        txt = name;
        atk_index = i;

    }

    public string GetText(){ return txt; }

    void OnDisable(){
        anim.SetBool("Highlight", false);
    }

    public async void OnPointerDown(PointerEventData eventData){
        anim.SetBool("Highlight", false);
        await fplayer.AttackEvent(atk_index);
    }

    public void OnPointerEnter(PointerEventData eventData){
        anim.SetBool("Highlight", true);
    }

    public void OnPointerExit(PointerEventData eventData){
        anim.SetBool("Highlight", false);
    }

}
