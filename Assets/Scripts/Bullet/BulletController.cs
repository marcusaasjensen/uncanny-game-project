using UnityEngine;

public class BulletController : ProjectileController, IPooledObject<BulletController>
{
    [Tooltip("Necessary bullet Configuration reference for affecting continuously the bullets if continuously affected.")]
    [SerializeField] BulletController _bulletConfiguration;
    
    void Update()
    {
        SetProjectileConfiguration();
    }

    public void OnObjectSpawn(BulletController bullet){
        _bulletConfiguration = bullet;
        base.OnObjectSpawn(bullet);
    }

    void SetProjectileConfiguration()
    {
        if (!_bulletConfiguration || !realTimeConfiguration) return;
        SetProjectile(_bulletConfiguration);
    }
}
