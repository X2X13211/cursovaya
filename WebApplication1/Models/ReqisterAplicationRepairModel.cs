using System;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Models
{
    public class ReqisterAplicationRepairModel
    {
        [Required(ErrorMessage = "Укажите номер корпуса")]
        [Range (1, 4, ErrorMessage = "Ошибка! такого корпуса нет")]
        public string RepairBuilding { get; set; } //номер корпуса
        
        [Required(ErrorMessage = "Укажите номер комнаты")]
        [Range (1, 200, ErrorMessage = "Ошибка! Такой комнаты нет")]
        public string RepairRoom { get; set; } //номер комнаты
        
        [Required(ErrorMessage = "Опишите неисправность")]
        [StringLength (150, MinimumLength = 10, ErrorMessage = "Описание неисправности не должно превышать 150 символов")]
        public string RepairDescription { get; set; } //описание неисправности (не более 100-150 символов)
    }
}