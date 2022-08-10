using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battle{// 末端クラスでのみAwake使いたいね

    public class FPlayer : Fighter{

        private string[] animator_flags = {"neutral", "Up", "Left", "Down", "Right"}; // TODO animatorの分岐をboolで制御

        private Vector2 now = new Vector2(0.0f, 0.0f);
        private int atk_id = 0;


        private int InputToState(Vector2 vec){
            
            if(vec.y == 1){ return (int)MoveTo.Up; }
            else if(vec.x == -1){ return (int)MoveTo.Left; }
            else if(vec.y == -1){ return (int)MoveTo.Down; }
            else if(vec.x == 1){ return (int)MoveTo.Right; }

            return 0;
        }

        
        public void OnMove(InputAction.CallbackContext obj){
            if(!obj.performed){ return; }
            Vector2 in_dir = obj.ReadValue<Vector2>();
            // 十字キーの縦横同時押し -> 後に押したキーを適用
            Vector2 diff = in_dir - now;
            if(now == in_dir){ return; }
            now = in_dir;

            int[] next_dir = new int[]{position[0] + (int)(in_dir.x * Mathf.Abs(diff.x)), position[1] + (int)(in_dir.y * Mathf.Abs(diff.y))};
            int a = ActMove(next_dir);
        }

        public void OnAttack(InputAction.CallbackContext obj){
            // 押しはじめに攻撃
            if(!obj.started){ return; }
            int a = ActAttack(atk_id);
        }
        
    }
}
