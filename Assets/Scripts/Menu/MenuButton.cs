using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler{

    [SerializeField]
    private string menu_scene = "AdventureMenu";

    private Animator anim;
    

    void Awake(){
        anim = gameObject.GetComponent<Animator>();
    }

    public void OnPointerDown(PointerEventData eventData){
        // Debug.Log("Down:Menu");
        SceneManager.LoadScene(menu_scene, LoadSceneMode.Additive);
        anim.SetBool("Highlight", false);
        Pauser.Pause();
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
