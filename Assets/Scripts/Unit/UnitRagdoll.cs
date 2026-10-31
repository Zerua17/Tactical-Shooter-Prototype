using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone, Vector3 damageSourcePosition)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);
        ApplyExplosionToRagdoll(ragdollRootBone, 8f, damageSourcePosition);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
                
            }  
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float force, Vector3 damageSourcePosition)
    {
        Vector3 pushDirection =
            (ragdollRootBone.position - damageSourcePosition).normalized;

        foreach (Rigidbody rb in root.GetComponentsInChildren<Rigidbody>())
        {
            if (rb.isKinematic) continue;

            rb.AddForce(
                pushDirection * force,
                ForceMode.Impulse
            );
        }
    }

   
}
