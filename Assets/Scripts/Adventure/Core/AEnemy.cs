using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Adventure{
public class AEnemy : MonoBehaviour{
        [SerializeField]
        private string scene_name = "test";
        [SerializeField]
        private List<string> enemies = new List<string>();
        [SerializeField]
        private List<posclass> enemy_pos;

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

            SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
        }

        public List<int[]> GetPositions(){
            List<int[]> ret = new List<int[]>();
            for(int i = 0;i < enemy_pos.Count;i++){
                ret.Add(new int[2]{enemy_pos[i].coords[0], enemy_pos[i].coords[1]});
            }
            return ret;
        }

        // for inspector
        [System.SerializableAttribute]
        public class posclass{
            public int[] coords = {0, 0};
        }

    }
}

