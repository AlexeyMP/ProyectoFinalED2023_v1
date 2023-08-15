using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AppLibroRecetas
{
    public partial class FrmPrincipal : Form
    {
        //Se crea el arreglo con los elementos iniciales
        private string[] recetas = new string[3] { "Pastel", "Cheesecake", "Budin" };

        //Crear un diccionario en el que se almacenaran las listas enlazadas
        Dictionary<string, LinkedList<string>> diccionarioDeListas = new Dictionary<string, LinkedList<string>>();

        //Se crea una linkedlist para cada elemento inicial del arreglo
        private LinkedList<string> Pastel = new LinkedList<string>();

        //Se instancia el proceso de imprimir
        private PrintDocument printDocument = new PrintDocument();

        public FrmPrincipal()
        {
            InitializeComponent();
            printDocument.PrintPage += printDocument1_PrintPage;
        }

        /// <summary>
        /// Proceso que se ejecuta al iniciar el programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

            //Se crea la lista que se almacenara en el diccionario de listas:
            diccionarioDeListas.Add("Pastel", new LinkedList<string>());
            diccionarioDeListas.Add("Cheesecake", new LinkedList<string>());
            diccionarioDeListas.Add("Budin", new LinkedList<string>());

            //Se añaden elementos a la lista
            diccionarioDeListas["Pastel"].AddLast("Huevos");
            diccionarioDeListas["Pastel"].AddLast("Harina");
            diccionarioDeListas["Pastel"].AddLast("Mantequilla");
            diccionarioDeListas["Cheesecake"].AddLast("Huevos");
            diccionarioDeListas["Cheesecake"].AddLast("Harina");
            diccionarioDeListas["Cheesecake"].AddLast("Mantequilla");
            diccionarioDeListas["Budin"].AddLast("Huevos");
            diccionarioDeListas["Budin"].AddLast("Harina");
            diccionarioDeListas["Budin"].AddLast("Mantequilla");

            //Se llama al proceso
            this.deshabilitarBotones();

            //Se llama al proceso
            this.ActualizarDatos();
        }

        /// <summary>
        /// Proceso que se encarga de deshabilitar los botones de accion de los ingredientes
        /// </summary>
        public void deshabilitarBotones()
        {
            //Se deshabilitan los botones de accion de los ingredientes para evitar errores
            btnAgregar2.Enabled = false;
            btnEliminar2.Enabled = false;
            btnImprimir.Enabled = false;
            //Cambia el color de los botones deshabilitados
            btnAgregar2.BackColor = Color.LightGray;
            btnEliminar2.BackColor = Color.LightGray;
            btnImprimir.BackColor = Color.LightGray;
        }

        /// <summary>
        /// Proceso que se encarga de deshabilitar los botones de accion de los ingredientes
        /// </summary>
        public void habilitarBotones()
        {
            //Se deshabilitan los botones de accion de los ingredientes para evitar errores
            btnAgregar2.Enabled = true;
            btnEliminar2.Enabled = true;
            btnImprimir.Enabled = true;
            //Cambia el color de los botones deshabilitados
            btnAgregar2.BackColor = Color.White;
            btnEliminar2.BackColor = Color.White;
            btnImprimir.BackColor = Color.White;
        }

        /// <summary>
        /// Proceso que se encarga de ingresar los datos del arreglo en las rejillas del DataGridView del formulario
        /// </summary>
        public void ActualizarDatos()
        {
            try
            {
                //Agregar una columna como titulo
                dtgRecetas.Columns.Add("", "NOMBRE POSTRES:");

                //Recorrer el arreglo para almacenar sus elementos en cada columna del datagridview
                foreach (string receta in recetas)
                {
                    dtgRecetas.Rows.Add(receta);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Proceso encargado de la acción click del boton agregar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                //Establece que la nueva receta será el texto ingresado en la caja de texto del buscador
                string nuevaReceta = this.txtBuscador.Text;

                //Se verifica que el usuario haya ingresado un dato en la caja de texto, de lo contrario mostrara mensaje de error.
                if (txtBuscador.Text == "")
                {
                    throw new Exception("Debes ingresar el nombre de la receta en el buscador para poder agregarlo...");
                }
                else
                {
                    //Se recorre el arreglo para verificar existencias
                    foreach (string recorrido in recetas)
                    {
                        //Si el nombre de la receta que se quiere agregar ya existe en el arreglo, mostrara mensaje de error.
                        if (recorrido == nuevaReceta)
                        {
                            throw new Exception("El nombre de esta receta ya existe, verifique los valores ingresados...");
                        }
                    }

                    //Si no se cumple ninguno de los condicionales, se llama al proceso
                    AgregarPostre(nuevaReceta);
                    ingredientes.Items.Clear();

                    //Limpia la caja de texto despues del proceso
                    this.txtBuscador.Text = "";

                    //Se llama al proceso deshabilitarBotones
                    this.deshabilitarBotones();

                    //Se cambia el titulo del listbox
                    groupBox5.Text = "INGREDIENTES:";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Proceso encargado de agregar una nueva receta al arreglo
        /// </summary>
        /// <param name="receta"></param>
        private void AgregarPostre(string receta)
        {
            try
            {
                // Crear un nuevo arreglo con un tamaño mayor
                string[] nuevoArreglo = new string[recetas.Length + 1];

                // Copiar los elementos existentes al nuevo arreglo
                for (int i = 0; i < recetas.Length; i++)
                {
                    nuevoArreglo[i] = recetas[i];
                }

                // Agregar el nuevo elemento al final
                nuevoArreglo[nuevoArreglo.Length - 1] = receta;

                // Asignar el nuevo arreglo al arreglo actual
                recetas = nuevoArreglo;

                //Se crea la lista de ingredientes para el respectivo postre
                diccionarioDeListas.Add(this.txtBuscador.Text, new LinkedList<string>());

                //Limpia el datagridview
                dtgRecetas.Rows.Clear();

                //Vuelve a recorrer el arreglo para ingresar los datos en el datagridview
                foreach (string recorrido in recetas)
                {
                    dtgRecetas.Rows.Add(recorrido);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Proceso encargado de la acción click del boton eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                //Se crea una variable para verificar existencias en el proceso
                int existencias = -1;

                //Establece que la receta a eliminar será el texto ingresado en la caja de texto del buscador
                string eliminarReceta = txtBuscador.Text;

                //Se verifica que el usuario haya ingresado un dato en la caja de texto, de lo contrario mostrara mensaje de error.
                if (txtBuscador.Text == "")
                {
                    throw new Exception("Debes ingresar el nombre de la receta en el buscador para poder eliminarla...");
                }
                else
                {
                    //Se recorre el arreglo para verificar existencias
                    foreach (string recorrido in recetas)
                    {
                        //Si el nombre de la receta no existe en el arreglo, se mostrara un mensaje de error.
                        if (recorrido == eliminarReceta)
                        {
                            existencias++;
                            //Llama al proceso Eliminar postre
                            EliminarPostre(eliminarReceta);

                            //Limpia el listbox de ingredientes
                            ingredientes.Items.Clear();

                            //Elimina la lista del diccionario
                            diccionarioDeListas.Remove(this.txtBuscador.Text);
                        }
                    }

                    if (existencias == -1)
                    {
                        throw new Exception("El nombre de esta receta no existe, verifique los valores ingresados...");
                    }
                    
                    //Limpia la caja de texto despues del proceso
                    this.txtBuscador.Text = "";

                    //Se llama al proceso deshabilitarBotones
                    this.deshabilitarBotones();

                    //Se cambia el titulo del listbox
                    groupBox5.Text = "INGREDIENTES:";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 


        /// <summary>
        /// Proceso encargado de eliminar una receta del arreglo
        /// </summary>
        /// <param name="receta"></param>
        private void EliminarPostre(string receta)
        {
            try
            {
                //Se establece la variable del indice al que pertenece el arreglo que se desea eliminar
                int indiceAEliminar = -1;

                //Se busca el indice a eliminar recorriendo el arreglo ya existente
                for (int i = 0; i < recetas.Length; i++)
                {
                    //Si dentro del arreglo existe una receta con el mismo nombre de la ingresada en la caja de texto
                    //Se establece que esa posición del arreglo es el indice a eliminar y se rompe el for.
                    if (recetas[i] == receta)
                    {
                        indiceAEliminar = i;
                        break;
                    }
                }

                //Si el indice a liminar no es -1, quiere decir que se encontraron coincidencias, por lo tanto
                //se crea un nuevo arreglo con el postre ya eliminado
                if (indiceAEliminar != -1)
                {
                    // Crear un nuevo arreglo con un tamaño menor
                    string[] nuevoArreglo = new string[recetas.Length - 1];
                    Array.Copy(recetas, 0, nuevoArreglo, 0, indiceAEliminar);
                    Array.Copy(recetas, indiceAEliminar + 1, nuevoArreglo, indiceAEliminar, recetas.Length - indiceAEliminar - 1);

                    recetas = nuevoArreglo;

                    //Limpia el datagridview
                    dtgRecetas.Rows.Clear();

                    //Vuelve a recorrer el arreglo para ingresar los datos en el datagridview
                    foreach (string recorrido in recetas)
                    {
                        dtgRecetas.Rows.Add(recorrido);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// proceso que se encarga de mostrar la pantalla de los ingredientes en el momento de hacer click en el boton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        public void btnIngredientes_Click(object sender, EventArgs e)
        {
            try
            {
                //Se crea una variable para verificar existencias en el proceso
                int existencias = -1;

                //Se verifica que el usuario haya ingresado un dato en la caja de texto, de lo contrario mostrara mensaje de error.
                if (txtBuscador.Text == "")
                {
                    throw new Exception("Debes ingresar el nombre de la receta para poder acceder a sus ingredientes...");
                }
                else
                {
                    //Se recorre el arreglo para verificar existencias
                    foreach (string recorrido in recetas)
                    {
                        //Si el nombre de la receta no existe en el arreglo, se mostrara un mensaje de error.
                        if (recorrido == this.txtBuscador.Text)
                        {
                            existencias++;

                            //Se encarga de limpiar la lista
                            ingredientes.Items.Clear();

                            //Se habilitan los botones de accion de los ingredientes
                            this.habilitarBotones();

                            // Muestra la lista en el ListBox
                            foreach (string ingrediente in diccionarioDeListas[this.txtBuscador.Text])
                            {
                                ingredientes.Items.Add(ingrediente);
                            }

                            //Se cambia el titulo del listbox
                            groupBox5.Text = "INGREDIENTES:" + this.txtBuscador.Text;
                        }
                    }

                    if (existencias == -1)
                    {
                        throw new Exception("El nombre de esta receta no existe, verifique los valores ingresados...");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Proceso que se encarga de agregar ingredientes
        /// </summary>
        public void agregarIngrediente()
        {
            try
            {

                //Se encarga de agregar el ingrediente a la lista enlazada que le corresponte al postre
                diccionarioDeListas[this.txtBuscador.Text].AddLast(this.txtBuscador2.Text);

                //Se encarga de limpiar el listbox
                ingredientes.Items.Clear();

                // Muestra la lista en el ListBox para actualizar los datos
                foreach (string ingrediente in diccionarioDeListas[this.txtBuscador.Text])
                {
                    ingredientes.Items.Add(ingrediente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnAgregar2_Click(object sender, EventArgs e)
        {
            try
            {

                //Se verifica que el usuario haya ingresado un dato en la caja de texto, de lo contrario mostrara mensaje de error.
                if (this.txtBuscador2.Text == "")
                {
                    throw new Exception("Debes ingresar el nombre del ingrediente que deseas agregar a la receta...");
                }
                else
                {
                    //Se recorre el arreglo para verificar existencias
                    foreach (string recorrido in diccionarioDeListas[this.txtBuscador.Text])
                    {
                        //Si el nombre del ingrediente ya existe en la lista se mostrara un mensaje de error.
                        if (recorrido == this.txtBuscador2.Text)
                        {
                            throw new Exception("El nombre de este ingrediente ya existe en la receta, verifique los valores ingresados...");

                        }
                    }

                    //Se llama al proceso:
                    this.agregarIngrediente();

                    //Limpia la caja de texto despues del proceso
                    this.txtBuscador2.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Proceso que se encarga de eliminar ingredientes
        /// </summary>
        public void eliminarIngrediente()
        {
            try
            {

                //Se encarga de eliminar el ingrediente de la lista enlazada
                diccionarioDeListas[this.txtBuscador.Text].Remove(this.txtBuscador2.Text);

                //Se encarga de limpiar el listbox
                ingredientes.Items.Clear();

                // Muestra la lista en el ListBox para actualizar los datos
                foreach (string ingrediente in diccionarioDeListas[this.txtBuscador.Text])
                {
                    ingredientes.Items.Add(ingrediente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnEliminar2_Click(object sender, EventArgs e)
        {
            try
            {
                //Se crea una variable para verificar existencias en el proceso
                int existencias = -1;

                //Se verifica que el usuario haya ingresado un dato en la caja de texto, de lo contrario mostrara mensaje de error.
                if (this.txtBuscador2.Text == "")
                {
                    throw new Exception("Debes ingresar el nombre del ingrediente que deseas eliminar...");
                }
                else
                {
                    //Se recorre el arreglo para verificar existencias
                    foreach (string recorrido in diccionarioDeListas[this.txtBuscador.Text])
                    {
                        //Si el nombre del ingrediente no existe en la lista, se mostrara un mensaje de error.
                        if (recorrido == this.txtBuscador2.Text)
                        {
                            existencias++;

                        }
                    }

                    if (existencias == -1)
                    {

                        throw new Exception("El ingrediente ingresado no existe...");
                    }

                    //Llama al proceso:
                    this.eliminarIngrediente();
                    //Limpia la caja de texto despues del proceso
                    this.txtBuscador2.Text = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Imprimir el título
            e.Graphics.DrawString("INGREDIENTES PARA REALIZAR: " + this.txtBuscador.Text, new Font("Arial", 16, FontStyle.Bold), Brushes.Black, 100, 100);

            //Se establece la posicion vertical inicial
            float yPos = 150;
            foreach (var item in ingredientes.Items)
            {
                e.Graphics.DrawString(item.ToString(), new Font("Arial", 14, FontStyle.Bold), Brushes.Black, 100, yPos);
                //Se ajusta la posicion vertical para la siguiente linea
                yPos += 20;
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                //Se instancia la variable
                PrintDialog printDialog = new PrintDialog();

                //Se cambia la orientacion a horizontal
                printDocument1.DefaultPageSettings.Landscape = true;

                //Si el archivo es correcto se imprime
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void txtBuscador_TextChanged(object sender, EventArgs e)
        {
        }

        private void dtgRecetas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
