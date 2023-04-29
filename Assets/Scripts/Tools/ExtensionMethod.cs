using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod {
    private static float dotThreshold = 0.5f;

    public static bool isFacingTarget(this Transform transform, Transform target) {
        var toTarget = (target.position - transform.position).normalized;
        return Vector3.Dot(transform.forward, toTarget) >= dotThreshold;
    }
}
