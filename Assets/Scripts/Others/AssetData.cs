using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/*
使用するプレハブ名とAddressableのパスを対応させるクラス
    ゲームの初回ロード時に初期化
*/
public class AssetData : MonoBehaviour{
    [SerializeField]
    private string[] prefab_names;// プレハブ名一覧

    [SerializeField]
    private string path = "Assets/Prefabs/Adventure/";

    private Dictionary<string, string> NameToPath = new Dictionary<string, string>();// パス一覧

    void Awake(){
    DontDestroyOnLoad(gameObject);
    
        foreach(string i in prefab_names){
    	    NameToPath[i] = path + i + ".prefab";
        }
    }

    // パスにアクセス
    public string GetPath(string name){
        if(!NameToPath.ContainsKey(name)){ return null; }// 指定したファイル名が存在しなければnullを返す
        return NameToPath[name];
    }
}
