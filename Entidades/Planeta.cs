using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Entidades
{
    public class Planeta: ISerializable
    {
        #region Atributos
        public string nombre;
        public int satelites;
        public double gravedad;
        #endregion

        #region Constructores
        public Planeta() {}

        public Planeta(string nombre,int satelites,double gravedad)
        {
            this.nombre = nombre;
            this.satelites = satelites;
            this.gravedad = gravedad;
        }
        #endregion

        #region Métodos
        private string Mostrar()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Nombre: " + this.nombre);
            sb.AppendLine("Satelites: " + this.satelites.ToString());
            sb.AppendLine("Gravedad: " + this.gravedad.ToString());

            return sb.ToString();

        }

        public override string ToString()
        {
            return this.Mostrar();
        }
        #endregion

        #region Implementación de la Interface
        public string Path 
        { 
            get {
                string path=Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                path += "//integrador//planetaSerializado.Xml";
                
                return path; 
            
                } 
        }

        public bool SerializarXml()
        {
            try
            {
                using (XmlTextWriter w = new XmlTextWriter(this.Path, Encoding.UTF8))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Planeta));

                    ser.Serialize(w, this);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public string DeserializarXml()
        {
            Planeta aux;
            try
            {
                using (XmlTextReader r = new XmlTextReader(this.Path))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Planeta));

                    aux = (Planeta)ser.Deserialize(r);
                }

            }
            catch (Exception e)
            {

                return "";
            }
            return aux.ToString();
        }
        #endregion
    }
}
