using UnityEngine;

public class BulletController : ProjectileController, IPooledObject<BulletController>
{
    [Tooltip("Necessary bullet template reference for affecting continuously the bullet if continuously affected.")]
    public BulletController bulletTMP;
    void Update()
    {
        if (bulletTMP == null || !isContinuouslyAffected) return;
        SetProjectile(bulletTMP);
    }

    public void OnObjectSpawn(BulletController bullet){
        bulletTMP = bullet;
        base.OnObjectSpawn(bullet);
    }
}
