using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour{

    [SerializeField]
    private MenuText[] menu_items;

    public void SelectText(int text_id){
        foreach(MenuText mtxt in menu_items){
            mtxt.Clicked(text_id);
        }
    }
}
