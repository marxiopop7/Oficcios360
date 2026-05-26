using System;
using System.Collections.Generic;
using System.Linq;
using Oficcios360.Models;

namespace Oficcios360.Helpers
{
    public class GestorRegistro
    {
        
        public double SumarHorasAutomaticas(List<RegistroActividad> listaActividades)
        {
            if (listaActividades == null) return 0.0;
            return listaActividades.Sum(a => a.HorasDedicadas);
        }

        
        public double CalcularPorcentajeProgreso(double horasActuales, double horasRequeridas)
        {
            if (horasActuales <= 0 || horasRequeridas <= 0) return 0.0;

            double porcentaje = (horasActuales / horasRequeridas) * 100;
            return porcentaje > 100.0 ? 100.0 : Math.Round(porcentaje, 2);
        }

       
        public bool ValidarMetaHorasCumplida(double horasActuales, double horasRequeridas)
        {
            if (horasRequeridas <= 0) return false;
            return horasActuales >= horasRequeridas;
        }
    }
}