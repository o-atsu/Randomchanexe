using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle{
    public class FRandomChan : FEnemy{

        private Dictionary<string, int> NameToAnimID = new Dictionary<string, int>(){{"ビーム", 1}, {"ファイアーブレード", 2}, {"パンチ", 3}, {"大剣", 4}, {"Xビット", 5}};


        public override void init(int id, int[] pos){
            base.init(id, pos);
            
            anim = gameObject.GetComponent<Animator>();
        }

        protected override async Task Pattern(){
            int m;
            await Task.Delay(1000);
            

            while(true){
                if(hp < max_hp / 2){ break; }

                await Task.Delay(cooltime * Random.Range(1, 6));


                int[] player_pos = battle_controller.GetPosition(player_id);
                
                m = ActMove(new int[]{Mathf.Min(position[0] + Random.Range(-1, 2), 8), position[1] + Random.Range(-1, 2)});

                // int[] pre_pos = position;
                // m = ActMove(new int[]{BattleController.stage_width / 2, position[1]});
                await AttackEvent(Random.Range(2, 6));
                // m = ActMove(pre_pos);
                

            }

            m = ActMove(new int[]{BattleController.stage_width / 2, 1});
            await Task.Delay(cooltime);
            await AttackEvent(1);
            
            m = ActMove(new int[]{BattleController.stage_width / 2, 3});
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
                    await AttackEvent(Random.Range(2, 6));
                    m = ActMove(pre_pos);
                    
                }
            }

        }
        
        
        protected override async Task AttackEvent(int atk_id){
            // Debug.Log(attacks.Length);

            int i = atk_id - 1;

            anim.SetInteger("motion", NameToAnimID[attacks[i].GetName()]);
            anim.SetTrigger("Trigger");
            stage_effector.Startup(false, position, attacks[i]);
            
            await Task.Delay(attacks[i].GetStartup() - pre_action);
            
            anim.SetTrigger("Attacked");
            await Task.Delay(pre_action);
            int a = ActAttack(attacks[i]);

            await Task.Delay(attacks[i].GetRecovery());

        }

    }   

}
