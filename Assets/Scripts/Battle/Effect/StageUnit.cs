using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*
攻撃の情報をマテリアルのfloatに知らせるスクリプト
    StageEffecterから該当する位置のユニットが呼び出される
    エフェクトの内容はシェーダーで記述
*/
namespace Battle{
    public class StageUnit : MonoBehaviour
    {
        [SerializeField]
        private int frequency = 4;// 等間隔に(攻撃時間 / frequency) 回だけマテリアルに値を設定

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
