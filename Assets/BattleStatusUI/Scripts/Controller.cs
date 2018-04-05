using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    //Animation Control
    private Animator battleStatusUI;
    private Animator mission;
    private Animator characterSelector;
    private AnimationControllerSupport animSupport;
    private Animator enemySelector;

    //Character Image Control
    private Transform allPlayerPanel;
    private readonly Sprite[] characterImages = new Sprite[5];
    private Image characterStatusImage;
    //Enemy
    private Image EnemyStatusImage;
    public int emenyNumber;
    //Debug - List
    List<EnemySprite> enemySprites;

    //Upper Status Bar Control
    private Text roundNumberText;
    

    [Header("Debug")]
    public int currentPlayer;
    public int LastPlayer;
    public float elapsedTime;
    public float elapsedTime2;
    public int health = 100;

    //Left【Tab】Control
    private bool isEntered;
    public int maxPlayer = 5;
    public bool autoReduceHealth;
    public bool autoChangePlayer;
    public int roundNumber = 1;

    


    /* 有用Method：
     * ChangePlayer(int player)
     * ChangeHealth(int health, int player) - 用之前要自己先get 個player嘅 health，
     *                                         - 有5個player你就要自己開5個 health variable
     *                                             - 你可以改咗個method先get 個fillAmount，再改個health
     * ChangeMana(int mana, int player) - same as above
     * ChangeRound(int r) - 入乜show乜
    */

    private void Start()
    {
        animSupport = GameObject.Find("Panel_CharacterSelector").GetComponent<AnimationControllerSupport>();
        battleStatusUI = GameObject.Find("Canvas_BattleStatusUI").GetComponent<Animator>();
        characterSelector = GameObject.Find("Panel_CharacterSelector").GetComponent<Animator>();
        enemySelector = GameObject.Find("Panel_EnemySelector").GetComponent<Animator>();
        mission = GameObject.Find("Panel_Mission").GetComponent<Animator>();
        allPlayerPanel = GameObject.Find("Canvas_BattleStatusUI").transform.Find("Panel_AllPlayer");
        characterStatusImage = GameObject.Find("Panel_CharacterSelector").transform.GetChild(0).GetChild(0)
            .GetComponent<Image>();
        LoadCharacterSprites();
        isEntered = false;
        roundNumberText = GameObject.Find("Panel_BattleStatusBar").GetComponentInChildren<Text>();

        //Debug - List
        enemySprites = LoadEnemySprite(GameObject.Find("Level1Container").GetComponent<Level1>().enemyList);
    }

    private void Update()
    {
        //Debug - Player Input
        InputUpdate();

        //Debug - AutoChangePlayer
        if (autoChangePlayer)
        {
            TestChangePlayer(2f); //change current player each second
            ChangeRound(++roundNumber);
        }
            

        //Debug - ReduceHealth
        if (autoReduceHealth)
        {
            elapsedTime2 += Time.deltaTime;
            if (elapsedTime2 >= 1)
            {
                ChangeHealth(--health, currentPlayer);
                ChangeMana((health-=5), currentPlayer);
                elapsedTime2 = 0;
            }
        }

        //highlight the current player
        CurrentPlayerAnimation(currentPlayer);
    }

    //Player Input
    private void InputUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A)) battleStatusUI.Play("IntoBattle");

        if (Input.GetKeyDown(KeyCode.S)) battleStatusUI.Play("LeaveBattle");

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!animSupport.GetCurrentStatus())
                characterSelector.Play("IntoStatus");
            enemySelector.Play("Enemy_Status_Enter");
        }
            

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!animSupport.GetCurrentStatus())
                characterSelector.Play("Status_Leave");
            enemySelector.Play("Enemy_Status_Leave");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            battleStatusUI.Play("IntoBattle");
            if (!animSupport.GetCurrentStatus())
                characterSelector.Play("IntoStatus");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            battleStatusUI.Play("LeaveBattle");
            if (!animSupport.GetCurrentStatus())
                characterSelector.Play("Status_Leave");
        }
    }

    //左方【tab】上的紅色border
    private void CurrentPlayerAnimation(int player)
    {
        var i = allPlayerPanel.GetChild(player).GetComponent<Image>();
        i.color = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.time, 0.5f));

        //change the last player's border to black
        if (player != LastPlayer)
        {
            allPlayerPanel.GetChild(LastPlayer).GetComponent<Image>().color = Color.black;
            LastPlayer = player;
        }
    }

    //用來改變Player在左方【Tab】上的Health bar
    public void ChangeHealth(int health, int player)
    {
        //due to different player contains different HP and MP Image, so get component here
        var i = allPlayerPanel.GetChild(player).Find("PlayerHP").GetComponent<Image>();
        var hp = (float)health / 100;  //change to decimal number
        i.fillAmount = hp;
    }

    public void ChangeMana(int mana, int player)
    {
        var i = allPlayerPanel.GetChild(player).Find("PlayerMP").GetComponent<Image>();
        var mp = (float)mana / 100;
        i.fillAmount = mp;
    }

    //自動轉【currentPlayer】，用於testing
    private void TestChangePlayer(float t)
    {
        //Timer for change player
        elapsedTime += Time.deltaTime;
        if (elapsedTime < t) return;
        currentPlayer = (currentPlayer + 1) % 5;
        elapsedTime = 0;
    }

    //改變player的sprite，此method會被 ChangePlayer() call
    public void ChangePlayerImage(int player)
    {
        characterStatusImage.sprite = characterImages[player];
    }

    //預先Load定所有character的sprite
    public void LoadCharacterSprites()
    {
        characterImages[0] = Resources.Load<Sprite>("Sprites/p1");
        characterImages[1] = Resources.Load<Sprite>("Sprites/p2");
        characterImages[2] = Resources.Load<Sprite>("Sprites/p1");
        characterImages[3] = Resources.Load<Sprite>("Sprites/p2");
        characterImages[4] = Resources.Load<Sprite>("Sprites/p1");
    }
    
    public List<EnemySprite> LoadEnemySprite(List<Enemy> enemyList)
    {
        List<EnemySprite> temp = new List<EnemySprite>();
        if (enemyList.Exists(x => x.Type == Enemy.EnemyType.type1))
            temp.Add(new EnemySprite() { type = Enemy.EnemyType.type1, sprite = Resources.Load<Sprite>("Sprites/e1") });
        if (enemyList.Exists(x => x.Type == Enemy.EnemyType.type2))
            temp.Add(new EnemySprite() { type = Enemy.EnemyType.type2, sprite = Resources.Load<Sprite>("Sprites/e2") });
        if (enemyList.Exists(x => x.Type == Enemy.EnemyType.type3))
            temp.Add(new EnemySprite() { type = Enemy.EnemyType.type3, sprite = Resources.Load<Sprite>("Sprites/e3") });
        return temp;
    }

    //改變【currentPlayer】，【狀態欄的圖片】，以及【標示(紅色border)】住左方Player的Tab
    public void ChangePlayer(int player)
    {
        currentPlayer = player;
        ChangePlayerImage(player);
        characterSelector.Play("IntoStatus");
    }

    //Show 【Mission Panel】
    public void ShowMission()
    {
        if (!isEntered)
        {
            mission.Play("Mission_Enter");
            isEntered = true;
        }
        else
        {
            mission.Play("Mission_Leave");
            isEntered = false;
        }
    }

    //用來改Round Number
    public void ChangeRound(int r)
    {
        roundNumberText.text = "Round : " + r; 
    }
    
}

[System.Serializable]
public class EnemySprite
{
    public Enemy.EnemyType type;
    public Sprite sprite;
}