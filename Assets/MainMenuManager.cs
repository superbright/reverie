using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenuManager : MonoBehaviour {

    public List<MainMenu> menus;
    public GameObject parent;

    private static MainMenuManager _instance;
    public static MainMenuManager Instance { get { return _instance; } }

    int currentMenu = 0;
    int currentPos = 0;

    // Use this for initialization
    void Awake () {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void scrollDown(Transform obj)
    {
        ScrollRect rect = menus[currentMenu].obj.GetComponentInChildren<ScrollRect>();
        Debug.Log(rect);
        rect.verticalNormalizedPosition += 0.1f;
    }

    public void scrollUp(Transform obj)
    {
        ScrollRect rect = menus[currentMenu].obj.GetComponentInChildren<ScrollRect>();
        Debug.Log(rect);
        rect.verticalNormalizedPosition -= 0.1f;
    }

    public void showNextMenu(Transform parent)
    {
       this.parent.transform.parent = parent;
       this.parent.transform.localPosition = new Vector3(0, 0, 0.5f);
       this.parent.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (currentMenu + 1 < menus.Count)
            currentMenu++;
        else
            currentMenu = 0;

        showMenu(currentMenu);

    }
    public void showPrevMenu(Transform parent)
    {
        this.parent.transform.parent = parent;
        this.parent.transform.localPosition = new Vector3(0, 0, 0.5f);
        this.parent.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if (currentMenu -1 >= 0)
            currentMenu--;
        else
            currentMenu = menus.Count -1;

        showMenu(currentMenu);
    }

    public MainMenu getMenu(int index)
    {
        if (index >= menus.Count)
            return null;

        else return menus[index];
    }

    public List<MainMenu> getMenus()
    {
        return menus;
    }

    public void showMenu(int index)
    {
        foreach(MainMenu menu in menus)
        {
            menu.obj.SetActive(false);
        }

        getMenu(index).obj.SetActive(true);
        //parent.transform.position = position;

    }
}

[Serializable]
public class MainMenu
{
    public string name;
    public GameObject obj;
}
