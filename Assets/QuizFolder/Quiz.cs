using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Add the quiz data that contains the question and the answer 

[CreateAssetMenu(fileName = "NewQuizData", menuName = "AR Quiz/QuizData")]
public class QuizData : ScriptableObject
{
    public string planetName;
    [TextArea]
    public string question;
    public string[] options = new string[3];
    public int correctAnswerIndex;
}
