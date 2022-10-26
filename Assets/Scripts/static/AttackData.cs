using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;


public class AttackData{
    private static Dictionary<string, Attack> Attacks = new Dictionary<string, Attack>();// 攻撃名でAttackクラスのScriptableObjectを検索

    static string attackpath = "Assets/ScriptableObjects/Attacks/";

    static AttackData(){
        string[] atks = System.IO.Directory.GetFiles(@attackpath, "*.asset");

        foreach(string i in atks){
            Addressables.LoadAssetAsync<Attack>(i).Completed += a =>{
                Attack atk = a.Result;
                Attacks.Add(atk.GetName(), atk);
            };
        }
    }

    // AttackのScriptableObjectにアクセス
    public static Attack NameToAttack(string name){
        // Debug.Log(Attacks.Count);
        if(!Attacks.ContainsKey(name)){
            Debug.Log("Attack: " + name + " is not found");
            return null;
        }// 指定したファイル名が存在しなければnullを返す

        return Attacks[name];
    }
}
