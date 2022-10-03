using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    private bool _isInstance;
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Duplicate " + Instance.ToString() + " deleted");
            Destroy(this);
        }
        else
        {
            Instance = GetComponent<T>();
            _isInstance = true;
        }
    }

    protected virtual void OnDestroy()
    {
        if(_isInstance) Instance = null;
    }
}
