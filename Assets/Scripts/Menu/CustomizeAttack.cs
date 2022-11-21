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
            next = set_cards[card_id - 1].GetComponent<AttackCard>();
        }else{
            next = get_cards[card_id - 1].GetComponent<AttackCard>();
        }

        if(current != null){ current.OnDeselect(); }
        next.OnSelect();
        if(isset){
            s_index = card_id;
        }else{
            after = card_id;
        }
        current = next;

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
