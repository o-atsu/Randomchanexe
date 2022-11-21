using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Battle{
    public class FEnemy : Fighter{

        [SerializeField]
        protected int cooltime = 300;
        [SerializeField]
        private Attack reward;

        protected Animator anim;
        protected int pre_action = 200;
        protected int player_id = 1;
        protected bool menu_opened = false;
        private string this_scene;


        public override void init(int id, int[] pos){
            base.init(id, pos);

            anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
            this_scene = SceneManager.GetActiveScene().name;
            SceneManager.sceneLoaded += (s, m) => {menu_opened = true;};
            SceneManager.sceneUnloaded += (s) => {menu_opened = false;};
        }


        async Task Start(){
            await Pattern();
        }

        protected async Task CheckScene(){
            while(menu_opened){
                await Task.Delay(10);
            }
        }

        protected virtual async Task AttackEvent(int atk_id){
            int i = atk_id - 1;

            await CheckScene();
            anim.SetTrigger("Trigger");

            await CheckScene();
            stage_effector.Startup(false, position, attacks[i]);
            await Task.Delay(attacks[i].GetStartup() - pre_action);

            await CheckScene();
            anim.SetTrigger("Attacked");
            audio_source.PlayOneShot(attack_ses[i]);
            await Task.Delay(pre_action);
            int a = ActAttack(attacks[i]);


            await CheckScene();
            await Task.Delay(attacks[i].GetRecovery());

        }
        
        protected override async Task Defeated(){
            if(reward != null){
                BattleToAdventure.RewardAttacks.Add(reward);
            }
            
            await base.Defeated();
        }


        protected virtual async Task Pattern(){
            while(gameObject.activeSelf){

                await Task.Delay(cooltime);

                await AttackEvent(1);
            }
        }

    }   

}
