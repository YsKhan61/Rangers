using UnityEngine;

public class TankController
{
    private TankModel m_TankModel;
    private TankView m_TankView;

    public TankController(TankDataSO tankData)
    {
        m_TankModel = new TankModel(tankData, this);
        m_TankView = Object.Instantiate(tankData.TankViewPrefab.gameObject).GetComponent<TankView>();
        m_TankView.SetController(this);
    }
}
