using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle{
    public class FEnemy : Fighter{

        [SerializeField]
        protected int cooltime = 300;
        [SerializeField]
        private Attack reward;

        private Animator anim;
        private int pre_action = 200;
        protected int player_id = 1;



        public override void init(int id, int[] pos){
            base.init(id, pos);

            anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        }


        async Task Start(){
            await Pattern();
        }


        protected async Task AttackEvent(int atk_id){
            int i = atk_id - 1;
            anim.SetTrigger("Trigger");

            stage_effector.Startup(false, position, attacks[i]);
            await Task.Delay(attacks[i].GetStartup() - pre_action);

            anim.SetTrigger("Attacked");
            await Task.Delay(pre_action);
            int a = ActAttack(attacks[i]);


            await Task.Delay(attacks[i].GetRecovery());

        }
        
        protected override void Defeated(){
            if(reward != null){
                BattleToAdventure.RewardAttacks.Add(reward);
            }
            
            base.Defeated();
        }


        protected virtual async Task Pattern(){
            while(gameObject.activeSelf){

                await Task.Delay(cooltime);

                await AttackEvent(1);
            }
        }

    }   

}
