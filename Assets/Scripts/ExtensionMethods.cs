using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ExtensionMethods
    {
        public static bool GetAnyOfKeysDown(params KeyCode[] keycodes)
        {
            foreach (var code in keycodes)
            {
                if (Input.GetKeyDown(code))
                {
                    return true;
                }
            }
            
            return false;
        }

        public static bool GetAnyOfKeys(params KeyCode[] keycodes)
        {
            foreach (var code in keycodes)
            {
                if (Input.GetKey(code))
                {
                    return true;
                }
            }

            return false;
        }

        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindDeepChild(aName);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
