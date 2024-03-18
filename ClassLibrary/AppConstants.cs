internal class AppConstants
{
    public const int CountOfHeaders = 9;
    public const string Separator = "\";\"";

    public const string MenuButtonText1 = "Загрузить новый файл";
    public const string MenuButtonText2 = "Выборка";
    public const string MenuButtonText3 = "Сортировка";
    public const string MenuButtonText4 = "Скачать последний обработанный файл";

    public const string FilterButtonText1 = "Фильтрация по Type";
    public const string FilterButtonText2 = "Фильтрация по RegistrationDate";
    public const string FilterButtonText3 = "Фильтрация по CertificateHolderName и RegistrationDate";

    public const string SortingButtonText1 = "Сортировка по RegistrationNumber в порядке возрастания номера";
    public const string SortingButtonText2 = "Сортировка по RegistrationNumber в порядке убывания номера";

    public const string OutputButtonText1 = "Скачать последний обработанный файл в JSON";
    public const string OutputButtonText2 = "Скачать последний обработанный файл в CSV";

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

