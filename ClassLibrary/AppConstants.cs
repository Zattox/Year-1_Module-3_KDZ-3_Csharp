public class AppConstants
{
    // Общие константы решения.
    public const int CountOfHeaders = 9; // Количество столбцов/заголовков в csv файле.
    public const string Token = "6757896301:AAGpTPCVQBwt0dkJN5wYG4MD3dCmmUJWGyg"; // Токен для связи с ботом.
    public const string Separator = "\";\""; // Разделитель в csv файле.

    // Константы для абсолютных путей до нужных директорий. (Иницилизируется при помощи метода Methods.CreateDirectories)
    public static string ExecutablePath = string.Empty; // Путь до папки проекта.
    public static string OutputJSONPath = ""; // Путь до папки, где хранятся файлы для вывода JSON.
    public static string OutputCSVPath = ""; // Путь до папки, где хранятся файлы для вывода CSV.
    public static string DataPath = ""; // Путь до папки, где хранятся все файлы с данными.
    public static string LogPath = ""; // Путь до папки, где хранятся файлы для логирования.

    // Тексты для кнопок основного меню.
    public const string MenuButtonText1 = "Загрузить новый файл";
    public const string MenuButtonText2 = "Выборка";
    public const string MenuButtonText3 = "Сортировка";
    public const string MenuButtonText4 = "Скачать последний обработанный файл";

    // Тектсы для кнопок *меню* фильтрации.
    public const string FilterButtonText1 = "Фильтрация по Type";
    public const string FilterButtonText2 = "Фильтрация по RegistrationDate";
    public const string FilterButtonText3 = "Фильтрация по CertificateHolderName и RegistrationDate";

    // Тексты для кнопок меню фильтрации.
    public const string SortingButtonText1 = "Сортировка по RegistrationNumber в порядке возрастания номера";
    public const string SortingButtonText2 = "Сортировка по RegistrationNumber в порядке убывания номера";

    // Тексты для кнопок меню загрузки файла.
    public const string OutputButtonText1 = "Скачать последний обработанный файл в JSON";
    public const string OutputButtonText2 = "Скачать последний обработанный файл в CSV";

    // Вспомогательные сообщения для пользователя.
    public const string InputMessage = "Загрузите файл в формате JSON или CSV ";
    public const string ErrorMessage = "Произошла ошибка во время исполнения программы! ";
    public const string StoppedMessage = "Для других функции для начала загрузите файла c данными! ";
    public const string TypeErrorMessage = "Программа не поддерживает такой тип данных! ";
    public const string CommandErrorMessage = "Такой команды не существует, повторите попытку ";
    public const string ChooseCommandMessage = "Выберите следующее действие ";
    public const string SuccessfulSaveMessage = "Данные успешно обработаны и локально сохранены! ";
    public const string UnluckyMessage = "По данному запросу не нашлось результатов (」°ロ°)」 ";
    public const string ChooseFilterMessage = "Введите запрос в одном из следующих форматов:\n\n" +
                                          $"{FilterButtonText1} \"значение поля\"\n\n" +
                                          $"{FilterButtonText2} \"значение поля\"\n\n" +
                                          $"{FilterButtonText3} \"значение поля\" \"значение поля\" ";
}