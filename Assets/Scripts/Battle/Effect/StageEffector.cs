using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Battle{
    public class StageEffector : MonoBehaviour
    {
        private StageUnit[,] Unit;// 各座標に対応するゲームオブジェクト
    
        void Awake(){
            Unit = new StageUnit[BattleController.stage_width, BattleController.stage_height];
        
            Transform children = gameObject.GetComponentInChildren<Transform>();
            if(children.childCount == 0){ return; }
            foreach(Transform floor in children){
                string child_name = floor.name;
                // floor(\d)(\d) のみ検出
                if(!System.Text.RegularExpressions.Regex.IsMatch(child_name, @"floor\d\d")){ continue; }
                // charからintに変換
                int w = child_name[5] - '0';
                int h = child_name[6] - '0';
                
                // Debug.Log(w + ", " + h);
                Unit[w, h] = floor.gameObject.GetComponent<StageUnit>();
            }

        }

        public void Startup(bool isplayer, int[] pos, Attack atk){
            // Debug.Log("StartUp");
            List<int[]> range = atk.GetRange();

            for(int i = 0;i < range.Count;i++){
                if(!isplayer){
                    range[i][0] *= -1;
                    range[i][1] *= -1;
                }
                int[] atk_pos = new int[]{pos[0] + range[i][0], pos[1] + range[i][1]};
                if(!BattleController.InField(atk_pos)){ continue; }

                Unit[atk_pos[0], atk_pos[1]].Startup(atk.GetStartup());
            }
        }
    }
}
