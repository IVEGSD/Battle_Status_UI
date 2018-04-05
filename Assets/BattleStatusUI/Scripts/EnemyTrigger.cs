using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Enemy.EnemyType type;
    public static List<EnemySprite> enemySprites = enemySprites = new List<EnemySprite>();
    EnemySprite mySprite;
    public Image enemyImage;
    private Animator enemySelector;

    //Mouse Over the Button
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Over!");
        enemyImage.sprite = enemySprites.Find(x => x.type == type).sprite;
        enemySelector.Play("Enemy_Status_Enter");
    }

    //Mouse Exit the Button
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit!");
        enemySelector.Play("Enemy_Status_Leave");
    }

    void Start() {
        mySprite = LoadEnemySprite(type);
        enemySprites.Add(mySprite);
        enemySelector = GameObject.Find("Panel_EnemySelector").GetComponent<Animator>();
    }
    
    //Load the Sprite by its type
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
