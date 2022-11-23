using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;

/*
戦闘キャラクターにアタッチするクラス
    自機(FPlayer), 敵(FEnemy)はこれを継承する
    Battle Controllerに対し, 即時移動, 即時攻撃を実行
*/
namespace Battle{
public class Fighter : MonoBehaviour{

        [SerializeField]
        string Name; // ファイター名(表示名)
        [SerializeField]
        protected Attack[] attacks; // 使用する攻撃一覧
        [SerializeField]
        protected AudioClip[] attack_ses; // 使用する攻撃一覧
        [SerializeField]
        protected AudioClip hit_se; // 使用する攻撃一覧
        [SerializeField]
        protected int max_hp; // 最大HP
        [SerializeField]
        protected TextMeshProUGUI name_text; // 名前を表示するテキスト
        [SerializeField]
        protected Slider hp_bar; // HPバー

        protected BattleController battle_controller;
        protected StageEffector stage_effector;
        protected AudioSource audio_source;
        protected int fighter_id;
        protected int hp;
        protected int[] position;


        
        public virtual void init(int id, int[] pos){
            battle_controller = GameObject.FindWithTag("Battle Controller").GetComponent<BattleController>();
            stage_effector = GameObject.FindWithTag("Stage Effector").GetComponent<StageEffector>();
            audio_source = gameObject.GetComponent<AudioSource>();
            Assert.IsFalse(battle_controller == null, "Cannot Find Battle Controller!");
            Assert.IsFalse(stage_effector == null, "Cannot Find Stage Effector!");
            Assert.IsFalse(audio_source == null, "Cannot Find Audio Source!");
            fighter_id = id;
            position = pos;
            hp = max_hp;
            if(BattleController.IsPlayer(id)){
                transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }else{
                transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
            hp_bar.value = GetHP(true);
            name_text.text = Name;
        }

        public string GetName(){ return Name; }

        public int GetHP(){ return hp; }
        public float GetHP(bool percentage){ 
            if(percentage){ return ((float)hp / (float)max_hp * 100.0f); }
            return (float)hp; 
        }

        public int HitAttack(int damage){// Defeated:1, not:0
            Debug.Log("Hit: " + damage.ToString());
            audio_source.PlayOneShot(hit_se);
            hp -= damage;
            hp_bar.value = GetHP(true);
            if(hp <= 0){
                Task dftd = Defeated();
                return 1;
            }
            return 0;
        }

        protected int ActMove(int[] pos){ // 絶対座標(pos)へ移動
            int ret = battle_controller.Move(fighter_id, pos);
            // Debug.Log(Name + ": Move from " + position[0] + ", " + position[1] + " to " + pos[0] + ", " + pos[1] + " : " + ret);
            if(ret == 1){// Battle Controllerにて移動できた場合
                position = pos;
                transform.position = BattleController.FieldPosToWorld(pos);
            }
            return ret;
        }


        protected int ActAttack(Attack atk){ // (atk)番目の攻撃を実行
            int ret = battle_controller.Attack(fighter_id, atk);
            // Debug.Log(Name + ": Attack " + atk + " : " + ret);
            return ret;
        }

        protected virtual async Task Defeated(){
            // Debug.Log(Name + ": Defeated");
            gameObject.SetActive(false);
            await battle_controller.Defeated(fighter_id);
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
