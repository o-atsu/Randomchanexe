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
APlayerクラスから攻撃一覧を取得し, 対応するカードを生成するクラス
    Enable時にカードのインスタンスを生成
*/
public class SortAttackCards : MonoBehaviour{
    [SerializeField]
    private string prefab_name = "AttackCard";
    [SerializeField]
    private bool set_attack = true;
    [SerializeField]
    private float width = 600.0f;
    [SerializeField]
    private Vector2 duration = new Vector2(210.0f, 210.0f);

    private APlayer aplayer;

    async void OnEnable(){
        aplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<APlayer>();

        Attack[] g_atks = aplayer.GetGetAttacks();

        if(set_attack){
            // 選択した攻撃一覧を表示
            int[] s_atks = aplayer.GetSelectAttacks();
            float columns = (float)s_atks.Length;
            for(int i = 0;i < s_atks.Length;i++){
                GameObject obj = await GenerateCard(g_atks[s_atks[i] - 1], (i + 1).ToString(), i + 1, true);

                Vector2 pos = new Vector2((i % columns) * (duration.x % width) - Mathf.Floor(columns / 2.0f) * duration.x, -Mathf.Floor((float)i / columns) * duration.y);// widthを超えるカードは折り返し
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }else{
            // 取得した攻撃一覧を表示
            float columns = Mathf.Min((float)g_atks.Length, Mathf.Floor(width / duration.x));
            for(int i = 0;i < g_atks.Length;i++){
                GameObject obj = await GenerateCard(g_atks[i], g_atks[i].GetName(), i + 1, false);

                Vector2 pos = new Vector2((i % columns) * (duration.x % width) - (float)(columns / 2) * duration.x, -Mathf.Floor((float)i / columns) * duration.y);// widthを超えるカードは折り返し
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
    }


    // 攻撃の情報からカードを生成
    private async Task<GameObject> GenerateCard(Attack atk, string name, int i, bool isset){

        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(prefab_name, transform);
        GameObject card = await handle.Task;
                
        AttackCard ac = card.GetComponent<AttackCard>();
        Assert.IsFalse(ac == null, "AttackCard Is Not Attached in " + atk.GetName());
        ac.init(atk.GetName(), name, atk.GetIcon(), i, isset);

        return card;
    }
}
