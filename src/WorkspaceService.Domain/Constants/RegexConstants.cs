namespace WorkspaceService.Domain.Constants;

public static class RegexConstants
{
    public const string RussianLetters = "^[а-яёА-ЯЁ]+$";
    public const string RussianLettersAndSpace = "^[а-яёА-ЯЁ ]+$";
    public const string RussianLettersAndNumbers = "^[а-яёА-ЯЁ0-9]+$";
    public const string EnglishLetters = "^[a-zA-Z]+$";
    public const string EnglishLettersAndSpace = "^[a-zA-Z ]+$";
    public const string EnglishLettersAndNumbers = "^[a-zA-Z0-9]+$";
    public const string EnglishLettersAndDot = "^[a-zA-Z.]+$";
    public const string EnglishLettersWithNumbersAndLimitedSymbols = "^[a-zA-Z0-9._]+$";
    public const string EnglishLettersWithNumbersAndSymbols = "^[a-zA-Z0-9!@$%^&*()_+{}:;<>,.?]+$";
    public const string Guid = "[a-fA-F\\d]{8}-[a-fA-F\\d]{4}-[a-fA-F\\d]{4}-[a-fA-F\\d]{4}-[a-fA-F\\d]{12}";
    public const string Email = "^[a-zA-Z0-9._-]+@[a-zA-Z0-9]+.[a-zA-Z]{2,}$";
}