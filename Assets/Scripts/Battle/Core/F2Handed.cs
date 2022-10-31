using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle{
    public class F2Handed : FEnemy{



        protected override async Task Pattern(){
            while(gameObject.activeSelf){

                await Task.Delay(cooltime);


                int[] player_pos = battle_controller.GetPosition(player_id);
                
                int m;
                if(position[1] != player_pos[1]){
                    m = ActMove(new int[]{position[0], position[1] + Random.Range(-1, 2)});
                }
                if(position[1] == player_pos[1]){
                    int[] pre_pos = position;
                    m = ActMove(new int[]{BattleController.stage_width / 2, position[1]});
                    await AttackEvent(1);
                    m = ActMove(pre_pos);
                    
                }

            }
        }

    }   

}
