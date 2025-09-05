using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewAnimalQuizData", menuName = "AR Quiz/QuizData")]
public class QuizData : ScriptableObject
{
    public string planetName;
    [TextArea]
    public string question;
    public string[] options = new string[3];
    public int correctAnswerIndex;
}
