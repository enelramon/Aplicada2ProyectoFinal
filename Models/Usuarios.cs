﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicada2ProyectoFinal.Models
{
    public class Usuarios
    {
        [Key]
        [Range(0, 100000000, ErrorMessage = "El campo Id no puede ser menor que cero")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = " Nombre no puede estar vacío")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Usuario no puede estar vacío")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La Contraseña no puede estar vacío")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Vuelva a escribir la contraseña")]
        [Compare("Contraseña", ErrorMessage = "La Contraseña y la confirmacion no coinciden.")]
        public string RepeatContraseña { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd,mm, yyyy}")]
        [Required(ErrorMessage = "El campo fecha no puede estar vacío")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El Tipo Usuario no puede estar vacío")]
        public string TipodeUsuario { get; set; }
        public Usuarios()
        {
            UsuarioId = 0;
            Nombre = string.Empty;
            Usuario = string.Empty;
            Contraseña = string.Empty;
            Fecha = DateTime.Now;
            TipodeUsuario = string.Empty;
        }
    }
}
