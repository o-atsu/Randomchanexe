using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace Adventure{
    public class AdventureData : MonoBehaviour{
        [SerializeField]
        private string[] prefab_names;

   	    private static string adventurepath = "Assets/Prefabs/Adventure/";

        private Dictionary<string, string> Adventurers = new Dictionary<string, string>();// 相対パス一覧
    
   	    void Awake(){
   	    DontDestroyOnLoad(gameObject);
   	    
   	        foreach(string i in prefab_names){
        	    Adventurers[i] = adventurepath + i + ".prefab";
   	        }
   	    }
    
   	    // ファイターの相対パスにアクセス
   	    public string GetAdventurePath(string name){
            if(!Adventurers.ContainsKey(name)){ return null; }// 指定したファイル名が存在しなければnullを返す
            return Adventurers[name];
        }
    }
}
