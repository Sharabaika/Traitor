using Photon.Pun;
using UnityEngine;

namespace Misc
{
    public interface IDamageable
    {
        void TakeHit(Vector3 hitDirection, Vector3 point, int damage, string damageSource);
    }
}