using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly AppDbContext _context; //поле для хранения контекста базы данных

    //конструктор
    public LoginController(ILogger<LoginController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    public IActionResult Login()
    {   
        return View("~/Views/Admin/Login.cshtml");
    }
    [HttpGet]
    public IActionResult AdminDashboard()
    {
        return View("~/Views/Admin/AdminDashboard.cshtml"); 
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(RegisterAdminForm model)
    {
        _logger.LogInformation("Валидация данных");
        var dbAdmin = await _context.Admin
            .FirstOrDefaultAsync(a => a.username == model.username && a.password == model.password);
        if (dbAdmin != null) 
        {
            _logger.LogInformation("Данные успешно прошли валидацию, перенаправление на AdminDashBoard");
            //jwt токен 
            return RedirectToAction("AdminDashBoard");
        }
        else 
        {
            _logger.LogInformation("Данные не прошли проверку, перенаправляем обратно на Login");
            _logger.LogInformation("Неверное имя пользователя или пароль.");
            return Login();
        }
    }
    
}
    