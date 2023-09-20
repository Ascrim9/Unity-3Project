using FlowerEndSummer.Common;
using UnityEngine;

namespace FlowerEndSummer.GameData
{
    /// <summary>
    /// 이펙트 프리팹과 경로와 타입등의 속성 데이터를 가지고
    /// 프리팹 사전 로딩 함 - 폴링
    /// 이펙트 인스턴스 기능도 갖고 있음 - 폴링
    /// </summary>
    public class EffectClip
    {
        public int Id = default;

        public EffectType effectType = EffectType.NORMAL;
        public GameObject effectPrefab = default;
        public string effectName = default;
        public string effectPath = default;
        public string effectFullpath = default;
        
        public EffectClip()
        {}

        public void PreLoad()
        {
            this.effectFullpath = effectPath + effectName;

            if (this.effectFullpath != string.Empty && this.effectPrefab == null)
            {
                this.effectPrefab = ResourceManager.Load(effectFullpath) as GameObject;
            }
        }

        public void ReleaseEffect()
        {
            if (this.effectPrefab != null)
            {
                this.effectPrefab = null;
            }
        }

        /// <summary>
        /// 원하는 위치에 내가 원하는 이펙트 인스턴스
        /// </summary>
        /// <param name="dirPos"></param>
        /// <returns></returns>
        public GameObject Instantiate(Vector3 dirPos)
        {
            if (effectPrefab is null)
            {
                PreLoad();
            }

            if (effectPrefab is not null)
            {
                
                GameObject effect = GameObject.Instantiate((effectPrefab), dirPos, Quaternion.identity);
                return effect;
            }

            return null;
        }


    }
}