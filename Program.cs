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
            User user = new User();
            User_Pasport user_pasport = new User_Pasport();
            User_Document user_document= new User_Document();
            
            user.Enter_name(); //имя
            user.Enter_surname(); //фамилия
            user.Enter_last_name(); //отчество
            user.Enter_phone_number(); //номер телефона
            user.Enter_email(); //email
            
            user_pasport.Enter_Data_Pasport(); //данные паспорта
            
            user_document.Enter_permanent_registration_address(); //адрес проживания
            user_document.Enter_information_education(); //информация об о.у
            user_document.Enter_grounds_for_granting(); // льготы
            
            using (var connection = new SqliteConnection("Data source = /Users/pavelvitenik/RiderProjects/ConsoleApp2/ConsoleApp2/users.db"))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
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
        Series TEXT NOT NULL,--series_pasport_number
        Number TEXT NOT NULL, --number_pasport_number
        IssuedDate TEXT NOT NULL, --pasport_issued (сохранять как строку 'YYYY-MM-DD')
        DepartmentCode TEXT, --code_pasport_number
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Documents (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        UserId INTEGER NOT NULL,
        RegistrationAddress TEXT,-- permanent_registration_address
        EducationInfo TEXT,-- information_education
        GroundsForGranting INTEGER,-- grounds_for_granting (0 - нет, 1 - да)
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );";
                command.ExecuteNonQuery(); //возвращает количество измененных записей
                
                Console.WriteLine("Таблица Users создана/заполнена");
                Console.WriteLine("Таблица Pasports создана/заполнена");
                Console.WriteLine("Таблица Documents создана/заполнена");
                
                //добавляем пользователя
                command.CommandText = $"INSERT INTO Users (Name, Surname, LastName, PhoneNumber, Email) VALUES ('{user.name}', '{user.surname}', '{user.last_name}', '{user.phone_number}', '{user.email}');"; 
                command.ExecuteNonQuery();

                //получаем айди созданного пользователя для связи
                command.CommandText = "SELECT last_insert_rowid();";
                long userId = (long)command.ExecuteScalar();

                //добавляем паспорт
                command.CommandText = $"INSERT INTO Pasports (UserId, Series, Number, IssuedDate, DepartmentCode) VALUES ({userId}, '{user_pasport.series_pasport_number}', '{user_pasport.number_pasport_number}', '{user_pasport.pasport_issued:yyyy-MM-dd}', '{user_pasport.code_pasport_number}');";
                command.ExecuteNonQuery();

                //добавляем документы
                int grounds = user_document.grounds_for_granting ? 1 : 0;
                command.CommandText = $"INSERT INTO Documents (UserId, RegistrationAddress, EducationInfo, GroundsForGranting) VALUES ({userId}, '{user_document.permanent_registration_address}', '{user_document.information_education}', {grounds});";
                command.ExecuteNonQuery();
                
                command.CommandText = @"
    SELECT Users.Name, Users.Surname, Documents.RegistrationAddress 
    FROM Users 
    JOIN Documents ON Users.Id = Documents.UserId 
    WHERE Documents.GroundsForGranting = 1;";
                
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\n--- список студентов на заселение ---");
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
}
class User //первичнаяя информация о пользователе
{ 
    public string name; // имя
    public string surname; //фамилия
    public string last_name; //отчество
    public string phone_number; //номер телефона 
    public string email; //email 

    public User()
    {
    }

    public void Enter_name()
    {
        //создаем внутренний логгер
        AppLogger appLogger = new AppLogger();

        try
        {
            appLogger.FirstMessage();
            Console.Write("Введите ваше имя: ");
            name = Console.ReadLine().Trim();
            appLogger.LogToDev(name); //пользователь ввел свое имя, его должен перехватить логер
            //вывод при этом соответсвующее сообщение
            bool isValid = Regex.IsMatch(name, @"^[a-zA-Z0-9_]+$"); //проверка на валид 
        }
        catch
        {
            if (string.IsNullOrEmpty(name))
            {
                Console.Write("Поле не может быть пустым!");
                Console.Write("Введите имя еще раз!"); 
                Enter_name(); 
            }
        }
    }

    public void Enter_surname()
    {
        //создаем внутренний логгер
        AppLogger appLogger = new AppLogger();
        try
        {
            //appLogger.FirstMessage();
            Console.Write("Введите вашу фамилию: "); 
            surname = Console.ReadLine().Trim();
            appLogger.LogToDev(surname); //лог
            bool isValid = Regex.IsMatch(surname, @"^[a-zA-Z0-9_]+$"); //проверка на валид 
        }
        catch
        {
            if (string.IsNullOrEmpty(surname))
            {
                Console.Write("Поле не может быть пустым!");
                Console.Write("Введите фамилию еще раз!"); 
                Enter_surname(); 
            }
        }
    }

