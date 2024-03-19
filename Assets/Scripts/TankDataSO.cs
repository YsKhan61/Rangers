using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "ScriptableObjects/TankDataSO")]
public class TankDataSO : ScriptableObject
{
    [SerializeField] private TankView m_TankViewPrefab;
    public TankView TankViewPrefab => m_TankViewPrefab;
}
