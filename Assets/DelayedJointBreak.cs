using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedJointBreak : MonoBehaviour
{
    public float delay = 0.1f; // Wait time in seconds before enabling break
    private FixedJoint2D joint;

    void OnEnable()
    {
        joint = GetComponent<FixedJoint2D>();
        if (joint != null)
        {
            joint.breakAction = JointBreakAction2D.Ignore;
            StartCoroutine(EnableBreakAfterDelay());
        }
    }

    System.Collections.IEnumerator EnableBreakAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        if (joint != null)
        {
            joint.breakAction = JointBreakAction2D.Destroy;
        }
    }
}