using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Enemy.EnemyType type;
    public static List<EnemySprite> enemySprites;
    EnemySprite mySprite;
    public Image enemyImage;
    private Animator enemySelector;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Over!");
        enemyImage.sprite = enemySprites.Find(x => x.type == type).sprite;
        enemySelector.Play("Enemy_Status_Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit!");
        enemySelector.Play("Enemy_Status_Leave");
    }

    // Use this for initialization
    void Start () {
        enemySprites = GameObject.Find("Level1Container").GetComponent<Level1>().enemySprites;
        mySprite = LoadEnemySprite(type);
        enemySprites.Add(mySprite);
        enemySelector = GameObject.Find("Panel_EnemySelector").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public EnemySprite LoadEnemySprite(Enemy.EnemyType _type)
    {
        EnemySprite temp = new EnemySprite();
        if (_type == Enemy.EnemyType.type1)
        {
            temp.type = Enemy.EnemyType.type1;
            temp.sprite = Resources.Load<Sprite>("Sprites/e1");
        }

        else if (_type == Enemy.EnemyType.type2)
        {
            temp.type = Enemy.EnemyType.type2;
            temp.sprite = Resources.Load<Sprite>("Sprites/e2");
        }
            
        else //(type == Enemy.EnemyType.type3)
        {
            temp.type = Enemy.EnemyType.type3;
            temp.sprite = Resources.Load<Sprite>("Sprites/e3");
        }

        return temp;
    }
}
