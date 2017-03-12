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
    }
}
