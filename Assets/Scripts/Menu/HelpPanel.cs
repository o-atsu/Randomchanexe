using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/*
操作説明パネルにアタッチするクラス
    画面タッチすると非アクティブになる
*/
public class HelpPanel : MonoBehaviour, IPointerDownHandler{
    
    [SerializeField]
    private bool pause = false;

    void OnEnable(){
        if(pause){ Pauser.Pause(); }
    }

    public async void OnPointerDown(PointerEventData eventData){
        // Debug.Log("Down:Menu");

        await Task.Delay(200);
        if(pause){ Pauser.Resume(); }
        await Task.Delay(100);
        gameObject.SetActive(false);
    }

}
