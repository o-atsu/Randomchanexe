using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class IconData{

    private static string iconpath = "Assets/UIIcons/";
    private static Dictionary<string, string> Icons = new Dictionary<string, string>();// 相対パス一覧
    
    static IconData(){
        string[] ads = System.IO.Directory.GetFiles(@iconpath, "*.png");
   
        foreach(string i in ads){
    	    Icons[System.IO.Path.GetFileNameWithoutExtension(i)] = i;
        }
    }
    
    // アイコンの相対パスにアクセス
    public static string GetIconPath(string name){
        if(!Icons.ContainsKey(name)){ return null; }// 指定したファイル名が存在しなければnullを返す
        return Icons[name];
    }
}
