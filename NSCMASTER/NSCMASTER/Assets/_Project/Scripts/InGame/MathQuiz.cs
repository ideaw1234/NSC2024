using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MathQuiz : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI Answer;
    [SerializeField] TextMeshProUGUI Num2;
    [SerializeField] TMP_InputField Player_Num1;
    [SerializeField] TMP_Text Feedback; 

    private int answer;
    private int num2;

    private void Start()
    {
        GenerateNewQuiz();
        Player_Num1.onValueChanged.AddListener(OnPlayerNum1ValueChanged);
    }

    private void GenerateNewQuiz()
    {
        answer = Random.Range(1, 100);
        num2 = Random.Range(1, 50);

        Debug.Log("New Quiz Generated");
        Debug.Log("Answer: " + answer);
        Debug.Log("Num2: " + num2);

        Answer.text = "= " + answer.ToString();
        Num2.text = "+ " + num2.ToString();
    }

    private void OnPlayerNum1ValueChanged(string input)
    {
        Pattren1();
    }

    private void Pattren1()
    {
        int num1;
        bool isValidInput = int.TryParse(Player_Num1.text, out num1);

        if (!isValidInput)
        {
            Debug.LogError("Invalid input. Please enter a valid number.");
            Feedback.text = "Invalid input. Please enter a valid number."; // แสดงข้อความ feedback
            return;
        }

        Debug.Log("Num1: " + num1);

        if (num1 == answer - num2)
        {
            Debug.Log("Correct");
            Feedback.text = "Correct!";
        }
        else
        {
            Debug.Log("Incorrect");
            Feedback.text = "Incorrect. Try again!";
        }
    }
}
