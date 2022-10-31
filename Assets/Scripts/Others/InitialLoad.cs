using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialLoad : MonoBehaviour{
    
    void Start(){
        BattleToAdventure.SavedInfoName = "INITIAL";
        SceneManager.LoadScene("Playground", LoadSceneMode.Single);
    }
}
