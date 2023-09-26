using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    public Text avatarName;
    public Text avatarLevel;

    public UITeam TeamWindow;

    protected override void OnStart()
    {
        this.UpdateAvatar();

    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的:" + result,"对话框响应结果",MessageBoxType.Information);
    }

    public void OnclickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }

    public void OnclickShop()
    {
        UIManager.Instance.Show<UIShop>();
    }

    public void OnclickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }

    public void OnclickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }

    public void OnClickFriend()
    {
        UIManager.Instance.Show<UIFriends>();
    }

    public void OnClickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }

    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
}
