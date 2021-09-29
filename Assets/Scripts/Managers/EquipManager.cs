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
        GameObject EquipAction = GameManager.Inst().ObjManager.MakeObj("EquipAction");
        EquipAction.transform.position = actor.transform.position;
        EquipAction.GetComponent<ActivationTimer>().IsStart = true;

        switch(GameManager.Inst().Player.GetItem(index).Type)
        {
            case (int)Item_ZzinEquipment.EquipType.MAGNET:
                GameObject MagnetEff = GameManager.Inst().ObjManager.MakeObj("MagnetAction");
                MagnetEff.transform.position = actor.transform.position;
                MagnetEff.GetComponent<ActivationTimer>().IsStart = true;

                Item_Resource[] resources = FindObjectsOfType<Item_Resource>();
                for(int i = 0; i < resources.Length; i++)
                {
                    if (resources[i].gameObject.activeSelf)
                        resources[i].BeginAbsorb();
                }
                break;

            case (int)Item_ZzinEquipment.EquipType.HOMING:
                GameManager.Inst().ShtManager.Shoot(Bullet.BulletType.EQUIP_MISSILE, actor, id, false, false, index);
                break;

            case (int)Item_ZzinEquipment.EquipType.HEAL:
                GameObject HealEff = GameManager.Inst().ObjManager.MakeObj("HealAction");
                HealEff.transform.position = actor.transform.position;
                HealEff.GetComponent<ActivationTimer>().IsStart = true;

                GameManager.Inst().SodManager.PlayEffect("Eq_Heal");

                if (id == 2)
                    actor.GetComponent<Player>().Heal((int)GameManager.Inst().Player.GetItem(index).Value);
                else
                    actor.GetComponent<SubWeapon>().Heal((int)GameManager.Inst().Player.GetItem(index).Value);
                break;

            case (int)Item_ZzinEquipment.EquipType.SHIELD:

                GameManager.Inst().SodManager.PlayEffect("Eq_Shield");

                if (id == 2)
                    actor.GetComponent<Player>().RestoreShield((int)GameManager.Inst().Player.GetItem(index).Value);
                else
                    actor.GetComponent<SubWeapon>().RestoreShield((int)GameManager.Inst().Player.GetItem(index).Value);
                break;

            case (int)Item_ZzinEquipment.EquipType.REVIVE:
                if (id == 2)
                    GameManager.Inst().UpgManager.BData[actor.GetComponent<Player>().GetBulletType()].SetIsRevive(true);
                else
                    GameManager.Inst().UpgManager.BData[actor.GetComponent<SubWeapon>().GetBulletType()].SetIsRevive(true);
                break;

            case (int)Item_ZzinEquipment.EquipType.KNOCKBACK:
                GameManager.Inst().ShtManager.Shoot(Bullet.BulletType.EQUIP_KNOCKBACK, actor, id, false, false, index);
                break;

            case (int)Item_ZzinEquipment.EquipType.SLOW:
                GameManager.Inst().ShtManager.Shoot(Bullet.BulletType.EQUIP_SLOW, actor, id, false, false, index);
                break;
        }

        GameManager.Inst().Player.GetItem(index).CoolTime = GameManager.Inst().EquipDatas[GameManager.Inst().Player.GetItem(index).Type, GameManager.Inst().Player.GetItem(index).Rarity, 0];
    }
}
