using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Adventure;


public class CustomizeAttack : MonoBehaviour{

    private List<GameObject> set_cards;
    private List<int> set_ids;
    private List<GameObject> get_cards;
    private List<int> get_ids;
    private APlayer aplayer;
    private AttackCard current;
    private int s_index;
    private int after;

    void Awake(){
        set_cards = new List<GameObject>();
        set_ids = new List<int>();
        get_cards = new List<GameObject>();
        get_ids = new List<int>();
        current = null;
        aplayer = GameObject.FindGameObjectWithTag("Player").GetComponent<APlayer>();
        s_index = after = -1;
    }

    private int IdToIndex(int i, bool isset){
        if(isset){
            return set_ids.IndexOf(i);
        }else{
            return get_ids.IndexOf(i);
        }
    }

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

    public void SelectCard(int card_id, bool isset){
        AttackCard next;
        if(isset){
            next = set_cards[IdToIndex(card_id, true)].GetComponent<AttackCard>();
        }else{
            next = get_cards[IdToIndex(card_id, false)].GetComponent<AttackCard>();
        }


        if(current != null){ current.OnDeselect(); }
        next.OnSelect();
        if(isset){
            s_index = IdToIndex(card_id, true);
        }else{
            after = card_id;
        }
        current = next;

        if(s_index != -1 && after != -1){
            AttackCard b = set_cards[s_index].GetComponent<AttackCard>();
            string a = get_cards[IdToIndex(after, false)].GetComponent<AttackCard>().GetText();
            aplayer.ChangeAttacks(s_index, after);
            b.OnChange(a);

            next.OnDeselect();
            current = null;
            s_index = after = -1;
        }
    }
}
