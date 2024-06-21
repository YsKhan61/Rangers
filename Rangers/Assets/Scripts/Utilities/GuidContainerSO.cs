using System;
using UnityEngine;

namespace BTG.Utilities
{
    public abstract class GuidContainerSO<T> : ScriptableObject where T : GuidSO
    {
        [SerializeField] private T[] m_DataList;
        public T[] DataList => m_DataList;

        public bool TryGetData(Guid guid, out T data)
        {
            data = Array.Find(m_DataList, d => d.Guid == guid);

            return data != null;
        }

        public T GetRandomData()
        {
            if (m_DataList == null || m_DataList.Length == 0)
            {
                return default;
            }

            return m_DataList[UnityEngine.Random.Range(0, m_DataList.Length)];
        }

        public void LogGuid()
        {
            foreach (var data in m_DataList)
            {
                Debug.Log($"{data.name} Guid: {data.Guid}");
            }
        }
    }
}

