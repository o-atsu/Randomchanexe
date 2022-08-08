using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Battle{
    public class BattleController : MonoBehaviour{
        public static int CPM = 120;// Command Per Minite
        public int stage_width = 10;
        public int stage_height = 5;

        private Dictionary<int, GameObject> players;
        private Dictionary<int, GameObject> enemies;
        private int[,] field_info;// Down-Left is {0, 0}

        /* 本当は動的に名前とポジション渡して生成したい TODO
        public BattleController(List<string> player_names, List<string> enemy_names){
            for(int i = 0;i < player_names.Count;i++){
                int i_id = GenerateId(true);
                GameObject obj = Instantiate(tmp, new Vector3(0.0f, 0.0f, 0.0f));
                players.Add(i_id, obj);
            }

            for(int i = 0;i < enemy_names.Count;i++){
                int i_id = GenerateId(false);
                GameObject obj = Instantiate(tmp, new Vector3(0.0f, 0.0f, 0.0f));
                enemies.Add(i_id, obj);
        }
        */


        // *FOR DEBUG*
        public GameObject tmp;
        public List<string> Player_Names; // TODO 名前 to Prefab
        public List<string> Enemy_Names;

        void Awake(){
            players = new Dictionary<int, GameObject>();
            enemies = new Dictionary<int, GameObject>();
            int[,] player_pos = new int[,]{{3, 2}};
            int[,] enemy_pos = new int[,]{{7, 4}, {8, 2}, {9, 0}};
            field_info = new int[stage_width, stage_height];
            for(int row = 0;row < field_info.GetLength(0);row++){
                for(int column = 0;column < field_info.GetLength(1);column++){
                    field_info[row, column] = 0;
                }
            }

            for(int i = 0;i < Player_Names.Count;i++){
                int[] field_pos = new int[]{player_pos[i, 0], player_pos[i, 1]};
                GenerateFighter(tmp, true, field_pos);
            }

            for(int i = 0;i < Enemy_Names.Count;i++){
                int[] field_pos = new int[]{enemy_pos[i, 0], enemy_pos[i, 1]};
                GenerateFighter(tmp, false, field_pos);
            }
        }
        //


        private int GenerateId(bool is_player){// IDを生成, 一桁目が{0:敵陣営, 1:味方陣営}, 2桁目以降が各陣営の登録済みオブジェクト数
            int id = is_player ? players.Count + 1 : enemies.Count + 1;
            id *= 10;
            id += Convert.ToInt32(is_player);
            return id;
        }

        private void GenerateFighter(GameObject prefab, bool is_player, int[] field_pos){// プレハブを指定位置に生成
                int obj_id = GenerateId(is_player);
                GameObject obj = Instantiate(prefab);
                Vector3 pos = FieldPosToWorld(field_pos);
                obj.transform.position = pos;
                field_info[field_pos[0], field_pos[1]] = obj_id;
                
                Fighter fighter = obj.GetComponent<Fighter>();
                Assert.IsFalse(fighter == null, "Fighter Is Not Attached in " + prefab);
                fighter.SetPosition(field_pos);

                if(is_player){ players.Add(obj_id, obj); }
                else{ enemies.Add(obj_id, obj); }
        }

        

        // TODO
        public int Move(int ID, int[] pos){
            return 1;
        }
        public int Attack(int ID, int atk){
            return 2;
        }
        //

        public int[] GetPosition(int id){// IDを渡すとそのオブジェクトのポジションを返す
            for(int row = 0;row < field_info.GetLength(0);row++){
                for(int column = 0;column < field_info.GetLength(1);column++){
                    if(id == field_info[row, column]){
                        int[] pos = new int[2]{row, column};
                        return pos;
                    }
                }
            }

            Debug.Assert(true, "ID:" + id + "Is Not in field_info");
            int[] tmp = new int[2]{-1, -1};
            return tmp;
        }


        public static int[] WorldPosToField(Vector3 world_pos){// ワールド座標からフィールド座標へ変換
            int[] field_pos = new int[2]{Convert.ToInt32(world_pos.x), Convert.ToInt32(world_pos.z)}; 
            return field_pos;
        }

        public static Vector3 FieldPosToWorld(int[] field_pos){// フィールド座標からワールド座標へ変換
            Vector3 world_pos = new Vector3((float)field_pos[0], 0.0f, (float)field_pos[1]);
            return world_pos;
        }

        public static bool IsPlayer(int id){ return Convert.ToBoolean(id % 10); }// IDから陣営を判定
        
        public static int GetDelay(){// delayを返す (milliseconds) /*for Task.Delay(this)*/
            return Convert.ToInt32(1000 * 60 / CPM);
        }
    }
}
