using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oficcios360.Models
{
    public class Estudiante : Usuario
    {
        public string? Telefono { get; set; }
        public string? Ciudad { get; set; }
        public string ModalidadGrado { get; set; } = "Pasantías";
        public string? PerfilProfesional { get; set; }

        
        public string? LugarFechaNacimiento { get; set; }
        public string? Sexo { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Direccion { get; set; }

        public virtual List<FormacionAcademica> Formaciones { get; set; } = new List<FormacionAcademica>();
    }

    public class FormacionAcademica
    {
        [Key]
        public int Id { get; set; }
        public string EstudianteId { get; set; } = string.Empty;
        public string Institucion { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public int AnioFin { get; set; }
    }
}