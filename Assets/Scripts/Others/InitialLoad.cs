using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class InitialLoad : MonoBehaviour{

    [SerializeField]
    private bool load_scene = true;
    [SerializeField]
    private string adv_scene = "Playground";
    [SerializeField]
    private string load_info = "INITIAL";
    
    async void Start(){

        AttackData atk_data = GameObject.FindGameObjectWithTag("Attack Data").GetComponent<AttackData>();
        await atk_data.Refresh();
        
        if(load_scene){
            BattleToAdventure.SavedInfoName = load_info;
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(adv_scene, LoadSceneMode.Single);
            await handle.Task;
        }
    }
}
