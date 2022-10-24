using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure{
    public class APlayer : AdventureCharacter{
        [SerializeField]
        private List<string> player_names;
        [SerializeField]
        private List<posclass> player_pos;


        public override void init(AdventurerInfo info){
            base.init(info);
            
        }

        public List<string> GetPlayerNames(){ return player_names; }

        public List<int[]> GetPositions(){
            List<int[]> ret = new List<int[]>();
            for(int i = 0;i < player_pos.Count;i++){
                ret.Add(new int[2]{player_pos[i].coords[0], player_pos[i].coords[1]});
            }
            return ret;
        }

        public override Dictionary<string, string> SavedInfo(){
            Dictionary<string, string> base_info = base.SavedInfo();

            return base_info;
        }

    }
}
