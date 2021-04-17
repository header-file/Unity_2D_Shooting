using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.Inst().EquManager = gameObject.GetComponent<EquipManager>();
    }

    public void Count(GameObject actor, int index, int id)
    {
        float time = GameManager.Inst().Player.GetItem(index).CoolTime -= Time.deltaTime;

        if(time <= 0.0f)
            Activate(actor, index, id);
    }

    void Activate(GameObject actor, int index, int id)
    {
        switch(GameManager.Inst().Player.GetItem(index).Type)
        {
            case (int)Item_ZzinEquipment.EquipType.MAGNET:
                Item_Resource[] resources = FindObjectsOfType<Item_Resource>();
                for(int i = 0; i < resources.Length; i++)
                {
                    if (resources[i].gameObject.activeSelf)
                        resources[i].BeginAbsorb();
                }
                break;

            case (int)Item_ZzinEquipment.EquipType.HOMING:
                GameManager.Inst().ShtManager.Shoot(Bullet.BulletType.EQUIP, actor, id, false, false);
                break;

            case (int)Item_ZzinEquipment.EquipType.HEAL:
                if(id == 2)
                    actor.GetComponent<Player>().Heal((int)GameManager.Inst().Player.GetItem(index).Value);
                else
                    actor.GetComponent<SubWeapon>().Heal((int)GameManager.Inst().Player.GetItem(index).Value);
                break;

            case (int)Item_ZzinEquipment.EquipType.SHIELD:
                if (id == 2)
                    actor.GetComponent<Player>().RestoreShield((int)GameManager.Inst().Player.GetItem(index).Value);
                else
                    actor.GetComponent<SubWeapon>().RestoreShield((int)GameManager.Inst().Player.GetItem(index).Value);
                break;

            case (int)Item_ZzinEquipment.EquipType.REVIVE:
                if (id == 2)
                    actor.GetComponent<Player>().IsRevive = true;
                else
                    actor.GetComponent<SubWeapon>().IsRevive = true;
                break;
        }

        GameManager.Inst().Player.GetItem(index).CoolTime = GameManager.Inst().EquipDatas[GameManager.Inst().Player.GetItem(index).Type, GameManager.Inst().Player.GetItem(index).Rarity, 0];
    }
}
