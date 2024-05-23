using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoSingleton<UIController>
{
    public ViewPlayerInfo viewPlayerInfo;
    public MiniMap miniMap;
    public GameObject startPanel;
    public GameObject settingPanel;
    public GameObject menuPanel;
    public GameObject endPanel;

    void Start()
    {
        PlayerModel.Instance.OnMaxHpChange += viewPlayerInfo.SetMaxHp;
        PlayerModel.Instance.OnCurHpChange += viewPlayerInfo.UpdateHp;

        PlayerModel.Instance.Init();
    }

    public void OnTakeDamage(int amount)
    {
        PlayerModel.Instance.TakeDamage(amount);
    }

    public void SetMiniMap(bool flag)
    {
        miniMap.gameObject.SetActive(flag);
    }

    public void SetPlayerInfo(bool flag)
    {
        viewPlayerInfo.gameObject.SetActive(flag);
    }

    public void SetStartPanel(bool flag)//Ω¯»Î”Œœ∑
    {
        AudioManager.Instance.PlayClickAudio();

        startPanel.SetActive(flag);
        SetEndPanel(flag);
        SetPlayerInfo(true);
        SetMiniMap(true);
        PlayerModel.Instance.CurrentHp = 5;
        GameManager.Instance.StartGame();
        MouseController.Instance.SetRealCursor(true);
        MouseController.Instance.isCoroRun = false;
        //MouseController.Instance.SetCursorVisible(true);
    }

    public void SetSettingPanel(bool flag)
    {
        AudioManager.Instance.PlayClickAudio();
        settingPanel.SetActive(flag);
        if(flag)
            settingPanel.GetComponent<UISettingPanel>().GetSenitive();
    }

    public void SetSen()
    {
        settingPanel.GetComponent<UISettingPanel>().SetSensitive();
        SetSettingPanel(false);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlayClickAudio();
        Application.Quit();
        Debug.Log("Quit.");
    }

    public void SetMenuPanel(bool flag)
    {
        AudioManager.Instance.PlayClickAudio();

        MouseController.Instance.SetRealCursor(flag);
        menuPanel.SetActive(flag);
        if (flag)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

    }

    public void BackStartScene()
    {
        AudioManager.Instance.PlayClickAudio();

        SetMenuPanel(false);
        SetStartPanel(true);
        SetEndPanel(false);
        SetMiniMap(false);
        SetPlayerInfo(false);
        //MouseController.Instance.SetCursorVisible(false);
        GameManager.Instance.StartScene();
        MouseController.Instance.SetRealCursor(false);
        MouseController.Instance.isCoroRun = false;
    }

    public void SetEndPanel(bool flag)
    {
        endPanel.SetActive(flag);
    }

    public void SetEndTitle(bool isComplete)
    {
        endPanel.GetComponent<UIEndPanel>().SetTitle(isComplete);
    }
}
