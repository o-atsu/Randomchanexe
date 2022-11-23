using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
敵の戦闘キャラクターにアタッチするクラス
    Patternにルーチンを記述
    これを継承しPatternをオーバーライドすることで敵の挙動を実装
*/
namespace Battle{
    public class FEnemy : Fighter{

        [SerializeField]
        protected int cooltime = 300;
        [SerializeField]
        private Attack reward;// 倒すと獲得できるAttack

        protected Animator anim;
        protected int pre_action = 200;// 攻撃判定のタイミングよりも少し早くアニメーション, SEを切り替え
        protected int player_id = 1;
        protected bool menu_opened = false;
        private string this_scene;


        public override void init(int id, int[] pos){
            base.init(id, pos);

            anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
            this_scene = SceneManager.GetActiveScene().name;
            // メニューを開いたか, 閉じたかを検知
            SceneManager.sceneLoaded += (s, m) => {menu_opened = true;};
            SceneManager.sceneUnloaded += (s) => {menu_opened = false;};
        }


        async Task Start(){
            await Pattern();
        }

        // メニューを開いていたら, 閉じるまで待機
        protected async Task CheckScene(){
            while(menu_opened){
                await Task.Delay(10);
            }
        }

        protected virtual async Task AttackEvent(int atk_id){
            int i = atk_id - 1;

            await CheckScene();
            anim.SetTrigger("Trigger");

            // 予備動作
            await CheckScene();
            stage_effector.Startup(false, position, attacks[i]);
            await Task.Delay(attacks[i].GetStartup() - pre_action);

            // アニメーション切り替え
            await CheckScene();
            anim.SetTrigger("Attacked");
            audio_source.PlayOneShot(attack_ses[i]);
            await Task.Delay(pre_action);
            int a = ActAttack(attacks[i]);


            // 後隙
            await CheckScene();
            await Task.Delay(attacks[i].GetRecovery());

        }
        
        protected override async Task Defeated(){
            if(reward != null && !BattleToAdventure.RewardAttacks.Contains(reward)){
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
