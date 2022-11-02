using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class AttackCard : MonoBehaviour{

    private Dictionary<string, string> icon_file = new Dictionary<string, string>(){
        {"大剣","Sword"},
        {"パンチ","Punch"},
        {"ビーム","Beam"},
        {"Xビット","Xbit"},
        {"ファイアーブレード","Fireblade"}
    };

    private int icon_id;


    public async void init(string atk, string name, int i){// For select_attacks
        Image img = transform.GetChild(0).gameObject.GetComponent<Image>();
        TextMeshProUGUI tmp = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(IconData.GetIconPath(icon_file[atk]));
        Sprite icon = await handle.Task;

        img.sprite = icon;
        tmp.text = name;
        icon_id = i;
    }

    public int GetId(){ return icon_id; }
}
