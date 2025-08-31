using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalQuizData", menuName = "AR Quiz/AnimalQuizData")]
public class AnimalQuizData : ScriptableObject
{
    public string animalName;
    [TextArea]
    public string question;
    public string[] options = new string[3];
    public int correctAnswerIndex;
}
