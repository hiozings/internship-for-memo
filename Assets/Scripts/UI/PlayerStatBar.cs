using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public List<Sprite> healthSprites;

    public Image buffImage;
    public List<Sprite> buffSprites;

    public void OnHealthChange(int health)
    {
        switch (health)
        {
            case 0:
                healthImage.sprite = healthSprites[0];
                break;
            case 1:
                healthImage.sprite = healthSprites[1];
                break;
            case 2:
                healthImage.sprite = healthSprites[2];
                break;
            case 3:
                healthImage.sprite = healthSprites[3];
                break;
            default:
                break;
        }

    }

    public void OnBuffChange(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.Nobuff:
                buffImage.sprite = buffSprites[0];
                break;
            case BuffType.Fly:
                buffImage.sprite = buffSprites[1];
                break;
            case BuffType.Invul:
                buffImage.sprite = buffSprites[2];
                break;
            case BuffType.SpeedUp:
                buffImage.sprite = buffSprites[3];
                break;
            case BuffType.Health:
                buffImage.sprite = buffSprites[0];
                break;
            default:
                break;
        }
    }
}
