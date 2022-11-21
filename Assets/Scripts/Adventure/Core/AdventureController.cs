using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

#if UNITY_EDITOR
using UnityEditor;
#endif

// シーン移行時にJSON形式のAdventureCharacterのsaved_infoを保存
// シーン開始時にJSON形式でAdventureCharacterのsaved_infoを読み込み
namespace Adventure{
    public class AdventureController : MonoBehaviour{
        
        private AttackData atk_data;
        private AdventureData adv_data;
        
        [SerializeField]
        private bool SaveSceneInAwake = false;

        
#if UNITY_EDITOR
        private string InfoPath = "Assets/AdventurerInfo/";
#else
        private string InfoPath = Application.persistentDataPath + @"/";
#endif
        private List<GameObject> objects;



        
        private async void SaveSceneNow(){// FOR DEBUG
            objects = new List<GameObject>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject p in players){ objects.Add(p); }
            foreach(GameObject e in enemies){ objects.Add(e); }
            await SaveInfo("saved");
            Debug.Log("Saved in " + InfoPath + "saved.json");
        }


        async void Awake(){
            atk_data = GameObject.FindGameObjectWithTag("Attack Data").GetComponent<AttackData>();
            adv_data = GameObject.FindGameObjectWithTag("Adventure Data").GetComponent<AdventureData>();
            
            if(SaveSceneInAwake){
                SaveSceneNow();
                return;
            }
            string LoadInfoName = BattleToAdventure.SavedInfoName;
            
            // await Task.Delay(1000);
            await LoadInfo(LoadInfoName);
        }


        /*
        async void Start(){// repop enemy
            string scene_name = SceneManager.GetActiveScene().name;
            await Task.Delay(repop_time);


            if(scene_name != SceneManager.GetActiveScene().name){ return; }

            foreach(GameObject o in objects){
                if(!o.activeSelf){ o.SetActive(true); }
            }
        }
        */

        public async void SceneChange(string scene_name){
            
            await SaveInfo("RECENT");
            
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(scene_name, LoadSceneMode.Single);
            await handle.Task;
        }


        private async Task LoadInfo(string FileName){
            objects = new List<GameObject>();
            string json;
            
#if UNITY_EDITOR
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(@InfoPath + @FileName + @".json");
            TextAsset txt = await handle.Task;
            Debug.Log(FileName);
            json = txt.ToString();
#else
            StreamReader reader = new StreamReader(@InfoPath + @FileName + @".json");
            json = reader.ReadToEnd();
            reader.Close();
#endif

            
            Adventurers ads = JsonUtility.FromJson<Adventurers>(json);
            
            for(int i = 0;i < ads.info.Length;i++){
                await GenerateAdventurer(ads.info[i], i);
            }

        }

        private async Task SaveInfo(string FileName){
            string save_path = @InfoPath + @FileName + @".json";
            string pre_path = @InfoPath + @FileName + @"_.json";
            if(File.Exists(save_path)){
                File.Move(save_path, pre_path);
#if UNITY_EDITOR
                File.Move(save_path + ".meta", pre_path + ".meta");
#endif
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
            
            if(!File.Exists(save_path)){
                File.Move(pre_path, save_path);
            }
            
            AdventureToBattle.SavedInfoName = FileName;
            if(File.Exists(pre_path)){
                File.Delete(pre_path);
            }
#if UNITY_EDITOR
            File.Move(pre_path + ".meta", save_path + ".meta");
#endif

        }
        
        private async Task GenerateAdventurer(AdventurerInfo info, int i){// プレハブを指定位置に生成
            string path = adv_data.GetAdventurePath(info.name);
            Assert.IsFalse(path == null, "Cannot Find Adventurer: " + info.name);
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(path, new Vector3(i, i, i), Quaternion.identity);

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
