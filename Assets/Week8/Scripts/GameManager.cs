using UnityEngine;
using TMPro;
using ChatGPTWrapper;
using UnityEngine.UI;
using System;

namespace Week8
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [SerializeField] ChatGPTConversation promptGenerator;
        [SerializeField] ChatGPTConversation promptResponder;

        [SerializeField] TMP_InputField playerPrompt;
        [SerializeField] TMP_Text promptText;
        [SerializeField] TMP_Text replyText;
        [SerializeField] NPCController npc;

        [SerializeField] GameObject submitPrompt;
        [SerializeField] GameObject gradeJoke;
        [SerializeField] Toggle randomToggle;

        void Awake()
        {
            instance = this;
            gradeJoke.SetActive(false);

            promptGenerator.Init();
            promptResponder.Init();
        }

        void Update()
        {
            if (Input.GetButtonUp("Submit"))
            {
                SubmitOwnPrompt();
            }
        }

        public void GetRandomPrompt()
        {
            submitPrompt.SetActive(false);
            playerPrompt.text = "";
            promptText.text = "Generating...";
            replyText.text = "";
            promptGenerator.SendToChatGPT("{\"player_said\":\"" + "next prompt" + "\"}");
        }

        public void ReceiveRandomPrompt(string message)
        {
            if (!message.EndsWith("}"))
            {
                if (message.Contains("}"))
                {
                    message = message[..(message.LastIndexOf("}") + 1)];
                }
                else
                {
                    message += "}";
                }
            }

            message = message.Replace("\\", "\\\\");
            NPCJSONReceiver npcJSON = JsonUtility.FromJson<NPCJSONReceiver>(message);
            string talkLine = npcJSON.reply_to_player;

            promptResponder.SendToChatGPT("{\"player_said\":\"" + talkLine + "\"}");
            promptText.text = $"Prompt: {talkLine}";
            replyText.text = "Generating...";
        }

        public void SubmitOwnPrompt()
        {
            if (playerPrompt.text != "")
            {
                promptResponder.SendToChatGPT("{\"player_said\":\"" + playerPrompt.text + "\"}");
                submitPrompt.SetActive(false);

                promptText.text = $"Prompt: {playerPrompt.text}";
                playerPrompt.text = "";
                replyText.text = "Generating...";
            }
        }

        public void ReceiveChatGPTReply(string message)
        {
            try
            {
                if (!message.EndsWith("}"))
                {
                    if (message.Contains("}"))
                    {
                        message = message[..(message.LastIndexOf("}") + 1)];
                    }
                    else
                    {
                        message += "}";
                    }
                }

                message = message.Replace("\\", "\\\\");
                NPCJSONReceiver npcJSON = JsonUtility.FromJson<NPCJSONReceiver>(message);
                string talkLine = npcJSON.reply_to_player;
                replyText.text = $"Joke: {talkLine}";
                npc.ShowAnimation(npcJSON.animation_name);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                string talkLine = "failed";
                replyText.text = talkLine;
            }

            gradeJoke.SetActive(true);
        }

        public void Continue()
        {
            gradeJoke.SetActive(false);

            if (randomToggle.isOn)
                GetRandomPrompt();
            else
                submitPrompt.SetActive(true);
        }
    }
}