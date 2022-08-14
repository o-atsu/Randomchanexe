using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace Battle{
    public class FighterData{
        private static Dictionary<string, string> Fighters = new Dictionary<string, string>();// 相対パス一覧

   	    static string fighterpath = "Assets/Prefabs/Battle/";
    
   	    static FighterData(){
   	        string[] fs = System.IO.Directory.GetFiles(@fighterpath, "*.prefab");
   
   	        foreach(string i in fs){
        	    Fighters[System.IO.Path.GetFileNameWithoutExtension(i)] = i;
   	        }
   	    }
    
   	    // ファイターの相対パスにアクセス
   	    public static string GetFighterPath(string name){
            if(!Fighters.ContainsKey(name)){ return null; }// 指定したファイル名が存在しなければnullを返す
            return Fighters[name];
        }
    }
}
