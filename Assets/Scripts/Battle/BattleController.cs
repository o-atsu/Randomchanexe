using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


// TODO
// OnGUIで座標とアクション知れる
// GUI box

namespace Battle{
    public class BattleController : MonoBehaviour{
        public static int CPM = 120;// Command Per Minite
        public int stage_width = 10;
        public int stage_height = 5;

        private Dictionary<int, GameObject> fighters;
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
        public GameObject player;
        public GameObject enemy;
        public List<string> Player_Names; // TODO 名前 to Prefab
        public List<string> Enemy_Names;

        void Awake(){
            fighters = new Dictionary<int, GameObject>();
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
                GenerateFighter(player, true, field_pos);
            }

            for(int i = 0;i < Enemy_Names.Count;i++){
                int[] field_pos = new int[]{enemy_pos[i, 0], enemy_pos[i, 1]};
                GenerateFighter(enemy, false, field_pos);
            }

            ShowField();
        }

        private void ShowField(){
            string ret = "Field Information:\n";
            for(int column = field_info.GetLength(1) - 1;column >= 0;column--){
                for(int row = 0;row < field_info.GetLength(0);row++){
                    ret += field_info[row, column] + "\t";
                }
                ret += "\n";
            }
            Debug.Log(ret);
        }
        //


        private int GenerateId(bool is_player){// IDを生成, 一桁目が{0:敵陣営, 1:味方陣営}, 2桁目以降が各陣営の登録済みオブジェクト数
            int id = fighters.Count;
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
            fighter.init(obj_id, field_pos);

            fighters.Add(obj_id, obj);
        }

        private bool InField(int[] pos){
            return (0 <= pos[0] && pos[0] < stage_width) && (0 <= pos[1] && pos[1] < stage_height);
        }

        private bool CanMove(int[] pos, bool is_player){// 移動先が動ける範囲かどうか, 別のFighterがいるか
            if(is_player){
                return (0 <= pos[0] && pos[0] < stage_width / 2) && (0 <= pos[1] && pos[1] < stage_height) && (field_info[pos[0], pos[1]] == 0);
            }else{
                return (stage_width / 2 <= pos[0] && pos[0] < stage_width) && (0 <= pos[1] && pos[1] < stage_height) && (field_info[pos[0], pos[1]] == 0);
            }
        }


        public int Move(int id, int[] pos){// Moveできない：0, Move完了：1
            if(!CanMove(pos, IsPlayer(id))){ return 0; }
            
            int[] pre = GetPosition(id);
            field_info[pos[0], pos[1]] = id;
            field_info[pre[0], pre[1]] = 0;

            // ShowField();

            return 1;
        }

        public int Attack(int ID, Attack atk){
            int hit = 0;
            List<int[]> range = atk.GetRange();
            int[] pos = GetPosition(ID);

            for(int i = 0;i < range.Count;i++){
                if(!IsPlayer(ID)){
                    range[i][0] *= -1;
                    range[i][1] *= -1;
                }
                int[] atk_pos = new int[]{pos[0] + range[i][0], pos[1] + range[i][1]};
                if(!InField(atk_pos)){ continue; }

                int hit_id = field_info[atk_pos[0], atk_pos[1]];
                if(hit_id == 0 || IsPlayer(hit_id) == IsPlayer(ID)){ continue; }

                int defeated = fighters[hit_id].GetComponent<Fighter>().Hit(atk.GetDamage());
                if(defeated == 1){
                    field_info[atk_pos[0], atk_pos[1]] = 0;// ヒットと同時に動くとダメ?
                }

                hit = 1;
            }
            return hit;
        }

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
