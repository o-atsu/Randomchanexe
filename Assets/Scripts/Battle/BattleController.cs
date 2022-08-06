using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle{
    public class BattleController : MonoBehaviour{
        [SerializeField]
        float CPS = 2;// Command Per Second
        public int stage_width = 10;
        public int stage_height = 6;

        private Dictionary<int, GameObject> players;
        private Dictionary<int, GameObject> enemies;
        private int[,] field_info;

        /* 本当は動的に名前渡して生成したい TODO
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
            field_info = new int[stage_width, stage_height];
            for(int row = 0;row < field_info.GetLength(0);row++){
                for(int column = 0;column < field_info.GetLength(1);column++){
                    field_info[row, column] = 0;
                }
            }

            for(int i = 0;i < Player_Names.Count;i++){
                int i_id = GenerateId(true);
                GameObject obj = Instantiate(tmp);
                players.Add(i_id, obj);
            }

            for(int i = 0;i < Enemy_Names.Count;i++){
                int i_id = GenerateId(false);
                GameObject obj = Instantiate(tmp);
                enemies.Add(i_id, obj);
            }
        }
        //
        

        private int GenerateId(bool is_player){// IDを生成, 一桁目が{0:敵陣営, 1:味方陣営}, 一意に決定
            int id = is_player ? players.Count + 1 : enemies.Count + 1;
            id *= 10;
            id += Convert.ToInt32(is_player);
            return id;
        }


        public bool IsPlayer(int id){ return Convert.ToBoolean(id % 10); }
        
        public int GetDelay(){// return delay (milliseconds)
            return Convert.ToInt32(1000 / CPS);
        }

        public int[] GetPosition(int id){
            for(int row = 0;row < field_info.GetLength(0);row++){
                for(int column = 0;column < field_info.GetLength(1);column++){
                    if(id == field_info[row, column]){
                        int[] pos = new int[2]{row, column};
                        return pos;
                    }
                }
            }

            Debug.Assert(true, "ID:" + id + "Is Not in field_info");
            int[] tmp = new int[2]{0, 0};
            return tmp;
        }

        // TODO
        public int Move(int ID, int[] pos){
            return 1;
        }
        public int Attack(int ID, int atk){
            return 2;
        }
    }
}
