using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Battle{
    public class FArcher : FEnemy{

        [SerializeField]
        private int offset = 200;

        protected override async Task Pattern(){
            await Task.Delay(offset);
            while(gameObject.activeSelf){

                await CheckScene();
                await Task.Delay(cooltime);
                await AttackEvent(1);

            }
        }

    }   

}
