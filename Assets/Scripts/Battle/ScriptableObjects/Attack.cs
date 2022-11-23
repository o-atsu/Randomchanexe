using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
攻撃の情報を保持したScriptable Object
    初回ロード時にすべて読み込まれる
*/
[CreateAssetMenu(menuName = "ScriptableObject/Create Attack")]
public class Attack : ScriptableObject{
    [SerializeField]
    private string display_name;// 表示名, 禁止：「,」
    [SerializeField]
    private Sprite icon;// 表示アイコン
    [SerializeField]
    private int damage;// ダメージ
    [SerializeField]
    private List<rangeclass> range;// 攻撃範囲
    [SerializeField]
    private int startup;// 予備動作時間 (milliseconds)
    [SerializeField]
    private int recovery;// 後隙の時間 (milliseconds)

   	public int GetDamage(){ return damage;}

    public List<int[]> GetRange(){
        List<int[]> ret = new List<int[]>();
        for(int i = 0;i < range.Count;i++){
            ret.Add(new int[2]{range[i].coords[0], range[i].coords[1]});
        }
        return ret;
    }

    public string GetName(){ return display_name; }

    public int GetStartup(){ return startup; }

    public int GetRecovery(){ return recovery; }
    
    public Sprite GetIcon(){ return icon; }


    public override string ToString(){
        string r = "";
        for(int i = 0;i < range.Count;i++){
            r += "(" + range[i].coords[0] + ", " + range[i].coords[1] + ")";
        }
        return "Damage: " + damage + ", range: " + r + ", startup: " + startup + ", recovery: " + recovery;
    }

    // for inspector
    [System.SerializableAttribute]
    public class rangeclass{
        public int[] coords = {0, 0};
    }
}
