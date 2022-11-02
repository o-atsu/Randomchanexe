using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class AttachPlayerFollow : MonoBehaviour{

    private string player_tag = "Player";

    private CinemachineVirtualCamera vcam;


    async void Start(){

        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
        GameObject player = null;


        while(player == null){
            await Task.Delay(100);

            player = GameObject.FindGameObjectWithTag(player_tag);
            if(player != null){
                vcam.Follow = player.transform;
                vcam.LookAt = player.transform;
            }
        }
        
    }

}
