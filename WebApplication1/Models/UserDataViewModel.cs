using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserDataViewModel
    {
        [Required(ErrorMessage = "Выберите категорию")]
        public string UserRole { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введите отчество")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Введите учебную группу")]
        public string StudyGroup { get; set; }
        
        [Range(1, 4, ErrorMessage = "Курс должен быть от 1 до 4")]
        public int Course { get; set; }
        
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите серию паспорта")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Серия паспорта должна состоять из 4 цифр")]
        public string PassportSeries { get; set; }

        [Required(ErrorMessage = "Введите номер паспорта")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Номер паспорта должен состоять из 6 цифр")]
        public string PassportNumber { get; set; }
        
        [Required(ErrorMessage = "Выберите корпус")]
        public string Building { get; set; }

        [Required(ErrorMessage = "Выберите тип комнаты")]
        public string RoomCapacity { get; set; }

        [Required(ErrorMessage = "Укажите дату заселения")]
        [DataType(DataType.Date)]
        public DateOnly CheckInDate { get; set; }

        [Required(ErrorMessage = "Укажите дату выселения")]
        [DataType(DataType.Date)]
        public DateOnly CheckOutDate { get; set; }
    }
}

