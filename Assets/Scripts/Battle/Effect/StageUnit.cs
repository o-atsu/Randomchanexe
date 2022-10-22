using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle{
    public class StageUnit : MonoBehaviour
    {
        private Material mat;

        void Awake(){
            mat = GetComponent<Renderer>().material;
        }
    
        public async void Startup(int time){
            Color pre = mat.GetColor("_BaseColor");
            mat.SetColor("_BaseColor", Color.red);

            await Task.Delay(time);

            mat.SetColor("_BaseColor", pre);
        }
    }
}
