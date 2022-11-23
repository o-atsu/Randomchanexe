using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
MenuTextの選択, 非選択とパネルの表示, 非表示を同期させるクラス
*/
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
