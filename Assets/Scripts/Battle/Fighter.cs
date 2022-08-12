﻿using System.Collections;
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
        string Name; // ファイター名
        [SerializeField]
        protected Attack[] attacks; // 使用する攻撃一覧
        [SerializeField]
        protected int max_hp; // 最大HP

        [SerializeField]
        protected BattleController battle_controller;
        protected int fighter_id;
        protected int hp;
        protected int[] position;


        
        public void init(int id, int[] pos){
            battle_controller = GameObject.FindWithTag("Battle Controller").GetComponent<BattleController>();
            Assert.IsFalse(battle_controller == null, "Cannot Find Battle Controller!");
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

        public int Hit(int damage){// Defeated:0, not:1
            hp -= damage;
            if(hp <= 0){
                Defeated();
                return 1;
            }
            return 0;
        }

        protected int ActMove(int[] pos){ // 即時, 絶対座標(pos)へ移動
            int ret = battle_controller.Move(fighter_id, pos);
            // Debug.Log(Name + ": Move from " + position[0] + ", " + position[1] + " to " + pos[0] + ", " + pos[1] + " : " + ret);
            if(ret == 1){
                position = pos;
            }
            return ret;
        }


        protected int ActAttack(Attack atk){ // 即時, (atk)番目の攻撃を実行
            int ret = battle_controller.Attack(fighter_id, atk);
            // Debug.Log(Name + ": Attack " + atk + " : " + ret);
            return ret;
        }

        protected virtual void Defeated(){
            // Debug.Log(Name + ": Defeated");
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