using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private Transform allPlayerPanel;
    private AnimationControllerSupport animSupport;
    public bool autoChangePlayer;

    [Header("Animation Control")] private Animator battleStatusUI;

    //character image control
    private readonly Sprite[] characterImages = new Sprite[5];
    private Animator characterSelector;
    private Image characterStatusImage;

    [Header("Debug")] public int currentPlayer;

    private float elapsedTime;
    private float elapsedTime2;
    public int health = 100;

    //Status Bar Control
    private bool isEntered;
    public int LastPlayer;
    public int maxPlayer = 5;
    private Animator mission;
    public bool reduceHealth;

    private void Start()
    {
        animSupport = GameObject.Find("Panel_CharacterSelector").GetComponent<AnimationControllerSupport>();
        battleStatusUI = GameObject.Find("Canvas_BattleStatusUI").GetComponent<Animator>();
        characterSelector = GameObject.Find("Panel_CharacterSelector").GetComponent<Animator>();
        mission = GameObject.Find("Panel_Mission").GetComponent<Animator>();
        allPlayerPanel = GameObject.Find("Canvas_BattleStatusUI").transform.Find("Panel_AllPlayer");
        characterStatusImage = GameObject.Find("Panel_CharacterSelector").transform.GetChild(0).GetChild(0)
            .GetComponent<Image>();
        LoadCharacterSprites();
        isEntered = false;
    }

    private void Update()
    {
        //Debug - Player Input
        InputUpdate();

        //Debug - AutoChangePlayer
        if (autoChangePlayer)
            TestChangePlayer(2f); //change current player each second

        //Debug - ReduceHealth
        if (reduceHealth)
        {
            elapsedTime2 += Time.deltaTime;
            if (elapsedTime2 >= 1)
            {
                ChangeHealth(--health, currentPlayer);
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
            if (!animSupport.GetCurrentStatus())
                characterSelector.Play("IntoStatus");

        if (Input.GetKeyDown(KeyCode.F))
            if (!animSupport.GetCurrentStatus())
                characterSelector.Play("Status_Leave");

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
    private void ChangeHealth(int health, int player)
    {
        var i = allPlayerPanel.GetChild(player).Find("PlayerHP").GetComponent<Image>();
        var hp = (float) health / 100;
        i.fillAmount = hp;
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
}