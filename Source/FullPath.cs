using System.Collections.Generic;
using UnityEngine;

namespace SpeedrunCutsceneSkip;

internal class FullPath
{
    public static string GetFullPath(GameObject go)
    {
        var transform = go.transform;
        List<string> pathParts = new List<string>();
        while (transform != null)
        {
            pathParts.Add(transform.name);
            transform = transform.parent;
        }
        pathParts.Reverse();
        return string.Join("/", pathParts);
    }
}
