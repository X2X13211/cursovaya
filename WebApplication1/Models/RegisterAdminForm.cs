using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class RegisterAdminForm
    {
        [Required(ErrorMessage = "Укажите имя пользователя")]
        [MaxLength(50)] 
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Имя пользователя не может быть меньше 5 символов")]
        public string username  {get; set;} 
        
        [Required(ErrorMessage = "Укажите пароль")]
        [MaxLength(50)] 
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль не может быть меньше 6 символов")]
        public string password {get; set;} 
    }
}