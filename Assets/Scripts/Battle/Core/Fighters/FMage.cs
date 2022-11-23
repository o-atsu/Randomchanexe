using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


/*
戦闘パートのMageWarrierのルーチンを記述したクラス
*/
namespace Battle{
    public class FMage : FEnemy{

        protected override async Task Pattern(){
            while(gameObject.activeSelf){

                await CheckScene();
                await Task.Delay(cooltime);


                int[] player_pos = battle_controller.GetPosition(player_id);
                
                int m;
                if(position[1] != player_pos[1]){
                    await CheckScene();
                    m = ActMove(new int[]{position[0], position[1] + Random.Range(-1, 2)});
                }
                if(position[1] == player_pos[1]){
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
