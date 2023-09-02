using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain> {

    public Text avatarName;
    public Text avatarLevel;

    protected override void OnStart()
    {
        this.UpdateAvatar();

    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    void Update() {

    }
    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.SetTitle("这是一个测试UI");
        test.OnClose += Test_OnClose;

    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show("点击了对话框的:" + result,"对话框响应结果",MessageBoxType.Information);
    }

    public void OnclickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
}
