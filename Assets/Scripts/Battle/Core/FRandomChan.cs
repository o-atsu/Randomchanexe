using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using WarriorAnimsFREE;


namespace Battle{
    public class FRandomChan : FEnemy{

        private WarriorController warrior_controller;
        
        public override void init(int id, int[] pos){
            base.init(id, pos);
            
            warrior_controller = gameObject.GetComponent<WarriorController>();
        }

        protected override async Task Pattern(){
            int m;
            while(hp > max_hp / 2){

                await Task.Delay(cooltime * Random.Range(1, 5));


                int[] player_pos = battle_controller.GetPosition(player_id);
                
                m = ActMove(new int[]{position[0] + Random.Range(-1, 2), position[1] + Random.Range(-1, 2)});


                if(Mathf.Abs(position[1] - player_pos[1]) < 2){
                    int[] pre_pos = position;
                    m = ActMove(new int[]{BattleController.stage_width / 2, position[1]});
                    await AttackEvent(Random.Range(1, 5));
                    m = ActMove(pre_pos);
                    
                }

            }

            m = ActMove(new int[]{BattleController.stage_width / 2, 1});
            await Task.Delay(cooltime);
            await AttackEvent(1);
            
            m = ActMove(new int[]{BattleController.stage_width / 2, 1});
            await Task.Delay(cooltime);
            await AttackEvent(1);
            await Task.Delay(cooltime);
            
            while(hp > 0){
                await Task.Delay(cooltime * Random.Range(1, 3));


                int[] player_pos = battle_controller.GetPosition(player_id);
                
                m = ActMove(new int[]{position[0] + Random.Range(-1, 2), position[1] + Random.Range(-1, 2)});


                if(Mathf.Abs(position[1] - player_pos[1]) < 2){
                    int[] pre_pos = position;
                    m = ActMove(new int[]{BattleController.stage_width / 2, position[1]});
                    await AttackEvent(Random.Range(1, 5));
                    m = ActMove(pre_pos);
                    
                }
            }

        }

    }   

}
