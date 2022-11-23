using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarriorAnimsFREE;


/*
戦闘パートのKnightWarrierのルーチンを記述したクラス
*/
namespace Battle{
    public class FKnight : FEnemy{

        private WarriorController warrior_controller;
        
        public override void init(int id, int[] pos){
            base.init(id, pos);
            
            warrior_controller = gameObject.GetComponent<WarriorController>();
        }

        protected override async Task Pattern(){
            while(gameObject.activeSelf){

                await Task.Delay(cooltime * 2);


                int[] player_pos = battle_controller.GetPosition(player_id);
                
                await CheckScene();
                int m = ActMove(new int[]{position[0], position[1] + Random.Range(-1, 2)});

                
                await Task.Delay(cooltime);
                await CheckScene();
                m = ActMove(new int[]{position[0] + Random.Range(-1, 2), position[1]});
                if(Mathf.Abs(position[1] - player_pos[1]) < 2){
                    int[] pre_pos = position;
                    await CheckScene();
                    m = ActMove(new int[]{BattleController.stage_width / 2, position[1]});
                    await AttackEvent(1);
                    await CheckScene();
                    m = ActMove(pre_pos);
                    
                }

            }
        }

    }   

}