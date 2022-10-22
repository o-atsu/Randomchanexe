using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Battle{
public class Fighter : MonoBehaviour{
        public enum MoveTo{ // WASDの順番で動く方向列挙
            Up = 1,
            Left,
            Down,
            Right
        }


        [SerializeField]
        string Name; // ファイター名(表示名)
        [SerializeField]
        protected Attack[] attacks; // 使用する攻撃一覧
        [SerializeField]
        protected int max_hp; // 最大HP

        protected BattleController battle_controller;
        protected StageEffector stage_effector;
        protected int fighter_id;
        protected int hp;
        protected int[] position;


        
        public void init(int id, int[] pos){
            battle_controller = GameObject.FindWithTag("Battle Controller").GetComponent<BattleController>();
            stage_effector = GameObject.FindWithTag("Stage Effector").GetComponent<StageEffector>();
            Assert.IsFalse(battle_controller == null, "Cannot Find Battle Controller!");
            Assert.IsFalse(stage_effector == null, "Cannot Find Stage Effector!");
            fighter_id = id;
            position = pos;
            hp = max_hp;
            if(BattleController.IsPlayer(id)){
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }else{
                transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
        }

        public string GetName(){ return Name; }

        public int GetHP(){ return hp; }
        public int GetHP(bool percentage){ 
            if(percentage){ return (int)((float)hp / (float)max_hp * 100.0f); }
            return hp; 
        }

        public int Hit(int damage){// Defeated:0, not:1
            hp -= damage;
            if(hp <= 0){
                Defeated();
                return 1;
            }
            return 0;
        }

        protected int ActMove(int[] pos){ // 絶対座標(pos)へ移動
            int ret = battle_controller.Move(fighter_id, pos);
            // Debug.Log(Name + ": Move from " + position[0] + ", " + position[1] + " to " + pos[0] + ", " + pos[1] + " : " + ret);
            if(ret == 1){
                position = pos;
            }
            return ret;
        }


        protected int ActAttack(Attack atk){ // (atk)番目の攻撃を実行
            int ret = battle_controller.Attack(fighter_id, atk);
            // Debug.Log(Name + ": Attack " + atk + " : " + ret);
            return ret;
        }

        protected virtual void Defeated(){
            // Debug.Log(Name + ": Defeated");
            int ret = battle_controller.Defeated(fighter_id);
            gameObject.SetActive(false);
        }

        // *FOR DEBUG*
        //void Update(){
        //    if(Keyboard.current.aKey.wasPressedThisFrame){
        //        int[] tmp = new int[2]{1, 1};
        //        ActMove(tmp);
        //    }
        //    if(Keyboard.current.dKey.wasPressedThisFrame){
        //        ActAttack(0);
        //    }
        //}
        //
    }
}
