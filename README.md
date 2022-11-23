# らんだむちゃんEXE



## 概要

Unityを用いて制作したアクションゲームです. 
敵を倒すことで新たな攻撃を獲得し, より強い敵に挑んでいきます. 


- 製作期間：約1ヶ月
- 制作：個人
- Unityのバージョン：2021.3.6f1
- レンダーパイプライン：URP 12.1.7



## 操作

マウスとキーボードで操作します. 

### 探索パート
- 移動  

W：↑, A：←, S：回転, D：→  

Space：ジャンプ

### 戦闘パート
- 移動  

W：↑, A：←, S：↓, D：→

- 攻撃  

1, 2, 3 またはボタンをクリック



## エディタ画面でのプレイについて

Assets/Scenes/ForDebug/TestLoad シーンを実行することで, テスト用の探索パートがロードされます. 
同様に, Assets/Scenes/Title/InitialLoading シーンを実行することで, 初回起動時の探索パートがロードされます. 
Assets/Scenes/Adventure/ 及び, Assets/Scenes/Battle/ にあるシーン内で実行しても, エラーが出てプレイできないのでご注意ください. 



## 注力した点

エネミーの追加や攻撃の調整など, コンテンツの充実化がしやすいようにクラス設計しました. 
攻撃は, AttackクラスのScriptableObjectを生成し, Attack Dataのパラメータにプレハブ名を追加することで増やせます. 
また, 敵キャラクターはFEnemy.csを継承したクラスでルーチンのみを実装し, Assets/Prefabs/Battle/内に置いたプレハブにアタッチ, Battle Dataにプレハブ名を追加することで増やせます. 
これによりゲームの開発効率を向上させることで, ゲーム全体のボリューム増加と最適なレベル調整によるコンテンツのクオリティ向上, ユーザの体験の向上が実現できると考えています. 



## プロジェクト構成

Assets/AdventurerInfo/ : 探索パートでのキャラクター情報をjsonファイルで保存するディレクトリ


Assets/AnimatorController/ : Animator Controllerを置くディレクトリ


Assets/Prefabs/ : Prefab, 及びそれに適用するシェーダー等を置くディレクトリ  
- Adventure/ : 探索パートで使用するキャラクター  
- Attacks/ : 攻撃に使用する武器やエフェクト  
- Battle/ : 戦闘パートで使用するキャラクター  
- BattleStage/ : 戦闘パートのステージに使用する床  
- Title/ : タイトル画面で使用するキャラクター  
- UI/ : アイコン等UIに使用するオブジェクト  


Assets/Scenes/ : シーンを置くディレクトリ  
- Adventure/ : 探索パートのシーンとそのメニューシーン  
- Battle/ : 戦闘パートのシーンとそのメニューシーン  
- ForDebug/ : デバッグ用の情報をロードするシーン  
- Title/ : タイトル, 初回ロード時のシーン  


Assets/ScriptableObjects/Attacks/ : 攻撃のScriptable Objectを置くディレクトリ  


Assets/Scripts/ : スクリプトを置くディレクトリ  
- Adventure/Core/ : 探索パートに使用するスクリプト  
    - AdventureCharacter.cs : キャラクターにアタッチするクラス（派生：AEnemy.cs, APlayer.cs）  
    - AdventureController.cs : 探索シーンを統括するクラス  

- Battle/ : 戦闘パートに使用するスクリプト  
    - Core/  
        - BattleController.cs : 戦闘シーンを統括するクラス  
        - Fighter.cs : 戦闘キャラクターにアタッチするクラス（派生：FPlayer.cs, FEnemy.cs）  
        - Fighters/ : 自機, 敵キャラクターの挙動を実装するスクリプト  
            - FPlayer.cs : 自機の動きを制御するクラス  
            - FEnemy.cs : 敵の動きを制御するクラス（派生：F2Handed.cs, FArcher.cs, FKnight.cs, FMage.cs, FRandomChan.cs）  
    - Effect/ : エフェクトのために使用するスクリプト  
        - StageEffector.cs : ステージのエフェクトを統括するクラス  
        - StageUnit.cs : StageUnitプレハブのマテリアルに値を設定するクラス  
    - ScriptableObjects/Attacks.cs : 攻撃のScriptableObjectを実装するクラス  
    - UI/ : UIのためのクラス  
        - AttackButton.cs : 攻撃ボタンにアタッチするクラス  
        - NewAttack.cs : 勝利時に獲得した攻撃のアイコンを適用するクラス  
        - SortButtons.cs : 攻撃ボタンを生成するクラス  

- Menu/ : メニュー画面に使用するスクリプト  
    - AttackCard.cs : Customizeで表示する攻撃カードにアタッチするクラス  
    - CloseMenu.cs : メニューシーンを閉じるクラス  
    - CustomizeAttack.cs : カードの入れ替えを実行するクラス  
    - MenuButton.cs : メニューシーンを開くクラス  
    - MenuController.cs : メニュー画面を統括するクラス  
    - MenuText.cs : メニューの選択肢にアタッチするクラス（派生：ShowPanel.cs, ExitGame.cs）  
    - SortAttackCards.cs : Customizeで攻撃カードを生成するクラス  
    - SoundSlider.cs : AudioMixerとスライダーの値を同期するクラス  

- Others/ : タイトル, ロードシーンなどに使用するスクリプト  
    - AssetData.cs : プレハブ名とAddressableのパス名を対応させるクラス  
    - AttackData.cs : 攻撃のScriptableObject名と中身を対応させるクラス  
    - InitialLoad.cs : 初回ロードと設定をし, 探索パートへ移行するクラス  
    - TitleController.cs : タイトル画面を統括するクラス  

- Static/ : 主にシーン間の値の受け渡しをする静的クラス  
    - AdventureToBattle.cs : 探索シーンから戦闘シーンへ値を渡すクラス  
    - BattleToAdventure.cs : 戦闘シーンから探索シーンへ値を渡すクラス  


Assets/UIIcons/ : アイコン等UIに使用するスプライト

