using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace Battle{
    public class FighterData : MonoBehaviour{
        [SerializeField]
        private string[] prefab_names;
    
        private static string fighterpath = "Assets/Prefabs/Battle/";
        
        private Dictionary<string, string> Fighters = new Dictionary<string, string>();// 相対パス一覧

   	    


   	    void Awake(){
   	        DontDestroyOnLoad(gameObject);
   
   	        foreach(string i in prefab_names){
        	    Fighters[i] = fighterpath + i + ".prefab";

   	        }
   	    }
    
   	    // ファイターの相対パスにアクセス
   	    public string GetFighterPath(string name){
            if(!Fighters.ContainsKey(name)){ return null; }// 指定したファイル名が存在しなければnullを返す
            return Fighters[name];
        }
    }
}
