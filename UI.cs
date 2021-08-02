using StressLevelZero.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

using ModThatIsNotMod;

namespace Zombones
{
    public static class UIWatch
    {
        public static TextMeshPro Text;
        public static TextMeshPro Border;

        public static void MakeUI()
        {
            GameObject UIObj = new GameObject();
            UIObj.transform.parent = TryFindPlayerWrist().transform;

            Transform TextTransform = new GameObject().transform;
            TextTransform.parent = UIObj.transform;
            TextTransform.localPosition = new Vector3(0f, 0f, 0.01f);

            Transform BorderTransform = new GameObject().transform;
            BorderTransform.parent = UIObj.transform;

            Text = TextTransform.gameObject.AddComponent<TextMeshPro>();
            Text.text = $"{BuildInfo.Name}\n{BuildInfo.Version}";
            Text.fontSize = 2f;
            Text.alignment = TextAlignmentOptions.Center;
            Text.fontStyle = FontStyles.Bold;

            Border = BorderTransform.gameObject.AddComponent<TextMeshPro>();
            Border.text = "[  ]";
            Border.fontSize = 12f;
            Border.alignment = TextAlignmentOptions.Center;
            Border.overflowMode = 0;
            Border.color = Color.red;

            UIObj.transform.localPosition = new Vector3(-0.0131f, 0.0306f, 0f);
            UIObj.transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
            UIObj.transform.localScale = Vector3.one / 15;
        }

        public static void SetText(string Text)
        {
            if (UIWatch.Text != null && UIWatch.Border != null)
            {
                UIWatch.Text.text = Text;
            }
        }

        public static void Haptic(float power)
        {
            Player.leftHand.controller.haptor.Haptic_WepFire(power);
        }

        public static GameObject TryFindPlayerWrist()
        {
            GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
            bool flag = array != null;
            bool flag2 = flag;
            if (flag2)
            {
                foreach (GameObject gameObject in array)
                {
                    bool flag3 = gameObject.name == "PlayerTrigger";
                    bool flag4 = flag3;
                    if (flag4)
                    {
                        GameObject gameObject2 = gameObject;
                        Transform parent = gameObject2.transform.parent.parent.parent;
                        Transform[] array3 = parent.GetComponentsInChildren<Transform>();
                        GameObject result = null;
                        foreach (Transform transform in array3)
                        {
                            bool flag5 = transform.name == "l_WristSHJnt";
                            bool flag6 = flag5;
                            if (flag6)
                            {
                                result = transform.gameObject;
                            }
                        }
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
