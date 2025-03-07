﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

namespace Photon.Pun.Demo.PunBasics
{
    public class EngQuiz : MonoBehaviourPunCallbacks
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI Question;
        [SerializeField] TextMeshProUGUI Ans_1_Left;
        [SerializeField] TextMeshProUGUI Ans_2_Right;
        [SerializeField] TMP_Text Feedback;
        [SerializeField] GameObject QuizPanel;

        private Dictionary<string, string[]> quizData = new Dictionary<string, string[]>();
        private string currentQuestion;
        private string[] currentAnswers;

        public bool Answered;
        private GameObject itemToDestroy;
        private string lastQuestionKey;

        void Start()
        {
            LoadQuizData();
            Feedback.text = "";
        }

        void Update()
        {
            if (Answered)
            {
                StartCoroutine(HideQuizPanelAfterDelay(1f)); // Wait 1 second before hiding
                Answered = false;
            }
        }

        IEnumerator HideQuizPanelAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            QuizPanel.SetActive(false);
            Feedback.text = ""; // Reset feedback text
            Debug.Log("Hide Quiz Panel");
        }

        void LoadQuizData()
        {
            quizData.Add("Start", new string[] { "เริ่มต้น", "จบ" });
            quizData.Add("New", new string[] { "ใหม่", "เก่า" });
            quizData.Add("Horse", new string[] { "บ้าน", "ม้า" });
            quizData.Add("Dog", new string[] { "หมา", "แมว" });
            quizData.Add("Cow", new string[] { "วัว", "ควาย" });
            // Add more questions as needed
        }

        public void ShowQuizPanel(GameObject item)
        {
            // ตรวจสอบว่าเป็นผู้เล่นที่เจอกล่องหรือไม่
            if (item.GetComponent<PhotonView>().IsMine)
            {
                itemToDestroy = item;
                QuizPanel.SetActive(true);
                DisplayNewQuestion();
                Debug.Log("Show Quiz Panel for local player");
            }
        }

        [PunRPC]
        void DestroyItemRPC(int viewID)
        {
            PhotonView.Find(viewID).gameObject.SetActive(false);
        }

        void DisplayNewQuestion()
        {
            if (lastQuestionKey == null)
            {
                // Randomly select a new question
                string[] keys = new List<string>(quizData.Keys).ToArray();
                lastQuestionKey = keys[Random.Range(0, keys.Length)];
            }

            currentQuestion = lastQuestionKey;
            currentAnswers = quizData[currentQuestion];

            Question.text = currentQuestion;
            Ans_1_Left.text = currentAnswers[0];
            Ans_2_Right.text = currentAnswers[1];
        }

        public void CheckAnswer(string selectedAnswer)
        {
            if (selectedAnswer == currentAnswers[0])
            {
                Feedback.text = "ถูกต้อง!";
                Feedback.text += " เกมจบแล้ว!";
                Answered = true;
                lastQuestionKey = null;
            }
            else
            {
                Feedback.text = "ไม่ถูกต้อง! ลองอีกครั้ง";
                StartCoroutine(HideQuizPanelAfterDelay(1f));
            }

            // ทำลายไอเทมสำหรับทุกผู้เล่น
            if (itemToDestroy != null)
            {
                photonView.RPC("DestroyItemRPC", RpcTarget.All, itemToDestroy.GetComponent<PhotonView>().ViewID);
            }
        }

        public void OnAnswer1Selected()
        {
            CheckAnswer(Ans_1_Left.text);
        }

        public void OnAnswer2Selected()
        {
            CheckAnswer(Ans_2_Right.text);
        }
    }

}
