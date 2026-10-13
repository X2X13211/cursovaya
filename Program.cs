using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace descriptionUser
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionDatabase = "Data Source=/Users/pavelvitenik/RiderProjects/ConsoleApp2/ConsoleApp2/users.db";
            UserDataBase db = new UserDataBase(connectionDatabase);
            db.AddTablesInDatabase();
            db.AddBaseDataUserToDatabase();
        }
    }
}

class UserDataBase
{
    private AppLogger appLogger = new AppLogger();
    
    [Required(ErrorMessage = "Поле строки подключения к базе данных не может быть пустым")]
    private string connection_data_string;

    public UserDataBase(string connectionData)
    {
        connection_data_string = connectionData;
    }

    public SqliteConnection UseConnectDatabase()
    {
        var connection = new SqliteConnection(connection_data_string);
        try
        {
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            appLogger.Error("Произошла ошибка при подключении к базе данных!", ex);
            throw;
        }
    }

    public void AddTablesInDatabase()
    {
        using (var connection = UseConnectDatabase())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Surname TEXT NOT NULL,
                        LastName TEXT,
                        PhoneNumber TEXT,
                        Email TEXT
                    );
                    CREATE TABLE IF NOT EXISTS Pasports (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Series TEXT NOT NULL,
                        Number TEXT NOT NULL,
                        IssuedDate TEXT NOT NULL,
                        DepartmentCode TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                    );
                    CREATE TABLE IF NOT EXISTS Documents (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        RegistrationAddress TEXT,
                        EducationInfo TEXT,
                        GroundsForGranting INTEGER,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                    );";
                command.ExecuteNonQuery();

                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name IN ('Users', 'Pasports', 'Documents');";
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("--- Проверка таблиц в БД ---");
                    bool hasTables = false;
                
                    while (reader.Read())
                    {
                        appLogger.Information($"Таблица найдена: {reader.GetString(0)}");
                        hasTables = true;
                    }
                    if (!hasTables)
                    {
                        appLogger.Error("Ни одной таблицы не найдено!");
                    }
                }
            }
        }
    }

    public void AddBaseDataUserToDatabase()
    {
        User user = new User();
        user.Enter_name();
        user.Enter_surname();
        user.Enter_last_name();
        user.Enter_phone_number();
        user.Enter_email();

        using (var connection = UseConnectDatabase())
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Users (Name, Surname, LastName, PhoneNumber, Email) VALUES (@name, @surname, @lastName, @phone, @email);";
                command.Parameters.AddWithValue("@name", user.name);
                command.Parameters.AddWithValue("@surname", user.surname);
                command.Parameters.AddWithValue("@lastName", (object)user.last_name ?? DBNull.Value);
                command.Parameters.AddWithValue("@phone", user.phone_number);
                command.Parameters.AddWithValue("@email", user.email);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.CommandText = "SELECT last_insert_rowid();";
                long userId = (long)command.ExecuteScalar();

                AddPasportDataUserToDataBase(userId, connection);
                AddDocumentsToDatabase(userId, connection);
                ShowList(connection);
            }
        }
    }

    public void AddPasportDataUserToDataBase(long userId, SqliteConnection connection)
    {
        User_Pasport user_pasport = new User_Pasport();
        user_pasport.Enter_Data_Pasport();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Pasports (UserId, Series, Number, IssuedDate, DepartmentCode) VALUES (@userId, @series, @number, @issuedDate, @deptCode);";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@series", user_pasport.series_pasport_number);
            command.Parameters.AddWithValue("@number", user_pasport.number_pasport_number);
            command.Parameters.AddWithValue("@issuedDate", user_pasport.pasport_issued.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@deptCode", user_pasport.code_pasport_number);
            command.ExecuteNonQuery();
        }
    }

    public void AddDocumentsToDatabase(long userId, SqliteConnection connection)
    {
        User_Document user_document = new User_Document();
        user_document.Enter_permanent_registration_address();
        user_document.Enter_information_education();
        user_document.Enter_grounds_for_granting();

        using (var command = connection.CreateCommand())
        {
            int grounds = user_document.grounds_for_granting ? 1 : 0;
            command.CommandText = "INSERT INTO Documents (UserId, RegistrationAddress, EducationInfo, GroundsForGranting) VALUES (@userId, @address, @education, @grounds);";
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@address", user_document.permanent_registration_address);
            command.Parameters.AddWithValue("@education", user_document.information_education);
            command.Parameters.AddWithValue("@grounds", grounds);
            command.ExecuteNonQuery();
        }
    }

    private void ShowList(SqliteConnection connection)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                SELECT Users.Name, Users.Surname, Documents.RegistrationAddress 
                FROM Users 
                JOIN Documents ON Users.Id = Documents.UserId 
                WHERE Documents.GroundsForGranting = 1;";

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("\n---СПИСОК СТУДЕНТОВ НА ЗАСЕЛЕНИЕ---");
                if (!reader.HasRows)
                {
                    Console.WriteLine("Студентов, нуждающихся в общежитии, не найдено.");
                }
                else
                {
                    while (reader.Read())
                    {
                        string studentName = reader[0].ToString();
                        string studentSurname = reader[1].ToString();
                        string address = reader[2].ToString();
                        Console.WriteLine($"Студент: {studentName} {studentSurname} | Прописка: {address}");
                    }
                }
            }
        }
    }
}

