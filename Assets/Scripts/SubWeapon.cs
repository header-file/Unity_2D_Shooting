using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubWeapon : MonoBehaviour
{
    public GameObject[] NormalPos;
    public GameObject[] SpreadPos;
    public GameObject LaserPos;
    public GameObject ChargePos;
    public GameObject Arrow;

    public Sprite[] Sprites;

    SpriteRenderer SpriteRenderer;

    int BulletType;
    int DownCount;
    bool IsDown;
    bool IsEditMode;
    bool IsReload;
    bool IsAlive;
    

    public int GetBulletType() { return BulletType; }
    
    public void SetBulletType(int T) { BulletType = T; }
    
    
    void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        BulletType = 0;
        DownCount = 0;
        IsDown = false;
        IsEditMode = false;
        IsReload = true;
        Arrow.SetActive(false);
        IsAlive = true;
    }

    void Update()
    {
        if (!IsAlive)
            return;

        if (IsDown)
            DownCount++;

        if (!IsEditMode && DownCount > 60)
            StartEditMode();

        if (IsEditMode)
            EditMode();
        else
            Fire();
    }

    void StartEditMode()
    {
        Time.timeScale = 0.1f;
        gameObject.transform.localScale = Vector3.one * 1.5f;
        IsEditMode = true;
        IsDown = false;
        Arrow.SetActive(true);
        GameManager.Inst().IptManager.SetIsAbleControl(false);

    }

    void EndEditMode()
    {
        Time.timeScale = 1.0f;
        gameObject.transform.localScale = Vector3.one * 1.0f;
        IsEditMode = false;
        DownCount = 0;
        Arrow.SetActive(false);
        GameManager.Inst().IptManager.SetIsAbleControl(true);
        Debug.Log("Fin");
    }

    void EditMode()
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 MPos = new Vector2(MousePos.x, MousePos.y);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 norm = (MPos - pos) / Vector2.Distance(MPos, pos);
        float angle = Vector2.Angle(Vector2.up, norm);
        if (MousePos.x > transform.position.x)
            angle *= -1;
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, angle);
        transform.rotation = rot;
    }

    public void Fire()
    {
        if (!IsReload || BulletType < 0)
            return;

        IsReload = false;
        
        GameManager.Inst().ShtManager.Shoot((Bullet.BulletType)BulletType, gameObject);
        
        Invoke("Reload", GameManager.Inst().UpgManager.GetBData(BulletType).GetReloadTime());
    }

    public void Reload()
    {
        IsReload = true;
    }

    public void Dead()
    {
        if (!IsAlive)
            return;

        IsAlive = false;
        SpriteRenderer.sprite = Sprites[1];

        Invoke("ReturnDead", 3.0f);
    }

    public void ReturnDead()
    {
        IsAlive = true;
        SpriteRenderer.sprite = Sprites[0];
    }

    private void OnMouseDown()
    {
        IsDown = true;
    }

    private void OnMouseUp()
    {
        EndEditMode();
    }
}
