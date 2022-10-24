using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace Adventure{
    public class AdventureData{

   	    private static string adventurepath = "Assets/Prefabs/Adventure/";
        private static Dictionary<string, string> Adventurers = new Dictionary<string, string>();// 相対パス一覧
    
   	    static AdventureData(){
   	        string[] ads = System.IO.Directory.GetFiles(@adventurepath, "*.prefab");
   
   	        foreach(string i in ads){
        	    Adventurers[System.IO.Path.GetFileNameWithoutExtension(i)] = i;
   	        }
   	    }
    
   	    // ファイターの相対パスにアクセス
   	    public static string GetAdventurePath(string name){
            if(!Adventurers.ContainsKey(name)){ return null; }// 指定したファイル名が存在しなければnullを返す
            return Adventurers[name];
        }
    }
}