class User
{ 
    AppLogger appLogger = new AppLogger();
    [Required(ErrorMessage = "Поле не может быть пустым!")]
    [RegularExpression(@"^[a-zA-ZА-Яа-я0-9_]+$", ErrorMessage = "Недопустимые символы в имени.")]
    public string name;

    [Required(ErrorMessage = "Поле не может быть пустым!")]
    [RegularExpression(@"^[a-zA-ZА-Яа-я0-9_]+$", ErrorMessage = "Недопустимые символы в фамилии.")]
    public string surname;

    [RegularExpression(@"^[a-zA-ZА-Яа-я0-9_]*$", ErrorMessage = "Недопустимые символы в отчестве.")]
    public string last_name;

    [Required(ErrorMessage = "Номер телефона не может быть пустым!")]
    [RegularExpression(@"^\+[1-9]\d{9,13}$", ErrorMessage = "Номер телефона введен неверно!")]
    public string phone_number;

    [Required(ErrorMessage = "Email не может быть пустым!")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email.")]
    public string email;

    public User() { }

    private bool ValidateModel()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, results, true);

        if (!isValid)
        {
            AppLogger appLogger = new AppLogger();
            foreach (var validationResult in results)
            {
                appLogger.Warning(validationResult.ErrorMessage);
                appLogger.Error($"Валидация не пройдена: {validationResult.ErrorMessage}");
            }
        }
        return isValid;
    }

    public void Enter_name()
    {
        Console.Write("Введите ваше имя: ");
        name = Console.ReadLine()?.Trim();
        if (!ValidateModel())
        {
            Console.Write("Введите имя еще раз!\n");
            Enter_name();
        }
    }

    public void Enter_surname()
    {
        Console.Write("Введите вашу фамилию: "); 
        surname = Console.ReadLine()?.Trim();
        if (!ValidateModel())
        {
            Console.Write("Введите фамилию еще раз!\n");
            Enter_surname();
        }
    }

    public void Enter_last_name()
    {
        Console.Write("Введите ваше отчество: ");
        last_name = Console.ReadLine()?.Trim();
        if (!ValidateModel())
        {
            Console.Write("Введите отчество еще раз!\n");
            Enter_last_name();
        }
    }

    public void Enter_phone_number()
    {
        Console.Write("Введите ваш номер телефона: ");
        phone_number = Console.ReadLine()?.Trim();
        if (!ValidateModel())
        {
            Console.Write("Попробуйте ввести номер повторно.\n");
            Enter_phone_number();
        }
    }

    public void Enter_email()
    {
        Console.Write("Введите ваш email адрес: "); 
        email = Console.ReadLine()?.Trim();
        
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(this, context, results, true))
        {
            foreach (var err in results)
            {
                if (err.MemberNames.Contains(nameof(email)))
                {
                    appLogger.Warning(err.ErrorMessage);
                    appLogger.Information($"Валидация не пройдена: строка '{email}' не является валидным адресом!");
                }
            }
            appLogger.Error("Попробуйте еще раз.");
            Enter_email();
        }
        else
        {
            appLogger.Information($"Email '{email}' успешно прошел валидацию.");
        }
    }
}

class User_Pasport
{
    [Required(ErrorMessage = "Серия не может быть пустой.")]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Серия паспорта должна состоять ровно из 4 цифр.")]
    public string series_pasport_number;

