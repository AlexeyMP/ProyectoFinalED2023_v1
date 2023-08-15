using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalED2023_v1
{
    public partial class FormPrincipal : Form
    {
        private LinkedList<int> lista = new LinkedList<int>(); // Declaración de una lista enlazada de enteros llamada 'lista'

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void buttonInicio_Click(object sender, EventArgs e)
        {

            lista.Clear(); // Reinicia la lista


            // Obtén los números ingresados en textBoxInicio
            string inputText = textBoxInicio.Text;

            // Divide los números por coma y conviértelos en un array de strings
            string[] numberStrings = inputText.Split(',');

            // Intenta convertir los strings en enteros
            if (numberStrings.Length == 2 && int.TryParse(numberStrings[0], out int num1) && int.TryParse(numberStrings[1], out int num2))
            {
                // Verifica si los números son 12 y 18, o viceversa
                if ((num1 == 12 && num2 == 18) || (num1 == 18 && num2 == 12))
                {
                    // Genera la lista completa entre 12 y 18
                    for (int i = Math.Min(num1, num2); i <= Math.Max(num1, num2); i++)
                    {
                        lista.AddLast(i);
                    }

                    // Muestra la lista en el ListBox
                    foreach (int number in lista)
                    {
                        listBoxCompleta.Items.Add(number);
                    }
                }
                else
                {
                    MessageBox.Show("Los números ingresados no son válidos. Debe ser 12 y 18, en cualquier orden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Ingrese dos números enteros separados por coma.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        private void CompleteList()
        {
            LinkedListNode<int> current = lista.First; // Obtiene el primer nodo de la lista
            int nextNumber = current.Value + 1; // Calcula el siguiente número a insertar en la lista

            while (current.Next != null) // Mientras haya un nodo siguiente en la lista
            {
                while (nextNumber < current.Next.Value) // Mientras el siguiente número sea menor que el valor del nodo siguiente
                {
                    lista.AddAfter(current, nextNumber); // Agrega el siguiente número después del nodo actual
                    nextNumber++; // Incrementa el siguiente número
                }

                current = current.Next; // Avanza al siguiente nodo en la lista
                nextNumber = current.Value + 1; // Calcula el siguiente número basado en el valor del nodo actual
            }
        }

        private void buttonBorrarListas_Click(object sender, EventArgs e)
        { 
            listBoxCompleta.Items.Clear(); // Limpia los elementos en el ListBox 'listBoxCompleta'
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); // Cierra el formulario (y el programa completo)
        }

  

    

       
    } // cierre
 } // cierre
