using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


/*
Attackクラスのアセット名とObjectを対応させるクラス
    ゲームの初回ロード時に初期化, 全Scriptable Objectを読み込み
*/
public class AttackData : MonoBehaviour{
    
    [SerializeField]
    private string[] asset_names;
    
    [SerializeField]
    private string attackpath = "Assets/ScriptableObjects/Attacks/";

    private static Dictionary<string, Attack> Attacks;// 攻撃名でAttackクラスのScriptableObjectを検索


    // 初期化, 全Scriptable Object読み込み
    public async Task Refresh(){
        DontDestroyOnLoad(gameObject);
        
        Attacks = new Dictionary<string, Attack>();
        foreach(string i in asset_names){
            string i_key = attackpath + i + ".asset";
            AsyncOperationHandle<Attack> handle = Addressables.LoadAssetAsync<Attack>(i_key);
            Attack atk = await handle.Task;
            Attacks.Add(atk.GetName(), atk);
            // Debug.Log(atk.GetName() + atk);
        }
    }

    // AttackのScriptableObjectにアクセス
    public Attack NameToAttack(string name){
        // Debug.Log(Attacks.Count);
        if(!Attacks.ContainsKey(name)){
            Debug.Log("Attack: " + name + " is not found");
            return null;
        }// 指定したファイル名が存在しなければnullを返す

        return Attacks[name];
    }

}
