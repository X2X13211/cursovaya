using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

//Команды для миграций
// dotnet ef migrations add InitialCreate 
// dotnet ef database update

namespace WebApplication1.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PassportSeries), nameof(PassportNumber), IsUnique = true)]
    [Table("Users")] 
    public class DataUserModel 
    {
        [Key] 
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Выберите категорию")]
        [MaxLength(50)] 
        public string UserRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите фамилию")]
        [MaxLength(100)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите имя")]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        public string Patronymic { get; set; } = string.Empty; 

        [Required(ErrorMessage = "Введите учебную группу")]
        [MaxLength(20)]
        public string StudyGroup { get; set; } = string.Empty;

        [Range(1, 4, ErrorMessage = "Курс должен быть от 1 до 4")]
        public int Course { get; set; } = 1; 
            
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите серию паспорта")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Серия паспорта должна состоять из 4 цифр")]
        [Column(TypeName = "TEXT")]
        public string PassportSeries { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите номер паспорта")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Номер паспорта должен состоять из 6 цифр")]
        public string PassportNumber { get; set; } = string.Empty;
            
        [Required(ErrorMessage = "Выберите корпус")]
        [MaxLength(50)]
        public string Building { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите тип комнаты")]
        [MaxLength(50)]
        public string RoomCapacity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите дату заселения")]
        [DataType(DataType.Date)]
        public DateOnly CheckInDate { get; set; } 
        
        [Required(ErrorMessage = "Укажите дату выселения")]
        [DataType(DataType.Date)]
        public DateOnly CheckOutDate { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}