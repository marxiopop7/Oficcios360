namespace Oficcios360.Models
{
    public class FormatoPrestablecidoBD
    {
        public string ObtenerPlantillaCarta(Estudiante e)
        {
            // Motivación extendida y más profesional
            string motivacionLarga = "Mi interés en formar parte de su equipo radica en el prestigio de su organización y en mi firme deseo de aplicar las competencias técnicas y analíticas desarrolladas durante mi formación académica. Estoy convencido de que mi capacidad de adaptación, pensamiento crítico y compromiso con la excelencia me permitirán contribuir de manera significativa a sus objetivos institucionales, mientras continúo fortaleciendo mi perfil profesional bajo sus estándares de calidad.";

            // Formato de fecha corregido (sin el error '7e')
            string fechaLimpia = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            return $@"Bogotá D.C., {fechaLimpia}

Señores:
RECURSOS HUMANOS
Presente.

Asunto: Carta de Presentación - Modalidad {e.ModalidadGrado}

Yo, {e.Nombre.ToUpper()}, identificado con CC {e.Identificacion}, estudiante de la Universidad Distrital Francisco José de Caldas, me dirijo a ustedes con el fin de postularme formalmente para realizar mi {e.ModalidadGrado} en su prestigiosa empresa.

{e.PerfilProfesional}

{motivacionLarga}

Quedo a su entera disposición para concertar una entrevista y profundizar en los detalles de mi perfil. Agradezco de antemano la atención prestada.

Atentamente,

{e.Nombre}
CC. {e.Identificacion}
Tel: {e.Telefono}";
        }
    }
}