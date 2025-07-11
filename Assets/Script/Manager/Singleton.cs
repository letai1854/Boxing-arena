using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;

    private static readonly object _lock = new object();

    private static bool applicationIsQuitting = false;


    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";


                    }

                }
                return _instance;
            }
        }
    }


    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            applicationIsQuitting = true;
        }
    }


    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;


            InitializeSingleton();
        }
        else if (_instance != this as T)
        {
            Debug.LogWarning($"[Singleton] Another instance of {GetType()} was found on GameObject '{gameObject.name}'. Destroying this duplicate instance.");
            Destroy(gameObject);
        }

    }

    protected virtual void InitializeSingleton()
    {

    }
}