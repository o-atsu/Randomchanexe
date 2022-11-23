using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Adventure;

/*
Start時に初回読み込みをし, 探索パートに移行するクラス
    探索パートのシーン名とAdventureInfoを指定
*/
public class InitialLoad : MonoBehaviour{

    [SerializeField]
    private bool load_scene = true;
    [SerializeField]
    private string adv_scene = "Playground";
    [SerializeField]
    private string load_info = "INITIAL";
    [SerializeField]
    private bool show_help = true;
    
    async void Start(){

        AttackData atk_data = GameObject.FindGameObjectWithTag("Attack Data").GetComponent<AttackData>();
        BattleToAdventure.SavedInfoName = load_info;
        AdventureToBattle.SavedInfoName = load_info;
        AdventureController.initial_load = show_help;
        await atk_data.Refresh();
        
        if(load_scene){
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(adv_scene, LoadSceneMode.Single);
            await handle.Task;
        }
    }
}
