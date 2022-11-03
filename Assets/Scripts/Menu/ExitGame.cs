using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MenuText{
    public override void OnSelect(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
