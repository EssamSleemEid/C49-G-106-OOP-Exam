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
            public int SelectedAnswerId { get; set; }

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
        }


        static void Main(string[] args)
        {
            
        }
    }
}
