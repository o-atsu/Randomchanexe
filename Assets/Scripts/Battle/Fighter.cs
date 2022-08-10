using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Battle{
public class Fighter : MonoBehaviour{
        [SerializeField]
        string Name; // ファイター名
        [SerializeField]
        private Attack[] attacks; // 使用する攻撃一覧
        [SerializeField]
        private int max_hp; // 最大HP

        private BattleController battle_controller;
        private int fighter_id;
        private int hp;
        private int[] position;


        
        public void init(int id, int[] pos){
            battle_controller = GameObject.FindWithTag("Battle Controller").GetComponent<BattleController>();
            Assert.IsFalse(battle_controller == null, "Cannot Find Battle Controller!");
            fighter_id = id;
            position = pos;
            hp = max_hp;
        }

        public string GetName(){ return Name; }

        protected int ActMove(int[] pos){ // 即時, 絶対座標(pos)へ移動
            int ret = battle_controller.Move(fighter_id, pos);
            Debug.Log(Name + ": Move from " + position[0] + ", " + position[1] + " to " + pos[0] + ", " + pos[1] + " : " + ret);
            return ret;
        }


        protected int ActAttack(int atk){ // 即時, (atk)番目の攻撃を実行
            int ret = battle_controller.Attack(fighter_id, atk);
            Debug.Log(Name + ": Attack " + attacks[atk] + " : " + ret);
            return ret;
        }


        // *FOR DEBUG*
        void Update(){
            if(Keyboard.current.aKey.wasPressedThisFrame){
                int[] tmp = new int[2]{1, 1};
                ActMove(tmp);
            }
            if(Keyboard.current.dKey.wasPressedThisFrame){
                ActAttack(0);
            }
        }
        //
    }
}
