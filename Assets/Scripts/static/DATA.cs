using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
PLAYERS:Prefabs/PlayersディレクトリのPrefabを取得
ENEMIES:Prefabs/EnemiesディレクトリのPrefabを取得

*現在未使用*
*/

public class DATA{
    private static List<string> PLAYERS = new List<string>();
    private static List<string> ENEMIES = new List<string>();

	static string playerpath = "Assets/Resources/Prefabs/Players";
	static string enemypath = "Assets/Resources/Prefabs/Enemies";

	static DATA(){
		string[] ps = System.IO.Directory.GetFiles(@playerpath, "*.prefab");
		string[] es = System.IO.Directory.GetFiles(@enemypath, "*.prefab");

		foreach(string i in ps){
			PLAYERS.Add(System.IO.Path.GetFileNameWithoutExtension(i));

		}
		foreach(string i in es){
			ENEMIES.Add(System.IO.Path.GetFileNameWithoutExtension(i));
		}
	}

	// パラメータへのアクセス
	public static string GetPlayer(int i){ return PLAYERS[i]; }
	public static string GetEnemy(int i){ return ENEMIES[i]; }
}