    public void Enter_last_name()
    {
        AppLogger appLogger = new AppLogger();
        
        try
        {
            Console.Write("Введите ваше отчество: ");
            last_name = Console.ReadLine().Trim();
            appLogger.LogToDev(last_name); //лог
            bool isValid = Regex.IsMatch(last_name, @"^[a-zA-Z0-9_]+$"); //проверка на валид 
        }
        catch
        {
            if (string.IsNullOrEmpty(last_name))
            {
                Console.Write("Поле не может быть пустым!");
                Console.Write("Введите отчество еще раз!"); 
                Enter_last_name(); 
            }
        }
    }

    public void Enter_phone_number()
    {
        AppLogger appLogger = new AppLogger();
        Console.Write("Введите ваш номер телефона: ");
        phone_number = Console.ReadLine().Trim();
        string pattern = @"^\+[1-9]\d{9,13}$";
        bool isPhoneValid = Regex.IsMatch(phone_number, pattern);
        if (!isPhoneValid) 
        {
            Console.Write("Номер телефона введен неверно или допущена ошибка!");
            Console.Write("Попробуйте ввести номер повторно."); 
            Enter_phone_number();
        }
        else
        {
            appLogger.LogToDev($"Номер '{phone_number}' успешно прошел валидацию.");
        }
    }

    public void Enter_email()
    {
        AppLogger appLogger = new AppLogger();
        Console.Write("Введите ваш email адрес: "); 
        email = Console.ReadLine().Trim();
        appLogger.FirstMessage();
        appLogger.ValidateEmail(email); //проводим валидациб с логированием
    }
}

class User_Pasport//паспортные данные
{
    public string series_pasport_number; //серия
    public string number_pasport_number; // номер
    public DateTime pasport_issued; //когда выдан
    public string code_pasport_number; // код подразделения

    public User_Pasport()
    {
    }

    public void Enter_Data_Pasport()
    {
        AppLogger appLogger = new AppLogger();
        Console.Write("Введите серию паспорта: "); 
        series_pasport_number = Console.ReadLine().Trim();
        
        Console.Write("Введите номер паспорта: ");
        number_pasport_number = Console.ReadLine().Trim();
        
        try
        {
            Console.Write("Введите когда был выдан паспорт (в формате ДД.ММ.ГГГГ): ");
            pasport_issued = DateTime.Parse(Console.ReadLine().Trim());
            appLogger.LogToDev($"Дата '{pasport_issued}' успешно прошла валидацию.");
        }
        catch
        {
            Console.WriteLine("Ошибка: неверный формат даты!");
            Console.WriteLine("Попробуйте еще раз!");
            Enter_Data_Pasport(); 
            return; 
        }

        Console.Write("Введите код подразделения: ");
        code_pasport_number = Console.ReadLine().Trim();

        appLogger.FirstMessage(); 
        appLogger.ValidatePassword(series_pasport_number, number_pasport_number, pasport_issued,  code_pasport_number);
    }
    
}

class User_Document //документы для получения комнаты
{
    public string permanent_registration_address; // адрес фактического проживания
    public string information_education; // наименование учебного заведения
    public bool grounds_for_granting; //нуждаемость, иногородний статус, наличие льгот

    public User_Document()
    {
    }

    public User_Document(string permanent_registration_address, string information_education,  bool grounds_for_granting)
    {
        this.permanent_registration_address = permanent_registration_address;
        this.information_education = information_education;
        this.grounds_for_granting = grounds_for_granting; 
    }

    public void Enter_permanent_registration_address()
    {
        try
        {
            Console.Write("Введите ваш адрес проживания: ");
            permanent_registration_address = Console.ReadLine().Trim();
            
            if (string.IsNullOrEmpty(permanent_registration_address))
            {
                throw new Exception();
            }
        }
        catch
        {
            Console.WriteLine("Поле не может быть пустым!");
            Console.WriteLine("Попробуйте еще раз.");
            Enter_permanent_registration_address();
        }
    }

