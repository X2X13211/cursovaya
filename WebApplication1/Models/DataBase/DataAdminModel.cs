using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    [Table("Admin")] 
    public class DataAdminModel 
    {
        [Key]
        public int Id { get; set; } 
        
        [Required(ErrorMessage = "Укажите имя пользователя")]
        [MaxLength(50)] 
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Имя пользователя не может быть меньше 5 символов")]
        public string username  {get; set;} 
        
        [Required(ErrorMessage = "Укажите пароль")]
        [MaxLength(50)] 
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль не может быть меньше 6 символов")]
        public string password {get; set;} 
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}