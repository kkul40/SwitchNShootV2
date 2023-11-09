using UnityEngine;

namespace Game.Manager
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;

        protected virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
                Destroy(this as T);

            DontDestroyOnLoad(this);
        }
    }
}