    public void Enter_information_education()
    {
        try
        {
            Console.Write("Введите название учебного заведения: ");
            information_education = Console.ReadLine().Trim();
            
            if (string.IsNullOrEmpty(information_education))
            {
                throw new Exception();
            }
        }
        catch
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
                Console.WriteLine("Статус: " + grounds_for_granting); //общежитие будет предоставлено
            }
            else if (response1 == 0 && response2 == 0)
            {
                grounds_for_granting = false; 
                Console.WriteLine("Статус: " + grounds_for_granting); //общежитие не будет предоставлено
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
public class AppLogger //логирование 
{
    public void FirstMessage()
    {
        Console.Clear();
        Console.Write("---Логирование включено----\n"); 
    }

    public void ValidateEmail(string email) //проверка email на валид
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                LogToUser("Поле Email не может быть пустым.");
                LogToDev("Валидация прервана: пустая строка.");
            }
            else if (!email.Contains("@") || !email.Contains(".") || email.Split('@')[1].Length < 3)
            {
                throw new FormatException();
            }
            else
            {
                LogToDev($"Email '{email}' успешно прошел валидацию.");
            }
        }
        catch (FormatException)
        {
            LogToUser("Некорректный формат Email. Пример правильного ввода: example@mail.com");
            LogToDev($"Валидация не пройдена: строка '{email}' не является валидным адресом!");
            LogToUser("Попробуйте еще раз.");
            User user = new User(); 
            user.Enter_email();
        }
    }

    public void ValidatePassword(string series_pasport_number, string number_pasport_number, DateTime pasport_issued, string code_pasport_number)
    {
        series_pasport_number = series_pasport_number?.Replace(" ", ""); 
        number_pasport_number = number_pasport_number?.Replace(" ", "");
        User_Pasport userPasport = new User_Pasport(); 
        
        if (string.IsNullOrWhiteSpace(series_pasport_number))
        {
            LogToUser("Серия не может быть пустой.");
            LogToDev("Валидация не пройдена: пустая строка.");
            userPasport.Enter_Data_Pasport();
        }
        else if (string.IsNullOrWhiteSpace(number_pasport_number))
        {
            LogToUser("Номер подразделения не может быть пустым.");
            LogToDev("Валидация не пройдена: пустая строка.");
            userPasport.Enter_Data_Pasport();
        }
        else if (pasport_issued > DateTime.Now)
        {
            LogToUser("Дата выдачи не может быть пустой.");
            LogToDev("Валидация не пройдена: пустая строка.");
            userPasport.Enter_Data_Pasport();
        }

        else if (string.IsNullOrWhiteSpace(code_pasport_number))
        {
            LogToUser("Код подразделения не может быть пустым.");
            LogToDev("Валидация не пройдена: пустая строка.");
            userPasport.Enter_Data_Pasport();
        }
        
        if (string.IsNullOrEmpty(series_pasport_number) || !Regex.IsMatch(series_pasport_number, @"^\d{4}$"))
        {
            LogToUser("Серия паспорта должна состоять ровно из 4 цифр.");
            LogToDev("Валидация не пройдена: неверный формат серии.");
            userPasport.Enter_Data_Pasport();
        }
        else if (string.IsNullOrEmpty(number_pasport_number) || !Regex.IsMatch(number_pasport_number, @"^\d{6}$"))
        {
            LogToUser("Номер паспорта должен состоять ровно из 6 цифр.");
            LogToDev("Валидация не пройдена: неверный формат номера.");
            userPasport.Enter_Data_Pasport();
        }
        else if (pasport_issued > DateTime.Now)
        {
            LogToUser("Дата выдачи паспорта не может быть в будущем.");
            LogToDev("Валидация не пройдена: дата выдачи из будущего.");
            userPasport.Enter_Data_Pasport();
        }
        
        string pattern = @"^\d{3}-\d{3}$";
        
        if (code_pasport_number != null && code_pasport_number.Length == 6 && Regex.IsMatch(code_pasport_number, @"^\d{6}$"))
        {
            code_pasport_number = code_pasport_number.Insert(3, "-");//добавляем дефис
            LogToDev($"Формат автоматически скорректирован: {code_pasport_number}");
        }
        if (code_pasport_number == null || !Regex.IsMatch(code_pasport_number, pattern))
        {
           LogToUser("Неверный формат кода подразделения. Пример правильного ввода: 123-456");
           LogToDev($"Валидация провалена: строка '{code_pasport_number}' не соответствует шаблону.");
           userPasport.Enter_Data_Pasport();
        }
    }
    public void LogToUser(string message) //пользователь 
    {
        //Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[ПОЛЬЗОВАТЕЛЬ]: {message} \n");
        Console.ResetColor();
    }
    public void LogToDev(string techMessage, Exception ex = null) //разработчик
    {
        //Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"[DEV]: {techMessage} \n");
        if (ex != null) Console.WriteLine(ex.ToString());
        Console.ResetColor();
    }
}