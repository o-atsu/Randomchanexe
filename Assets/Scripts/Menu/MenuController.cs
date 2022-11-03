using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour{

    [SerializeField]
    private MenuText[] menu_items;

    private int[] text_ids;
    private int current;

    void Awake(){
        current = 65535;
        text_ids = new int[menu_items.Length];
        for(int i = 0;i < menu_items.Length;i++){
            text_ids[i] = menu_items[i].GetId();
        }
    }

    private int IdToIndex(int i){ return System.Array.IndexOf(text_ids, i); }

    public void SelectText(int text_id){
        if(current != 65535){
            menu_items[IdToIndex(current)].OnDeselect();
        }
        menu_items[IdToIndex(text_id)].OnSelect();
        current = text_id;
    }
}
