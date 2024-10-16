using UnityEngine;
public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}
