using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/*
探索パートの敵キャラクターにアタッチするクラス
    トリガーに接触すると, AdventureToBattleに名前と位置を渡し, シーン遷移を呼び出す
*/
namespace Adventure{
public class AEnemy : AdventureCharacter{
        [SerializeField]
        private string scene_name = "test";// 戦闘用シーンの名前
        [SerializeField]
        private List<string> enemies = new List<string>();// 戦闘時の敵キャラクター一覧
        [SerializeField]
        private List<posclass> enemy_pos;// 戦闘開始時の位置

        private AdventureController adventure_controller;

        public override void init(AdventurerInfo info){
            base.init(info);

            adventure_controller = GameObject.FindWithTag("Adventure Controller").GetComponent<AdventureController>();
            Assert.IsFalse(adventure_controller == null, "Cannot Find Adventure Controller!");
            
        }

        void OnTriggerEnter(Collider other){
            if(!other.CompareTag("Player")){ return; }
            APlayer p = other.GetComponent<APlayer>();

            List<string> players = p.GetPlayerNames();
            AdventureToBattle.PlayerNames = players;
            AdventureToBattle.EnemyNames = enemies;
            List<int[]> playerpositions = p.GetPositions();
            AdventureToBattle.PlayerPositions = playerpositions;
            List<int[]> enemypositions = GetPositions();
            AdventureToBattle.EnemyPositions = enemypositions;


            adventure_controller.SceneChange(scene_name);
        }

        public List<int[]> GetPositions(){
            List<int[]> ret = new List<int[]>();
            for(int i = 0;i < enemy_pos.Count;i++){
                ret.Add(new int[2]{enemy_pos[i].coords[0], enemy_pos[i].coords[1]});
            }
            return ret;
        }

        public override Dictionary<string, string> SavedInfo(){
            Dictionary<string, string> base_info = base.SavedInfo();


            return base_info;
        }

    }
}

