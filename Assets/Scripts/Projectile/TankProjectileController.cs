using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectileController
{
    private TankProjectileModel m_ProjectileModel;
    private TankProjectileView m_ProjectileView;

    public Transform Transform => m_ProjectileView.transform;

    public void Update()
    {

    }

    public TankProjectileController(TankProjectileDataSO projectileData)
    {
        m_ProjectileModel = new TankProjectileModel(projectileData, this);
        m_ProjectileView = Object.Instantiate(projectileData.ProjectileViewPrefab);
        m_ProjectileView.SetController(this);
    }
}
