using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/*
メニューを閉じるボタンにアタッチするクラス
*/
public class CloseMenu : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{

    [SerializeField]
    private string menu_scene = "AdventureMenu";

    private Animator anim;
    

    void Awake(){
        anim = gameObject.GetComponent<Animator>();
    }

    public async void OnPointerDown(PointerEventData eventData){
        // Debug.Log("Down:Menu");

        await Task.Delay(200);
        Pauser.Resume();
        await Task.Delay(100);
        SceneManager.UnloadSceneAsync(menu_scene);
    }

    public void OnPointerEnter(PointerEventData eventData){
        // Debug.Log("Enter:Menu");
        anim.SetBool("Highlight", true);
    }

    public void OnPointerExit(PointerEventData eventData){
        // Debug.Log("Exit:Menu");
        anim.SetBool("Highlight", false);
    }
}
