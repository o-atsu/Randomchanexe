using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Adventure;


/*
攻撃入れ替えを実行するクラス
    カードの選択時にIDをs_indexかafterに代入
        どちらも代入されたら入れ替え実行
*/
public class CustomizeAttack : MonoBehaviour{

    private List<GameObject> set_cards;
    private List<int> set_ids;
    private List<GameObject> get_cards;
    private List<int> get_ids;
    private APlayer aplayer;
    private AttackCard current;
    private int s_index; // 選択した攻撃のindex(セット済み)
    private int after; // 選択した攻撃のindex(未セット)


    // アクティブになったら初期化
    void OnEnable(){
        set_cards = new List<GameObject>();
        set_ids = new List<int>();
        get_cards = new List<GameObject>();
        get_ids = new List<int>();
        current = null;
        aplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<APlayer>();
        s_index = after = -1;
    }

    void OnDisable(){
        current = null;
        s_index = after = -1;
    }

    // カード生成時にAttackCardから呼び出してリストに登録
    public void SubscribeCard(GameObject card, bool isset){
        if(isset){
            set_cards.Add(card);
            set_ids.Add(card.GetComponent<AttackCard>().GetId());
            // Debug.Log(card.GetComponent<AttackCard>().GetId());
        }else{
            get_cards.Add(card);
            get_ids.Add(card.GetComponent<AttackCard>().GetId());
            // Debug.Log(card.GetComponent<AttackCard>().GetId());
        }
    }

    // カード選択時の処理
    public void SelectCard(int card_id, bool isset){
        AttackCard next;
        if(isset){
            next = set_cards[card_id - 1].GetComponent<AttackCard>();
            s_index = card_id;
        }else{
            next = get_cards[card_id - 1].GetComponent<AttackCard>();
            after = card_id;
        }

        if(current != null){ current.OnDeselect(); }
        next.OnSelect();
        current = next;

        // s_indexとafterがともに代入されたら入れ替えを実行
        if(s_index != -1 && after != -1){
            AttackCard b = set_cards[s_index - 1].GetComponent<AttackCard>();
            AttackCard a = get_cards[after - 1].GetComponent<AttackCard>();
            aplayer.ChangeAttacks(s_index, after);
            set_ids[s_index - 1] = after;
            b.OnChange(a.GetIcon());

            next.OnDeselect();
            current = null;
            s_index = after = -1;
        }
    }
}
