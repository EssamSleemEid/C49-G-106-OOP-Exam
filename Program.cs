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


        static void Main(string[] args)
        {
            
        }
    }
}
