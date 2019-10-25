using UnityEngine;

namespace PowerMage
{
    public interface IVision
    {
        Vector3 GetLookDirection();
        Vector3 GetPivot();
    }
}
