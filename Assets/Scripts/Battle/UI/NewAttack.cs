using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using Battle;

public class NewAttack : MonoBehaviour{




    [SerializeField]
    private int reward_index = 0;



    public void OnEnable(){
        if(BattleToAdventure.RewardAttacks.Count <= reward_index){
            gameObject.SetActive(false);
            return;
        }

        Attack atk = BattleToAdventure.RewardAttacks[reward_index];

        Image img = transform.GetChild(1).gameObject.GetComponent<Image>();
        TextMeshProUGUI tmp = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();


        img.sprite = atk.GetIcon();
        tmp.text = name;
    }

}
