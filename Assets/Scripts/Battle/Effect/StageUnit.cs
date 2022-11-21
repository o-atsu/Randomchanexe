using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle{
    public class StageUnit : MonoBehaviour
    {
        [SerializeField]
        private int frequency = 4;

        private Material mat;

        void Awake(){
            mat = GetComponent<Renderer>().material;
        }
    
        public async void Startup(int time){
            int await_time = time / frequency / 2;
            for(int i = 0;i < frequency - 1;i++){
                mat.SetFloat("_StartUp", 1.0f);
                await Task.Delay(await_time);

                mat.SetFloat("_StartUp", 0.0f);
                await Task.Delay(await_time);
            }

            mat.SetFloat("_Attacked", 1.0f);
            await Task.Delay(await_time);
            mat.SetFloat("_Attacked", 0.0f);
        }
    }
}