    [Required(ErrorMessage = "Номер не может быть пустым.")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Номер паспорта должен состоять ровно из 6 цифр.")]
    public string number_pasport_number;

    [Required(ErrorMessage = "Дата выдачи не может быть пустой.")]
    public DateTime pasport_issued;

    [Required(ErrorMessage = "Код подразделения не может быть пустым.")]
    [RegularExpression(@"^\d{3}-\d{3}$", ErrorMessage = "Неверный формат кода подразделения. Пример: 123-456")]
    public string code_pasport_number;

    public User_Pasport()
    {
    }

    public void Enter_Data_Pasport()
    {
        AppLogger appLogger = new AppLogger();
        Console.Write("Введите серию паспорта: "); 
        series_pasport_number = Console.ReadLine()?.Trim();

        Console.Write("Введите номер паспорта: "); 
        number_pasport_number = Console.ReadLine()?.Trim();
        
        try
        {
            Console.Write("Введите когда был выдан паспорт (в формате ДД.ММ.ГГГГ): ");
            pasport_issued = DateTime.Parse(Console.ReadLine().Trim());
            if (pasport_issued > DateTime.Now)
            {
                throw new Exception("Дата из будущего");
            }
        }
        catch
        {
            Console.WriteLine("Ошибка: неверный формат даты или дата из будущего!");
            Console.WriteLine("Попробуйте еще раз!");
            Enter_Data_Pasport(); 
            return;
        }

        Console.Write("Введите код подразделения (например, 123-456): ");
        code_pasport_number = Console.ReadLine()?.Trim();

        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();
        
        appLogger.FirstMessage(); 
        if (!Validator.TryValidateObject(this, context, results, true))
        {
            foreach (var error in results)
            {
                appLogger.Warning(error.ErrorMessage);
                appLogger.Information($"Валидация не пройдена: {error.ErrorMessage}");
            }
            Enter_Data_Pasport();
        }
        else
        {
            appLogger.Information("Паспортные данные успешно прошли валидацию.");
        }
    }
}

class User_Document
{
    [Required(ErrorMessage = "Адрес проживания не может быть пустым!")]
    public string permanent_registration_address;

    [Required(ErrorMessage = "Название учебного заведения не может быть пустым!")]
    public string information_education;

    public bool grounds_for_granting;

    public User_Document() { }

    public User_Document(string permanent_registration_address, string information_education, bool grounds_for_granting)
    {
        this.permanent_registration_address = permanent_registration_address;
        this.information_education = information_education;
        this.grounds_for_granting = grounds_for_granting; 
    }

    public void Enter_permanent_registration_address()
    {
        Console.Write("Введите ваш адрес проживания: ");
        permanent_registration_address = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrEmpty(permanent_registration_address))
        {
            Console.WriteLine("Поле не может быть пустым!");
            Console.WriteLine("Попробуйте еще раз.");
            Enter_permanent_registration_address();
        }
    }

    public void Enter_information_education()
    {
        Console.Write("Введите название учебного заведения: ");
        information_education = Console.ReadLine()?.Trim();
        
        if (string.IsNullOrEmpty(information_education))
        {
            Console.WriteLine("Поле не может быть пустым!");
            Console.WriteLine("Попробуйте еще раз.");
            Enter_information_education();
        }
    }

    public void Enter_grounds_for_granting()
    {
        try
        {
            Console.Write("Вы иногородний студент? 0/1: "); 
            int response1 = int.Parse(Console.ReadLine().Trim());
            
            Console.Write("У вас есть льготы? 0/1: "); 
            int response2 = int.Parse(Console.ReadLine().Trim());

            if (response1 == 1 || response2 == 1)
            {
                grounds_for_granting = true;
                Console.WriteLine("Статус: " + grounds_for_granting); 
            }
            else if (response1 == 0 && response2 == 0)
            {
                grounds_for_granting = false; 
                Console.WriteLine("Статус: " + grounds_for_granting); 
            }
            else
            {
                throw new Exception();
            }
        }
        catch
        {
            Console.WriteLine("Ошибка ввода! Введите только 0 или 1.");
            Enter_grounds_for_granting();
        }
    }
}

public class InvalidExceptionUser : Exception //пользовательское исключение
{
    public InvalidExceptionUser(string message) : base(message) { }
}

public class AppLogger
{
    public void FirstMessage()
    {
        Console.Clear();
        Console.Write("---Логирование включено----\n"); 
    }

    public void Warning(string message)
    {
        Console.Write($"[WARNING]: {message}");
    }

    public void Information(string message)
    {
        Console.Write($"[Information]: {message} \n");
    }
    
    public void Error(string techMessage, Exception ex = null)
    {
        Console.Write($"[Error]: {techMessage} \n");
        if (ex != null)
        { 
            Console.WriteLine(ex.ToString());
        }
    }
}