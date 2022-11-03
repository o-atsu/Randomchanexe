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

        [SerializeField]
        private Attack[] get_attacks; 
        private int[] select_attacks = new int[]{1, 1, 1}; 


        public bool ChangeAttacks(int s_index, int after){
            /*
            if(before != after){// 同じ値の引数でないなら入れ替え
                int b_pos = -1;
                int a_pos = -1;
                for(int i = 0;i < select_attacks.Length;i++){
                    if(before == select_attacks[i]){ b_pos = select_attacks[i]; }
                    if(after == select_attacks[i]){ a_pos = select_attacks[i]; }
                }
                if(b_pos == -1){
                    Debug.Log("invalid argument in chenge_attacks");
                    return false;
                }else if(a_pos == -1){
                    Debug.Log("invalid argument in chenge_attacks");
                    return false;
                }else{
                    select_attacks[b_pos] = after;
                    select_attacks[a_pos] = before;
                }
                    
            }
            */
            if(s_index != -1){ select_attacks[s_index] = after; }


            Attack[] atks = new Attack[num_attacks];
            for(int i = 0;i < num_attacks;i++){
                // Debug.Log(select_attacks[i]);
                atks[i] = get_attacks[select_attacks[i] - 1];
            }
            AdventureToBattle.select_attacks = atks;

            // Debug.Log("Changed: " + after + " in " + s_index);
            Debug.Log("Attacks: " + select_attacks[0] + ", " + select_attacks[1] + ", " + select_attacks[2]);
            Debug.Log("Attacks: " + AdventureToBattle.select_attacks[0].GetName() + ", " + AdventureToBattle.select_attacks[1].GetName() + ", " + AdventureToBattle.select_attacks[2].GetName());
            return true;
        }


        public override void init(AdventurerInfo info){
            base.init(info);
            
            List<Attack> rewarded = BattleToAdventure.RewardAttacks;
            
            string[] atks = info.additional.Split(',');
            get_attacks = new Attack[atks.Length + rewarded.Count - num_attacks - 2];
            select_attacks = new int[num_attacks];
            
            Assert.IsTrue(atks[0] == "SELECT", "additional info in APlayer is invalid");
            Assert.IsTrue(atks[num_attacks + 1] == "GET", "additional info in APlayer is invalid");
            for(int i = 1;i < num_attacks + 1;i++){
                select_attacks[i - 1] = int.Parse(atks[i]);
            }
            

            bool[] new_atk = new bool[rewarded.Count];
            for(int i = 0;i < rewarded.Count;i++){ new_atk[i] = true; }
            
            for(int i = num_attacks + 2;i < atks.Length;i++){
                Debug.Log(atks[i]);
                get_attacks[i - num_attacks - 2] = AttackData.NameToAttack(atks[i]);
                
                for(int k = 0;k < rewarded.Count;k++){
                    if(atks[i] == rewarded[k].GetName()){ new_atk[k] = false; }
                    // Debug.Log(rewarded[k].GetName());
                }
            }
            
            for(int i = 0;i < rewarded.Count;i++){
                if(new_atk[i]){ get_attacks[get_attacks.Length - i - 1] = rewarded[i]; }
            }

            bool saved = ChangeAttacks(-1, -1);
            BattleToAdventure.RewardAttacks = new List<Attack>();
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
            
            float pos_z = float.Parse(base_info["position_z"]) - 2.0f;
            base_info["position_z"] = pos_z.ToString();

            string save_atks = "SELECT,";
            foreach(int v in select_attacks){
                save_atks += v.ToString() + ",";
            }

            save_atks += "GET,";
            foreach(Attack a in get_attacks){
                save_atks += a.GetName() + ",";
            }
            save_atks = save_atks.Remove(save_atks.Length - 1, 1);

            base_info["additional"] = save_atks;

            return base_info;
        }


        public Attack[] GetGetAttacks(){
            return get_attacks;
        }
        public int[] GetSelectAttacks(){
            return select_attacks;
        }

    }
}
