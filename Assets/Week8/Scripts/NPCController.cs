using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week8
{
    [Serializable]
    public class NPCJSONReceiver
    {
        public string animation_name;
        public string reply_to_player;
    }

    public class NPCController : MonoBehaviour
    {
        Animator anim;
        [SerializeField] SkinnedMeshRenderer face_Blendshape;

        int blinking = 0;
        float blinkingValue = 0;
        float blinkingTimer = 0;
        float blinkingTimerTotal = 3.5f;

        void Awake()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            blinkingTimer += Time.deltaTime;
            if (blinking == 0 && (UnityEngine.Random.value < 0.001f || blinkingTimer > blinkingTimerTotal))
            {
                blinkingTimer = 0;
                blinkingTimerTotal = UnityEngine.Random.Range(1.1f, 5.01f);
                blinking = 1;
                blinkingValue = 0;
            }
            else if (blinking == 1)
            {
                blinkingValue += Time.deltaTime * 1000;
                if (blinkingValue > 100)
                {
                    blinking = 2;
                    face_Blendshape.SetBlendShapeWeight(35, 100);
                }
                else
                {
                    face_Blendshape.SetBlendShapeWeight(35, blinkingValue);
                }
            }
            else if (blinking == 2)
            {
                blinkingValue -= Time.deltaTime * 600;
                if (blinkingValue < 0)
                {
                    blinking = 0;
                    face_Blendshape.SetBlendShapeWeight(35, 0);
                }
                else
                {
                    face_Blendshape.SetBlendShapeWeight(35, blinkingValue);
                }
            }
        }

        public void ShowAnimation(string animID)
        {
            //Debug.Log(animID);
            for (int i = 0; i < 60; i++)
            {
                if (i != 1)
                {
                    face_Blendshape.SetBlendShapeWeight(i, 0);
                }
            }

            switch (animID)
            {
                case "idle":
                    anim.SetTrigger(UnityEngine.Random.value < 0.3f ? "idle1" : UnityEngine.Random.value < 0.6f ? "idle2" : "idle");
                    face_Blendshape.SetBlendShapeWeight(UnityEngine.Random.value < 0.5f ? 9 : 24, UnityEngine.Random.value < 0.5f ? 100 : 67);
                    break;
                case "shy":
                    anim.SetTrigger("shy");
                    break;
                case "confuse":
                    anim.SetTrigger("confuse");
                    face_Blendshape.SetBlendShapeWeight(32, 100);
                    break;
                case "joking":
                    anim.SetTrigger("joking");
                    face_Blendshape.SetBlendShapeWeight(33, 100);
                    break;
                case "worried":
                    anim.SetTrigger("worried");
                    face_Blendshape.SetBlendShapeWeight(52, 100);
                    break;
                case "surprise":
                    anim.SetTrigger("surprise");
                    face_Blendshape.SetBlendShapeWeight(53, 100);
                    break;
                case "focus":
                    anim.SetTrigger("focus");
                    face_Blendshape.SetBlendShapeWeight(50, 100);
                    break;
                case "angry":
                    anim.SetTrigger("angry");
                    face_Blendshape.SetBlendShapeWeight(49, 100);
                    break;
                case "cheers":
                    anim.SetTrigger("cheers");
                    face_Blendshape.SetBlendShapeWeight(24, 100);
                    break;
                case "nod":
                    anim.SetTrigger("nod");
                    face_Blendshape.SetBlendShapeWeight(9, 100);
                    break;
                case "waving_arm":
                    anim.SetTrigger("waving_arm");
                    face_Blendshape.SetBlendShapeWeight(24, 100);
                    break;
                case "proud":
                    anim.SetTrigger("proud");
                    face_Blendshape.SetBlendShapeWeight(24, 100);
                    break;
            }
        }
    }
}