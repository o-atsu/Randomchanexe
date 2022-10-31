using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle{// 末端クラスでのみAwake使いたいね

    public class FPlayer : Fighter{
        
        private Dictionary<string, int> NameToAnimID = new Dictionary<string, int>(){{"ビーム", 1}, {"ファイアーブレード", 2}, {"パンチ", 3}, {"大剣", 4}, {"Xビット", 5}};
        private int pre_action = 200;
        
        private Vector2 now = new Vector2(0.0f, 0.0f);
        private bool IsIdle = true;
        private Animator anim;

        


        public override void init(int id, int[] pos){
            base.init(id, pos);

            attacks = AdventureToBattle.select_attacks;
            anim = gameObject.GetComponent<Animator>();
            // Debug.Log(id);
        }


        private async Task AttackEvent(int atk_id){
            if(!IsIdle){ return; }
            // Debug.Log(attacks.Length);

            int i = atk_id - 1;

            IsIdle = false;
            anim.SetInteger("motion", NameToAnimID[attacks[i].GetName()]);
            anim.SetTrigger("Trigger");
            stage_effector.Startup(true, position, attacks[i]);
            
            await Task.Delay(attacks[i].GetStartup() - pre_action);
            
            anim.SetTrigger("Attacked");
            await Task.Delay(pre_action);
            int a = ActAttack(attacks[i]);

            await Task.Delay(attacks[i].GetRecovery());
            IsIdle = true;
        }

        
        public void OnMove(InputAction.CallbackContext obj){
            if(!IsIdle){ return; }

            Vector2 in_dir = obj.ReadValue<Vector2>();
            // 十字キーの縦横同時押し -> 後に押したキーを適用
            Vector2 diff = in_dir - now;
            if(now == in_dir){ return; }
            now = in_dir;

            int[] next_pos = new int[]{position[0] + (int)(in_dir.x * Mathf.Abs(diff.x)), position[1] + (int)(in_dir.y * Mathf.Abs(diff.y))};
            int a = ActMove(next_pos);
        }

        public async void OnAttack1(InputAction.CallbackContext obj){
            // 押しはじめに攻撃
            if(!obj.started){ return; }
            await AttackEvent(1);
        }
        public async void OnAttack2(InputAction.CallbackContext obj){
            if(!obj.started){ return; }
            await AttackEvent(2);
        }
        public async void OnAttack3(InputAction.CallbackContext obj){
            if(!obj.started){ return; }
            await AttackEvent(3);
        }

        
        
    }
}
