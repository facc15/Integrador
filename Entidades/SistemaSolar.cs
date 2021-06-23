using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class SistemaSolar<T> where T : Planeta
    {
        #region Atributos
        public List<T> lista;
        protected int capacidad;
        #endregion

        #region Constructores
        public SistemaSolar()
        {
            this.lista = new List<T>();
        }   

        public SistemaSolar(int capacidad):this()
        {
            this.capacidad = capacidad;
        }

        #endregion

        #region Métodos

        public bool Agregar(Planeta planeta)
        {
            if(this.lista.Count<this.capacidad)
            {
                this.lista.Add((T)planeta);
            }else
            {
                throw new NoHayLugarException();
            }
            
            return true;

        }
        #endregion
    }
}
