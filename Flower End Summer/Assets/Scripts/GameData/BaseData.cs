using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// data의 기본 클래스
/// data = name
/// 데이터의 갯수와 이름의 목록 리스트를 얻을 수 있음
/// </summary>
///
///


namespace FlowerEndSummer
{
    public class BaseData : ScriptableObject
    {
        public const string DataDirectory = @"Resources\Data\";
        public string[] names = default;
        
        public BaseData() {}
    
    
        public int GetDataCount()
        {
            int retValue = default;
            
            if (this.names is not null)
            {
                retValue = this.names.Length;
            }
            
            return retValue;
        }
    
        /// <summary>
        /// 이름 목록 가져오기
        /// </summary>
        /// <param name="showID"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string[] GetNameList(bool showID, string filter)
        {
            string[] retList = default;
    
            if (this.names is null) return retList;
    
            retList = new string[this.names.Length];
            
            for (var i = 0; i < this.names.Length; i++)
            {
                if (filter is "") continue;
                if (names[i].ToUpper().Contains(filter.ToUpper()) is false)
                {
                    continue;
                }
    
                if (showID)
                {
                    retList[i] = i.ToString() + " : " + names[i];
                }
                else
                {
                    names[i] = this.names[i];
                }
            }
    
            return retList;
        }
        
        public virtual int AddData(string name)
        {
            return GetDataCount();
        }
    
        public virtual void RemoveData(int index)
        {
            
        }
    
        public virtual void Copy(int index)
        {
            
        }
    
        
        
    }

}
