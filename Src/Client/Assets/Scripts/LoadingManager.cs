using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SkillBridge.Message;
using ProtoBuf;
using Services;

public class LoadingManager : MonoBehaviour {

    public GameObject UIGameUp;
    public GameObject UITips;
    public GameObject UILoading;
    public GameObject UILogin;

    public Slider progressBar;
    public Text progressText;
    public Text progressNumber;

    public Text tipText; // 提示字串
    public Text loadingText; // 正在加载

    private string[] loadingTips = {
        "初始化...",
        "加载玩家数据...",
        "加载场景资源...",
        "预加载音效...",
    };
    // Use this for initialization
    IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        UITips.SetActive(true);
        UILoading.SetActive(false);
        UILogin.SetActive(false);
        yield return new WaitForSeconds(2f);
        UILoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);

        //yield return DataManager.Instance.LoadData();

        //Init basic services
        MapService.Instance.Init();
        UserService.Instance.Init();


        // Fake Loading Simulate
        for (float i = 0; i < 1;)
        {
            i += Random.Range(0.001f, 0.005f);

            progressBar.value = i;
            progressNumber.text = Mathf.RoundToInt(i * 100) + "%";

            if (i < 0.9)
            {
                int tipIndex = Mathf.Clamp(Mathf.FloorToInt(i * loadingTips.Length), 0, loadingTips.Length - 1);
                tipText.text = loadingTips[tipIndex];
            }
            else if (i >= 1)
            {
                tipText.text = ""; // 清空tipText的内容
                loadingText.text = "加载完成！";
                yield return new WaitForSeconds(1f);
                break;
            }
            else if (string.IsNullOrEmpty(tipText.text))
            {
                loadingText.text = "正在加载...";
            }
            yield return new WaitForEndOfFrame();
        }


        UILoading.SetActive(false);
        UILogin.SetActive(true);
        yield return null;
    }


    // Update is called once per frame
    void Update () {

    }
}
