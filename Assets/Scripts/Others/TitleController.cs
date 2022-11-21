using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class TitleController : MonoBehaviour, IPointerDownHandler{
    
    private string initial_json = @"{
    ""info"": [
        {
            ""name"": ""RandomChan"",
            ""active"": ""True"",
            ""position_x"": ""0"",
            ""position_y"": ""0"",
            ""position_z"": ""-2"",
            ""rotation_x"": ""0"",
            ""rotation_y"": ""0"",
            ""rotation_z"": ""0"",
            ""rotation_w"": ""1"",
            ""additional"": ""SELECT,1,1,1,GET,パンチ""
        },
        {
            ""name"": ""2HandedWarrior"",
            ""active"": ""True"",
            ""position_x"": ""4.98"",
            ""position_y"": ""0"",
            ""position_z"": ""5.75"",
            ""rotation_x"": ""0"",
            ""rotation_y"": ""1"",
            ""rotation_z"": ""0"",
            ""rotation_w"": ""0"",
            ""additional"": """"
        },
        {
            ""name"": ""ArcherWarrior"",
            ""active"": ""True"",
            ""position_x"": ""5.13"",
            ""position_y"": ""4.07"",
            ""position_z"": ""16.12"",
            ""rotation_x"": ""0"",
            ""rotation_y"": ""0.7071068"",
            ""rotation_z"": ""0"",
            ""rotation_w"": ""0.7071068"",
            ""additional"": """"
        },
        {
            ""name"": ""AERandomChan"",
            ""active"": ""True"",
            ""position_x"": ""-10.1"",
            ""position_y"": ""8.09"",
            ""position_z"": ""8.65"",
            ""rotation_x"": ""0"",
            ""rotation_y"": ""0.7071068"",
            ""rotation_z"": ""0"",
            ""rotation_w"": ""0.7071068"",
            ""additional"": """"
        },
        {
            ""name"": ""KnightWarrior"",
            ""active"": ""True"",
            ""position_x"": ""11.57"",
            ""position_y"": ""0.38"",
            ""position_z"": ""10.32"",
            ""rotation_x"": ""0"",
            ""rotation_y"": ""-0.7071068"",
            ""rotation_z"": ""0"",
            ""rotation_w"": ""0.7071068"",
            ""additional"": """"
        },
        {
            ""name"": ""MageWarrior"",
            ""active"": ""True"",
            ""position_x"": ""13.67"",
            ""position_y"": ""0.46"",
            ""position_z"": ""16.95"",
            ""rotation_x"": ""0"",
            ""rotation_y"": ""1"",
            ""rotation_z"": ""0"",
            ""rotation_w"": ""0"",
            ""additional"": """"
        }
    ]
}";

    private bool initialized;
    
    [SerializeField]
    private GameObject clicktostart;
    
    [SerializeField]
    private AudioClip click_se;
    
    
    async Task Awake(){
#if UNITY_EDITOR
        initialized = true;
        clicktostart.SetActive(true);
#else
        initialized = false;
        
        string InfoPath = Application.persistentDataPath;
        StreamWriter writer = File.CreateText(@InfoPath + @"/INITIAL.json");
        await writer.WriteAsync(initial_json);
        
        writer.Flush();
        writer.Close();
        initialized = true;
        clicktostart.SetActive(true);
#endif
    }
    
    public void OnPointerDown(PointerEventData eventData){
        if(initialized){
            GetComponent<AudioSource>().PlayOneShot(click_se);
            SceneManager.LoadScene("InitialLoading", LoadSceneMode.Additive);
        }
    }
}
