﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    enum Mode
    {
        search,
        move,
        none
    }
    public static Player instance { private set; get; }
    private Mode mode=Mode.none;
    private LayerMask collectableLayer;
    private LayerMask moveableLayer;
    private GameObject UI_Gameplay;
    private GameObject UI_Choose;
    public bool isFound { private set; get; }
    private void Awake()
    {
        instance = this;
        collectableLayer =1<< LayerMask.NameToLayer("Collectable");
        moveableLayer = 1 << LayerMask.NameToLayer("Moveable");
        GameObject ui = GameObject.FindWithTag("UI");
        UI_Gameplay = ui.transform.GetChild(0).gameObject;
        UI_Choose = ui.transform.GetChild(1).gameObject;
        UI_Choose.SetActive(false);
    }
    private void Update()
    {
        if (mode == Mode.search)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider2D collider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, collectableLayer);
                if (collider)
                {
                    collider.GetComponent<CollectableItem>().BeCollected();
                    TimeController.instance.CostTime(1);
                    FearController.instance.AddFear(2);
                    UI_Choose.SetActive(false);
                    UI_Gameplay.SetActive(true);
                    mode = Mode.none;
                    Enemy.instance.Act();
                    TimeController.instance.SyTime();
                }
            }
        }
        else if (mode == Mode.move)
        {
            if (Input.GetMouseButton(0)) {
                Collider2D collider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.1f, moveableLayer);
                if (collider)
                {
                    Door door = collider.GetComponent<Door>();
                    MapController.instance.ChangeRoom(door.indexX, door.indexY);
                    TimeController.instance.CostTime(1);
                    FearController.instance.AddFear(2);
                    mode = Mode.none;
                    UI_Choose.SetActive(false);
                    UI_Gameplay.SetActive(true);
                    Enemy.instance.Act();
                    TimeController.instance.SyTime();
                }
            }
        }
    }
    public void Search()
    {
        UI_Choose.SetActive(true);
        UI_Gameplay.SetActive(false);
        mode = Mode.search;
    }
    public void Move()
    {
        UI_Choose.SetActive(true);
        UI_Gameplay.SetActive(false);
        mode = Mode.move;
    }
    public void Back()
    {
        UI_Choose.SetActive(false);
        UI_Gameplay.SetActive(true);
        mode = Mode.none;
    }
    public void Rest()
    {
        FearController.instance.ReduceFear(5);
        TimeController.instance.CostTime(1);
        Enemy.instance.Act();
        TimeController.instance.SyTime();
    }
}
