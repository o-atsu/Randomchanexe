using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MenuText{

    [SerializeField]
    private GameObject Panel;


    public override void OnSelect(){
        base.OnSelect();
        Panel.SetActive(true);
    }

    public override void OnDeselect(){
        base.OnDeselect();
        Panel.SetActive(false);
    }
}
