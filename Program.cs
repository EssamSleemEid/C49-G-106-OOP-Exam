using System.Diagnostics;

namespace C49_G_106_OOP_Exam
{
    internal class Program
    {
        public class Answer : ICloneable, IComparable<Answer>
        {
            public int AnswerId { get; set; }
            public string AnswerText { get; set; }

            public Answer(int AnswerId,string AnswerText)
            {
                this.AnswerId = AnswerId;
                this.AnswerText = AnswerText;
            }

            public object Clone()
            {
                return new Answer(this.AnswerId,this.AnswerText);
            }

            public override string ToString()
            {
                return $"{AnswerId}. {AnswerText}";
            }

            public int CompareTo(Answer other)
            {
                if (other == null)
                    return 1;
                return AnswerId.CompareTo(other.AnswerId);
            }
        }

        public abstract class Question : ICloneable, IComparable<Question>
        {
            public string Header { get; set; }
            public string Body { get; set; }
            public int Mark { get; set; }
            public Answer[] Answers { get; set; }
            public Answer RightAnswer { get; set; }
            public int? SelectedAnswerId { get; set; }

            public Question(string Header, string Body, int Mark, Answer[] Answers, Answer RightAnswer)
            {
                this.Header = Header;
                this.Body = Body;
                this.Mark = Mark;
                this.Answers = Answers;
                this.RightAnswer = RightAnswer;
            }

            public object Clone()
            {
                Answer[] clonedAnswers = new Answer[Answers.Length];

                for (int i = 0; i < Answers.Length; i++)
                {
                    clonedAnswers[i] = (Answer)Answers[i].Clone();
                }

                Answer clonedRightAnswer = null;

                foreach (Answer answer in clonedAnswers)
                {
                    if (answer.AnswerId == RightAnswer.AnswerId)
                    {
                        clonedRightAnswer = answer;
                        break;
                    }
                }
                Question cloned = (Question)MemberwiseClone();
                cloned.Answers = clonedAnswers;
                cloned.RightAnswer = clonedRightAnswer;

                return cloned;
            }

            public override string ToString()
            {
                return $"{Header}: {Body} (Mark: {Mark})";
            }

            public int CompareTo(Question other)
            {
                if (other == null)
                    return 1;
                return Mark.CompareTo(other.Mark);
            }

            public abstract void ShowQuestion();
        }

        public class MyMCQ : Question
        {
            public MyMCQ(string header, string body, int mark,Answer[] answers, Answer rightAnswer): base(header, body, mark, answers, rightAnswer)
            {

            }

            public override void ShowQuestion()
            {
                Console.WriteLine(ToString());

                foreach (Answer answer in Answers)
                {
                    Console.WriteLine(answer);
                }
            }
        }

        public class MyTrueFale : Question
        {
            public MyTrueFale(string header, string body, int mark, Answer[] answers, Answer rightAnswer): base(header, body, mark, answers, rightAnswer)
            {

            }

            public override void ShowQuestion()
            {
                Console.WriteLine(ToString());

                foreach (Answer answer in Answers)
                {
                    Console.WriteLine(answer);
                }
            }
        }

        public abstract class Exam
        {
            public int Time { get; set; }
            public int NumberOfQuestions { get; set; }
            public Question[] Questions { get; set; }
            public Subject Subject { get; set; }
            
            public Exam(int Time, int NumberOfQuestions, Subject Subject)
            {
                this.Time = Time;
                this.NumberOfQuestions = NumberOfQuestions;
                Questions = new Question[NumberOfQuestions];
                this.Subject = Subject;
            }
            public abstract void ShowExam();

            public void StartExam()
            {
                Console.WriteLine("\n---------- EXAM STARTED ----------");
                Console.WriteLine($"Subject: {Subject.SubjectName}");
                Console.WriteLine($"Exam Duration: {Time} minutes\n");

                foreach (Question question in Questions)
                {
                    question.ShowQuestion();

                    int answerId = ReadInt("Choose your answer ID: ",1,question.Answers.Length);

                    question.SelectedAnswerId = answerId;

                    Console.WriteLine();
                }
            }
            public int CalculateGrade()
            {
                int grade = 0;

                foreach (Question question in Questions)
                {
                    if (question.SelectedAnswerId == question.RightAnswer.AnswerId)
                    {
                        grade += question.Mark;
                    }
                }

                return grade;
            }

            public int TotalMarks()
            {
                int total = 0;

                foreach (Question question in Questions)
                {
                    total += question.Mark;
                }

                return total;
            }

            public void ShowResult(Stopwatch stopwatch)
            {
                Console.WriteLine("\n---------- EXAM RESULT ----------");
                Console.WriteLine($"Grade: {CalculateGrade()} / {TotalMarks()}");
                Console.WriteLine($"Time Taken: {stopwatch.Elapsed:hh\\:mm\\:ss}");
            }

