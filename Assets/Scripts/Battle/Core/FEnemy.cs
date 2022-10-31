using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle{
    public class FEnemy : Fighter{

        [SerializeField]
        protected int cooltime = 300;

        private Animator anim;
        protected int player_id = 1;



        public override void init(int id, int[] pos){
            base.init(id, pos);

            anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        }
        
        async void Start(){
            await Pattern();
        }

        protected async Task AttackEvent(int atk_id){
            int i = atk_id - 1;
            anim.SetBool("Trigger", true);

            stage_effector.Startup(false, position, attacks[i]);
            await Task.Delay(attacks[i].GetStartup());

            anim.SetBool("Attacked", true);
            int a = ActAttack(attacks[i]);


            await Task.Delay(attacks[i].GetRecovery());
            anim.SetBool("Attacked", false);
            anim.SetBool("Trigger", false);
        }


        protected virtual async Task Pattern(){
            while(gameObject.activeSelf){

                await Task.Delay(cooltime);

                await AttackEvent(1);
            }
        }

    }   

}