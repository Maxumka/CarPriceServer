
namespace MiddlewareLibrary
{
    public record Error(int Id, string Message);

    public static class Errors
    {
        public static readonly Error UnknownError = new(0, "Неизвестная ошибка");

        public static readonly Error UserNotFound = new(1, "Пользователь не найден");

        public static readonly Error CarWasNull = new(2, "Машина была недействительна");

        public static readonly Error UserWasNull = new(3, "Пользователь был недейстивтельным");

        public static readonly Error UserAlreadyInDb = new(4, "Пользователь уже в базе данных");
    }
}