            public static int ReadInt(string message, int min, int max)
            {
                int value;

                while (true)
                {
                    Console.Write(message);

                    if (int.TryParse(Console.ReadLine(), out value)
                        && value >= min && value <= max)
                    {
                        return value;
                    }
                    Console.WriteLine($"Enter a Number From {min} To {max}.");
                }
            }
        }

        public class PracticalExam : Exam
        {
            public PracticalExam(int time, int numberOfQuestions, Subject subject): base(time, numberOfQuestions, subject)
            {

            }
            public override void ShowExam()
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                StartExam();

                stopwatch.Stop();

                ShowResult(stopwatch);

                Console.WriteLine("\n---------- CORRECT ANSWERS ----------");

                foreach (Question question in Questions)
                {
                    Console.WriteLine(question.Body);
                    Console.WriteLine($"Correct Answer: {question.RightAnswer}");
                    Console.WriteLine();
                }

                Console.WriteLine("Thank you...");
            }
        }
        public class FinalExam : Exam
        {
            public FinalExam(int time, int numberOfQuestions, Subject subject): base(time, numberOfQuestions, subject)
            {

            }
            public override void ShowExam()
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                StartExam();

                stopwatch.Stop();

                ShowResult(stopwatch);

                Console.WriteLine("\n---------- EXAM REVIEW ----------");

                foreach (Question question in Questions)
                {
                    question.ShowQuestion();

                    Console.WriteLine($"Your Answer: {FindAnswer(question, question.SelectedAnswerId.Value)}");

                    Console.WriteLine($"Correct Answer: {question.RightAnswer}");
                    Console.WriteLine();
                }

                Console.WriteLine("Thank you...");
            }

            private string FindAnswer(Question question, int answerId)
            {
                foreach (Answer answer in question.Answers)
                {
                    if (answer.AnswerId == answerId)
                    {
                        return answer.AnswerText;
                    }
                }
                return "Not found";
            }
        }

        public class Subject
        {
            public int SubjectId { get; set; }
            public string SubjectName { get; set; }
            public Exam Exam { get; set; }
            public Subject(int SubjectId, string SubjectName)
            {
                this.SubjectId = SubjectId;
                this.SubjectName = SubjectName;
            }
            public void CreateExam()
            {
                Console.WriteLine("---------- Create Exam ----------");
                Console.WriteLine("1. Final Exam");
                Console.WriteLine("2. Practical Exam");

                int examType = ReadInt("Choose Exam Type: ", 1, 2);
                int time = ReadInt("Enter Exam Time (30 - 180 minutes): ", 30, 180);
                int count = ReadInt("Enter Number Of Questions: ", 1, 100);

                if (examType == 1)
                {
                    Exam = new FinalExam(time, count, this);
                }
                else
                {
                    Exam = new PracticalExam(time, count, this);
                }

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"\n---------- QUESTION {i + 1} ----------");

                    int questionType = 2;

                    if (examType == 1)
                    {
                        Console.WriteLine("1. True OR False");
                        Console.WriteLine("2. MCQ");

                        questionType = ReadInt("Choose Question Type: ", 1, 2);
                    }

                    int mark = ReadInt("Enter Question Mark: ", 1, 1000);

                    Console.Write("Enter Question Body: ");
                    string body = Console.ReadLine() ?? "";

                    int answerCount = questionType == 1 ? 2 : 4;

                    Answer[] answers = new Answer[answerCount];

                    for (int j = 0; j < answerCount; j++)
                    {
                        string answerText;

                        if (questionType == 1)
                        {
                            answerText = j == 0 ? "True" : "False";
                            Console.WriteLine($"Answer {j + 1}: {answerText}");
                        }
                        else
                        {
                            Console.Write($"Enter Answer {j + 1}: ");
                            answerText = Console.ReadLine() ?? "";
                        }

                        answers[j] = new Answer(j + 1, answerText);
                    }

                    int rightAnswerId = ReadInt("Enter Correct Answer ID: ", 1, answerCount);

                    Answer rightAnswer = answers[rightAnswerId - 1];

                    if (examType == 1 && questionType == 1)
                    {
                        Exam.Questions[i] = new MyTrueFale("True OR False",body,mark,answers,rightAnswer);
                    }
                    else
                    {
                        Exam.Questions[i] = new MyMCQ("MCQ",body,mark,answers,rightAnswer);
                    }
                }

                Console.WriteLine("\nExam Created Successfully");
            }
            private int ReadInt(string message, int min, int max)
            {
                int value;

                while (true)
                {
                    Console.Write(message);

                    if (int.TryParse(Console.ReadLine(), out value)
                        && value >= min && value <= max)
                    {
                        return value;
                    }
                    Console.WriteLine($"Enter a Number From {min} To {max}");
                }
            }

        }
        static void Main(string[] args)
        {
            Subject subject = new Subject(1, "OOP");
            subject.CreateExam();

            Console.WriteLine("\nDo you want to start the Exam? (Y/N): ");
            string choice = Console.ReadLine() ?? "";

            if (choice.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                subject.Exam.ShowExam();
            }
            else
            {
                Console.WriteLine("Exam cancelled");
            }
        }
    }
}
