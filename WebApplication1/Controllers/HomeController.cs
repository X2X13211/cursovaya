using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context; //поле для хранения контекста базы данных
    
    
    //конструктор
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Index() //отрисовываем страницу с полями для заполнения 
    {
        return View("~/Views/Home/Index.cshtml"); 
    }
    
    [HttpGet]
    public IActionResult SuccessRegistrationPage() //успешная регистрация заявки
    {
        return View("~/Views/Home/SuccesRegistrationPage.cshtml"); 
    }

    public IActionResult SuccessRepairRequest() //успешная регистрация заявки на ремонт
    {
        return View("~/Views/Home/SuccesRegistrationRepairPage.cshtml"); 
    }
    
    
    [HttpPost]
    public async Task<IActionResult> SubmitRegistration(UserDataViewModel model)
    {
        _logger.LogInformation("Данные успешно прошли первичную проверку"); 
        _logger.LogInformation("Метод SubmitRegistration вызван! Фамилия: {model.Surname}");
        _logger.LogInformation("Имя: {model.Username}");
        _logger.LogInformation("Отчество: {model.Patronymic}");
        _logger.LogInformation("Учебная группа: {model.StudyGroup}"); 
        _logger.LogInformation("Курс: {model.Course}"); 
        _logger.LogInformation("Номер телефона: {model.Phone}");
        _logger.LogInformation("Почта: {model.Email}");
        _logger.LogInformation("Серия паспорта: {model.PassportSeries}");
        _logger.LogInformation("Корпус: {model.Building}");
        _logger.LogInformation("Тип комнаты: {model.RoomCapacity}");
        _logger.LogInformation("Дата заселения: {model.CheckInDate}");
        _logger.LogInformation("Дата выселения: {model.CheckOutDate}");
        
        
        
        if (!ModelState.IsValid) 
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"[ERROR] Поле: {state.Key} | Ошибка: {error.ErrorMessage}");
                }
            }
            _logger.LogWarning("Ошибка валидации ModelState!");
            return View("~/Views/Home/Index.cshtml", model); 
        }
        _logger.LogInformation("Данные успешно прошли валидацию!");
        var dbUser = new DataUserModel()
        {
            Surname = model.Surname,
            Username = model.Username,
            Patronymic = model.Patronymic,
            StudyGroup = model.StudyGroup,
            Course = model.Course,
            Phone = model.Phone,
            Email = model.Email,
            PassportSeries = model.PassportSeries,
            Building = model.Building,
            RoomCapacity = model.RoomCapacity,
            CheckInDate = model.CheckInDate,
            CheckOutDate = model.CheckOutDate
        }; 
        _context.User.Add(dbUser); //сохранение в базу данных
        await _context.SaveChangesAsync(); 
        
        _logger.LogInformation("Пользователь успешно сохранен в БД!");
        
        return RedirectToAction("SuccessRegistrationPage"); 
    }

    public IActionResult SubmitRepairRequest(ReqisterAplicationRepairModel model)
    {
        _logger.LogInformation("Данные успешно прошли первичную проверку");
        _logger.LogInformation(" Метод SubmitRepairRequest вызван! ");
        _logger.LogInformation($" Номер корпуса: {model.RepairBuilding}");
        _logger.LogInformation($" Номер комнаты: {model.RepairRoom}");
        _logger.LogInformation($" Описание неисправности: {model.RepairDescription}");

        if (!ModelState.IsValid)
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"[ERROR] Поле: {state.Key} | Ошибка: {error.ErrorMessage}");
                }
            }
            return View("~/Views/Home/Index.cshtml");
        }
        return RedirectToAction("SuccessRepairRequest");
    }
}