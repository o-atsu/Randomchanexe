using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Adventure{
    public class APlayer : AdventureCharacter{
        [SerializeField]
        private List<string> player_names;
        [SerializeField]
        private List<posclass> player_pos;
        [SerializeField]
        private int num_attacks = 3;

        private Attack[] get_attacks; 
        private int[] select_attacks; 


        private void change_attacks(int before, int after){
            if(before != after){// 同じ値の引数でないなら入れ替え
                int b_pos = -1;
                int a_pos = -1;
                for(int i = 0;i < select_attacks.Length;i++){
                    if(before == select_attacks[i]){ b_pos = select_attacks[i]; }
                    if(after == select_attacks[i]){ a_pos = select_attacks[i]; }
                }
                if(b_pos == -1){
                    Debug.Log("invalid argument in chenge_attacks");
                }else if(a_pos == -1){
                    select_attacks[b_pos] = after;
                }else{
                    select_attacks[b_pos] = after;
                    select_attacks[a_pos] = before;
                }
            }


            Attack[] atks = new Attack[num_attacks];
            for(int i = 0;i < num_attacks;i++){
                atks[i] = get_attacks[select_attacks[i]];
            }
            AdventureToBattle.select_attacks = atks;
        }


        public override void init(AdventurerInfo info){
            base.init(info);
            
            string[] atks = info.additional.Split(',');
            get_attacks = new Attack[atks.Length - num_attacks - 2];
            select_attacks = new int[num_attacks];
            
            Assert.IsTrue(atks[0] == "SELECT", "additional info in APlayer is invalid");
            Assert.IsTrue(atks[num_attacks + 1] == "GET", "additional info in APlayer is invalid");
            for(int i = 1;i < num_attacks + 1;i++){
                // Debug.Log(atks[i]);
                select_attacks[i - 1] = int.Parse(atks[i]);
            }
            for(int i = num_attacks + 2;i < atks.Length;i++){
                // Debug.Log(atks[i]);
                get_attacks[i - num_attacks - 2] = AttackData.NameToAttack(atks[i]);
            }

            change_attacks(0, 0);
        }

        public List<string> GetPlayerNames(){ return player_names; }

        public List<int[]> GetPositions(){
            List<int[]> ret = new List<int[]>();
            for(int i = 0;i < player_pos.Count;i++){
                ret.Add(new int[2]{player_pos[i].coords[0], player_pos[i].coords[1]});
            }
            return ret;
        }

        public override Dictionary<string, string> SavedInfo(){// additional ... select -> getの順attackを保存
            Dictionary<string, string> base_info = base.SavedInfo();

            string save_atks = "SELECT,";
            foreach(int v in select_attacks){
                save_atks += v.ToString() + ",";
            }

            save_atks += "GET,";
            foreach(Attack a in get_attacks){
                save_atks += a.GetName() + ",";
            }

            base_info["additional"] = save_atks;

            return base_info;
        }

    }
}
