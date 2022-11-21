using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;



namespace Battle{
    public class BattleController : MonoBehaviour{
        // public static int CPM = 120;// Command Per Minite
        public static int stage_width = 10;
        public static int stage_height = 5;

        [SerializeField]
        private GameObject win_panel;
        [SerializeField]
        private GameObject lose_panel;
        [SerializeField]
        private int end_delay = 2000;

        private Dictionary<int, GameObject> fighters;
        private Dictionary<int, bool> islive;
        private int[,] field_info;// Down-Left is {0, 0}
        private FighterData ftr_data;

        private string adv_scene = "Playground";



        async void Awake(){
            List<string> player_names = AdventureToBattle.PlayerNames; 
            List<string> enemy_names = AdventureToBattle.EnemyNames;
            List<int[]> player_pos = AdventureToBattle.PlayerPositions;
            List<int[]> enemy_pos = AdventureToBattle.EnemyPositions;
            ftr_data = GameObject.FindGameObjectWithTag("Fighter Data").GetComponent<FighterData>();

            fighters = new Dictionary<int, GameObject>();
            islive = new Dictionary<int, bool>();
            field_info = new int[stage_width, stage_height];
            for(int row = 0;row < field_info.GetLength(0);row++){
                for(int column = 0;column < field_info.GetLength(1);column++){
                    field_info[row, column] = 0;
                }
            }

            for(int i = 0;i < player_names.Count;i++){
                int[] field_pos = new int[]{player_pos[i][0], player_pos[i][1]};
                await GenerateFighter(player_names[i], true, field_pos);
            }

            for(int i = 0;i < enemy_names.Count;i++){
                int[] field_pos = new int[]{enemy_pos[i][0], enemy_pos[i][1]};
                await GenerateFighter(enemy_names[i], false, field_pos);
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


        private int GenerateId(bool is_player){// IDを生成, 一桁目が{0:敵陣営, 1:味方陣営}, 2桁目以降が各陣営の登録済みオブジェクト数
            int id = fighters.Count;
            id *= 10;
            id += Convert.ToInt32(is_player);
            return id;
        }

        private async Task GenerateFighter(string name, bool is_player, int[] field_pos){// プレハブを指定位置に生成
            string path = ftr_data.GetFighterPath(name);
            Assert.IsFalse(path == null, "Cannot Find Fighter: " + name);
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path);

            int obj_id = GenerateId(is_player);
            GameObject obj = await handle.Task;
            Vector3 pos = FieldPosToWorld(field_pos);
            obj.transform.position = pos;
            // Debug.Log(field_pos[0] + ", " + field_pos[1]);
            field_info[field_pos[0], field_pos[1]] = obj_id;
                
            Fighter fighter = obj.GetComponent<Fighter>();
            Assert.IsFalse(fighter == null, "Fighter Is Not Attached in " + name);
            fighter.init(obj_id, field_pos);

            fighters.Add(obj_id, obj);
            islive.Add(obj_id, true);
        }

        private async Task OnWin(){
            win_panel.SetActive(true);
            await Task.Delay(end_delay);
        }

        private async Task OnLose(){
            lose_panel.SetActive(true);
            await Task.Delay(end_delay);
        }

        private async Task SceneChange(bool player_win){
            BattleToAdventure.SavedInfoName = AdventureToBattle.SavedInfoName;

            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(adv_scene, LoadSceneMode.Single);
            await handle.Task;
        }


        private bool CanMove(int[] pos, bool is_player){// 移動先が動ける範囲かどうか, 別のFighterがいるか
            if(is_player){
                return (0 <= pos[0] && pos[0] < stage_width / 2) && (0 <= pos[1] && pos[1] < stage_height) && (field_info[pos[0], pos[1]] == 0);
            }else{
                return (stage_width / 2 <= pos[0] && pos[0] < stage_width) && (0 <= pos[1] && pos[1] < stage_height) && (field_info[pos[0], pos[1]] == 0);
            }
        }

        public int Move(int ID, int[] pos){// Moveできない：0, Move完了：1
            if(!CanMove(pos, IsPlayer(ID))){ return 0; }
            
            int[] pre = GetPosition(ID);
            field_info[pos[0], pos[1]] = ID;
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

                int defeated = fighters[hit_id].GetComponent<Fighter>().HitAttack(atk.GetDamage());
                if(defeated == 1){
                    field_info[atk_pos[0], atk_pos[1]] = 0;
                }

                hit = 1;
            }
            return hit;
        }

        public async Task Defeated(int ID){
            // Debug.Log("Defeated: " + ID);

            int[] pos = GetPosition(ID);
            field_info[pos[0], pos[1]] = 0;
            islive[ID] = false;
            // ShowField();

            int num_players = 0;
            int num_enemies = 0;
            foreach(KeyValuePair<int, bool> live in islive){
                if(live.Value){
                    if(IsPlayer(live.Key)){ num_players++; }
                    else{ num_enemies++; }
                }
            }

            if(num_enemies == 0){
                await OnWin();
                await SceneChange(true);
            }else if(num_players == 0){
                await OnLose();
                await SceneChange(false);
            }
        }

        public int[] GetPosition(int ID){// IDを渡すとそのオブジェクトのポジションを返す
            for(int row = 0;row < field_info.GetLength(0);row++){
                for(int column = 0;column < field_info.GetLength(1);column++){
                    if(ID == field_info[row, column]){
                        int[] pos = new int[2]{row, column};
                        return pos;
                    }
                }
            }

            Debug.Assert(true, "ID:" + ID + "Is Not in field_info");
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

        public static bool IsPlayer(int ID){ return Convert.ToBoolean(ID % 10); }// IDから陣営を判定

        public static bool InField(int[] pos){ // 座標がフィールドの範囲内かどうかを返す
            return (0 <= pos[0] && pos[0] < stage_width) && (0 <= pos[1] && pos[1] < stage_height);
        }

        
        //public static int GetDelay(){// delayを返す (milliseconds) /*for Task.Delay(this)*/
        //    return Convert.ToInt32(1000 * 60 / CPM);
        //}
    }
}
