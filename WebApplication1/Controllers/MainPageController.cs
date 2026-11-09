using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class MainPageController : Controller
{
    private readonly ILogger<MainPageController> _logger;

    public MainPageController(ILogger<MainPageController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult MainPage()
    {
        return View("~/Views/MainPage/MainPage.cshtml");
    }
}