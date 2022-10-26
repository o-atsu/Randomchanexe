using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

#if UNITY_EDITOR
using UnityEditor;
#endif

// シーン移行時にJSON形式のAdventureCharacterのsaved_infoを保存
// シーン開始時にJSON形式でAdventureCharacterのsaved_infoを書き出し
namespace Adventure{
    public class AdventureController : MonoBehaviour{
        
        
        [SerializeField]
        private int repop_time = 10000;// milliseconds
        [SerializeField]
        private bool SaveSceneInAwake = false;

        private string InfoPath = "Assets/AdventurerInfo/";
        private List<GameObject> objects;



        
        private async void SaveSceneNow(){// FOR DEBUG
            objects = new List<GameObject>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(players);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject p in players){ objects.Add(p); }
            foreach(GameObject e in enemies){ objects.Add(e); }
            await SaveInfo("saved");
        }


        async void Awake(){
            
            if(SaveSceneInAwake){
                SaveSceneNow();
                return;
            }
            string LoadInfoName = BattleToAdventure.SavedInfoName;
            
            // await Task.Delay(1000);
            await LoadInfo(LoadInfoName);
        }


        async void Start(){// repop enemy
            string scene_name = SceneManager.GetActiveScene().name;
            await Task.Delay(repop_time);


            if(scene_name != SceneManager.GetActiveScene().name){ return; }
            foreach(GameObject o in objects){
                if(!o.activeSelf){ o.SetActive(true); }
            }
        }

        public async void SceneChange(string scene_name){
            
            await SaveInfo("RECENT");
            
            SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
        }


        private async Task LoadInfo(string FileName){
            objects = new List<GameObject>();
            
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(@InfoPath + @FileName + @".json");
            TextAsset txt = await handle.Task;
            Debug.Log(txt);
            string json = txt.ToString();

            
            Adventurers ads = JsonUtility.FromJson<Adventurers>(json);
            
            foreach(AdventurerInfo info in ads.info){
                await GenerateAdventurer(info);
            }
        }

        private async Task SaveInfo(string FileName){
            string save_path = @InfoPath + @FileName + @".json";
            string pre_path = @InfoPath + @FileName + @"_.json";
            if(File.Exists(save_path)){
                File.Move(save_path, pre_path);
            }
        
            int num_ads = objects.Count;
            Adventurers ads = new Adventurers();
            ads.info = new AdventurerInfo[num_ads];
            
            int i = 0;
            foreach(GameObject chara in objects){
                // Debug.Log(chara);
                Dictionary<string, string> c_info = chara.GetComponent<AdventureCharacter>().SavedInfo();
                // Debug.Log(c_info.Count);
                ads.info[i] = new AdventurerInfo(c_info);

                
                i++;
            }
            
            string json = JsonUtility.ToJson(ads, true);
            StreamWriter writer = new StreamWriter(@save_path);
            await writer.WriteAsync(json);
            writer.Flush();
            writer.Close();
            
            Assert.IsTrue(File.Exists(save_path), "Cannot saved " + save_path);
            
            AdventureToBattle.SavedInfoName = FileName;
            if(File.Exists(pre_path)){
                File.Delete(pre_path);
            }

        }
        
        private async Task GenerateAdventurer(AdventurerInfo info){// プレハブを指定位置に生成
            string path = AdventureData.GetAdventurePath(info.name);
            Assert.IsFalse(path == null, "Cannot Find Adventurer: " + info.name);
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path);

            GameObject chara = await handle.Task;
                
            AdventureCharacter adventurer = chara.GetComponent<AdventureCharacter>();
            Assert.IsFalse(adventurer == null, "AdventureCharacter Is Not Attached in " + info.name);
            adventurer.init(info);

            objects.Add(chara);
        }
        
    }


    [System.Serializable]
    public class AdventurerInfo{
        public string name;// プレハブ名
        public string active;// (非)アクティブ
        public string position_x;// ワールド位置
        public string position_y;
        public string position_z;
        public string rotation_x;// 回転（quaternion）
        public string rotation_y;
        public string rotation_z;
        public string rotation_w;
        public string additional;// 派生クラスに必要な情報
        
        public AdventurerInfo(Dictionary<string, string> c_info){
            name = c_info["name"];
            active = c_info["active"];
            position_x = c_info["position_x"];
            position_y = c_info["position_y"];
            position_z = c_info["position_z"];
            rotation_x = c_info["rotation_x"];
            rotation_y = c_info["rotation_y"];
            rotation_z = c_info["rotation_z"];
            rotation_w = c_info["rotation_w"];
            additional = c_info["additional"];
        }
    }

    [System.Serializable]
    public class Adventurers{
        public AdventurerInfo[] info;
    }


}
