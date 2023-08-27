using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }


    void Start()
    {
        if (global)
        {
            if (instance != null && instance != this.gameObject.GetComponent<T>())
            {
                // 如果已经有一个实例存在，且不等于当前的gameObject的组件T，销毁当前gameObject
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            if (instance == null)
            {
                instance = this.gameObject.GetComponent<T>();
            }
        }
        this.OnStart();
    }


    protected virtual void OnStart()
    {
    }
}