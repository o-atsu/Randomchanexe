using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MenuText{

    [SerializeField]
    private GameObject Panel;


    protected override void OnSelect(){
        Panel.SetActive(true);
    }

    protected override void OnDeselect(){
        Panel.SetActive(false);
    }
}
