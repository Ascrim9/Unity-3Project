using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FlowerEndSummer.Common
{
    /// <summary>
    /// DCL 기법을 참고한 제네릭한 싱글톤 생성
    /// </summary>
    public class SingletonBase<T> : MonoBehaviour where T : class
    {
        private static readonly object _lock = new();
        private static volatile T _instance = default; //캐시 불일치 volatile 방지. (reference Java)
        private static bool _isAppExit = false;

        public static T Instance
        {
            get
            {
                if (_isAppExit is true)
                {
                    return null;
                }
                if (_instance is null)
                {
                    //Thread Safe 
                    lock (_lock)
                    {
                        string componentName = typeof(T).ToString();
                        GameObject findObject = GameObject.Find(componentName);
                        
                        //중복 확인
                        if (findObject is not null)
                        {
                            throw new Exception($"이미  {componentName} 해당이름의 오브젝트가 있습니다. ErrorCode-0000");
                            return null;
                        }

                        findObject = new GameObject(componentName);
                        _instance = findObject.AddComponent(typeof(T)) as T;
                        
                        // _instance = Activator.CreateInstance<T>();
                        DontDestroyOnLoad(findObject);
                        
                        Debug.Log(message: $"Create Singleton {typeof(T)} Singleton");
                    }
                }
                
                Debug.Log(message: $"Return {typeof(T)} Singleton");
                
                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        
        /// <summary>
        /// 고스트 객체 생성방지
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            _isAppExit = true;
        }
        
        public virtual void OnDestroy()
        {
            _isAppExit = true;
        }
    }
    
    
    public class SingletonBaseLocal<T> : MonoBehaviour where T : SingletonBaseLocal<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        singletonObject.name = typeof(T).ToString();
                        instance = singletonObject.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else if (instance != this)
            {
                DestroyImmediate(gameObject);
            }
        }

        protected void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}