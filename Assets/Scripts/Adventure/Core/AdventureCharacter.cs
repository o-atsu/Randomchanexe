using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure{
    public class AdventureCharacter : MonoBehaviour{

        [SerializeField]
        private bool saved = true;
        [SerializeField]
        private string prefab_name = "for_test";
        protected Dictionary<string, string> saved_component = new Dictionary<string, string>();

        public virtual void init(AdventurerInfo info){
            prefab_name = info.name;
            Vector3 pos = new Vector3(float.Parse(info.position_x), float.Parse(info.position_y), float.Parse(info.position_z));
            // Debug.Log(pos);
            transform.position = pos;
            transform.rotation.Set(float.Parse(info.rotation_x), float.Parse(info.rotation_y), float.Parse(info.rotation_z), float.Parse(info.rotation_w));
            gameObject.SetActive(System.Convert.ToBoolean(info.active));
        }
    
        public virtual Dictionary<string, string> SavedInfo(){// 保存する情報の更新
            if(!saved){ return null; }

            saved_component.Add("name", prefab_name);
            saved_component.Add("active", true.ToString());
            saved_component.Add("position_x", transform.position.x.ToString());
            saved_component.Add("position_y", transform.position.y.ToString());
            saved_component.Add("position_z", transform.position.z.ToString());
            saved_component.Add("rotation_x", transform.rotation.x.ToString());
            saved_component.Add("rotation_y", transform.rotation.y.ToString());
            saved_component.Add("rotation_z", transform.rotation.z.ToString());
            saved_component.Add("rotation_w", transform.rotation.w.ToString());
            saved_component.Add("additional", "");

            return saved_component;
        }

        // for inspector
        [System.SerializableAttribute]
        public class posclass{
            public int[] coords = {0, 0};
        }
    }
}
