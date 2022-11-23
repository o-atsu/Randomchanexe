using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Adventure;

/*
AdventureToBattleから攻撃一覧を取得し, 対応する攻撃ボタンを生成するクラス
    Enable時にボタンのインスタンスを生成
*/
public class SortButtons : MonoBehaviour{
    [SerializeField]
    private string prefab_name = "AttackButton";
    [SerializeField]
    private float width = 800.0f;
    [SerializeField]
    private Vector2 duration = new Vector2(210.0f, 210.0f);


    async void Awake(){

        Attack[] s_atks = AdventureToBattle.select_attacks;

        while(GameObject.FindGameObjectWithTag("Player") == null){
            await Task.Delay(10);
        }

        float columns = (float)s_atks.Length;
        for(int i = 0;i < s_atks.Length;i++){
            GameObject obj = await GenerateCard(s_atks[i], (i + 1).ToString(), i + 1);

            Vector2 pos = new Vector2((i % columns) * (duration.x % width) - Mathf.Floor(columns / 2.0f) * duration.x, -Mathf.Floor((float)i / columns) * duration.y);// widthを超えるカードは折り返し
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
        }

        Debug.Log("Attacks: " + s_atks[0].GetName() + ", " + s_atks[1].GetName() + ", " + s_atks[2].GetName());
    }


    private async Task<GameObject> GenerateCard(Attack atk, string name, int i){

        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(prefab_name, transform);
        GameObject card = await handle.Task;
                
        AttackButton ab = card.GetComponent<AttackButton>();
        Assert.IsFalse(ab == null, "AttackButton Is Not Attached in " + atk.GetName());
        
        while(GameObject.FindGameObjectWithTag("Player") == null){
            await Task.Delay(100);
        }
        ab.init(atk.GetName(), name, atk.GetIcon(), i);

        return card;
    }
}
