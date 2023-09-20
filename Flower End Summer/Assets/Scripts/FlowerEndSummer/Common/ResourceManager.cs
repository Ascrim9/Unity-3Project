using UnityEditor.U2D;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace FlowerEndSummer.Common
{
    /// <summary>
    /// Resources.Load를 래핑하는 클래스.
    /// 나중에 어셋번들로 변경
    /// </summary>
    public class ResourceManager
    {
        public static UnityObject Load(string path)
        {
            //에셋 로드 변경
            return Resources.Load(path);
        }
        
        public static GameObject LoadAndIns(string path)
        {
            UnityObject source = Load(path);
            
            return (source is not null) ? GameObject.Instantiate(source) as GameObject: null;

        }
    }
    

}