namespace EscapeRoom.Infrastructure.Data.Constants
{
    public static class DbConstants
    {
        public const int MaxNameLength = 150;
        public const int MaxDescriptionLength = 2500;

        public static class CategoryConstants
        {
            public const int NameMaxLength = 60;
        }

        public static class GameConstants
        {
            public const int TitleMaxLength = 200;
            public const int CoverUrlMaxLength = 1000;
        }

        public static class PuzzleConstants
        {
            public const int TitleMaxLength = 200;
            public const int QuestionMaxLength = 2000;
            public const int AnswerHashMaxLength = 512;
        }
    }
}
