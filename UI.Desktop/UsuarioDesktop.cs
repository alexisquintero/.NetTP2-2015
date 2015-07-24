﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business.Logic;
using Business.Entities;
using System.Text.RegularExpressions;

namespace UI.Desktop
{
    public partial class UsuarioDesktop : ApplicationForm
    {
        public UsuarioDesktop()
        {
            InitializeComponent();
        }

        public Usuario UsuarioActual;

        public UsuarioDesktop(ModoForm modo)
            : this()
        {
            //Internamete debe setear a ModoForm en el modo enviado, este constructor servirá para las altas.
        }

        public UsuarioDesktop(int ID, ModoForm modo)
            : this()
        {
            //En este nuevo constructor seteamos el modo que ha sido especificado en el parámetro e instanciamos un nuevo objeto de UsuarioLogic y
            //utilizamos el método GetOne para recuperar la entidad Usuario. Entonces la asignamos a la propiedad UsuarioActual e invocaremos al método
            //MapearDeDatos.
            UsuarioActual = ul.GetOne(ID);
            this.MapearDeDatos();
        }

        public virtual void MapearDeDatos() {

            // txtClave, txtConfirmarClave
            this.txtID.Text = this.UsuarioActual.ID.ToString();
            this.chkHabilitado.Checked = this.UsuarioActual.Habilitado;
            this.txtNombre.Text = this.UsuarioActual.Nombre;
            this.txtApellido.Text = this.UsuarioActual.Apellido;
            this.txtEmail.Text = this.UsuarioActual.Email;
            this.txtUsuario.Text = this.UsuarioActual.NombreUsuario;
//            this.txtClave.Text = "******";

            //Dentro del mismo método setearemos el texto del botón Aceptar en función del Modo del formulario de esta forma 
            //Alta o Modificación Guardar 
            //Baja Eliminar 
            //Consulta Aceptar

//TODO: Arreglar función con enum.
            int i = 5;
            switch( i)
            {
                case 0:
                    this.btnAceptar.Name = "Guardar";
                    break;
                case 1:
                    this.btnAceptar.Name = "Eliminar";
                    break;
                case 2:
                    this.btnAceptar.Name = "Guardar";
                    break;
                default:
                    //this.btnAceptar.Name = "Aceptar";
                    break;
            }
        
        }
        public virtual void MapearADatos() {

//TODO: arreglar, enum            if(Modo del formulario es Alta){
                UsuarioActual = new Usuario();
//TODO: getid de algun lado, método de clase?                UsuarioActual.ID = 0; 
//            }

//TODO: arreglar, enum            if(Modo del formulario es Alta){
                UsuarioActual.ID = int.Parse(this.txtID.Text);
//            }

            this.UsuarioActual.Habilitado = this.chkHabilitado.Checked;
            this.UsuarioActual.Nombre = this.txtNombre.Text;
            this.UsuarioActual.Apellido = this.txtApellido.Text;
            this.UsuarioActual.Email = this.txtEmail.Text;
            this.UsuarioActual.NombreUsuario = this.txtUsuario.Text;

//TODO: arreglar, enum            this.UsuarioActual.State = ModoForm;

        
        }
        public virtual void GuardarCambios() {

            MapearADatos();

            UsuarioLogic uL = new UsuarioLogic();

            uL.Save(UsuarioActual);
        
        }
        public virtual bool Validar() {

            if (this.txtID.Text == "" || this.txtApellido.Text == "" || this.txtNombre.Text == "" || this.txtEmail.Text == "" || this.txtUsuario.Text == "" ||
                this.txtClave.Text == "" || this.txtConfirmarClave.Text == "")
            {
//TODO: agregar parámetros                this.Notificar();
            };

            if (this.txtClave.Text.Length < 8)
            {
//TODO: agregar parámetros                this.Notificar();
            }

            if(this.txtClave.Text != this.txtConfirmarClave.Text )
            {
//TODO: agregar parámetros                this.Notificar();
            }

            bool emailValido = false;  
            string rgxPattern = "\\S+" ; //texto sin espacios y con por lo menos 1 caracter
            Regex rgx = new Regex(rgxPattern, RegexOptions.IgnoreCase);  
            MatchCollection matches;
            string[] cadenas = this.txtEmail.Text.Split('@');

            if (cadenas.Length == 2)  // 1 solo @
            {
                matches = rgx.Matches(cadenas[0]); // regex antes del @
                if(matches.Count !=0)
                {
                    cadenas = cadenas[1].Split('.');
                    if(cadenas.Length == 2)  // 1 punto despues del @
                    {
                        if(cadenas[1].Length == 3)  // 3 caracteres despues del punto; .com, .edu, etc.
                        {
                            matches = rgx.Matches(cadenas[1]);

                            if (matches.Count != 0)  // regex despues del .
                            {
                                emailValido = true;
                            }
                        }
                    }
                    if(cadenas.Length == 3)  // 2 puntos despues del @
                    {
                        if (cadenas[1].Length == 3)  // 3 caracteres despues del primer punto; .com, .edu, etc.
                        {
                            matches = rgx.Matches(cadenas[1]);

                            if (matches.Count != 0)  // regex despues del primer .
                            {
                                if (cadenas[2].Length == 2)  // 2 caracteres despues del segundo punto; .ar, .it, etc.
                                {
                                    matches = rgx.Matches(cadenas[2]);

                                    if (matches.Count != 0)  // regex despues del segundo .
                                    {
                                        emailValido = true;
                                    }
                                }

                            }
                        }
                    }
                }


            }

            if (emailValido == false)
            {
//TODO: agregar parámetros                this.Notificar();
            }
        
//TODO:   si es todo válido debe llamar retornar true.
        
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Validar())
            {
                GuardarCambios();
                this.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();  
            /* "20.También creamos el manejador del evento clic para el botón salir y allí
utilizamos el método"         no dice nada más */
        }

//TODO: arreglar la siguiente función, "21.Volvemos al Windows Form Usuarios y en el ToolStripButton tbsNuevo
//      hacemos doble clic para manejar el evento clic." , tbsNuevo no aparece antes, no sé que tiene o hace

        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            UsuarioDesktop formUsuario = new UsuarioDesktop(ApplicationForm.ModoForm.Alta);
            formUsuario.ShowDialog();
//            this.Listar();    
    
/* 22.Allí creamos una variable de tipo UsuarioDesktop, la instanciamos
invocando al constructor con un parámetro y pasanadole el modo alta.
Luego mostramos el formulario con ShowDialog y a continuación
refrescamos la grilla llamando al método Listar, para que al finalizar  --> Grilla?!
la ejecución de la nueva alta si se agregó un usuario este se vea en la
grilla. El código será similar a este:
 private void tsbNuevo_Click(object sender, EventArgs e)
 {
 UsuarioDesktop formUsuario = new UsuarioDesktop(ApplicationForm.ModoForm.Alta);
 formUsuario.ShowDialog();
 this.Listar();
 }
        }

//TODO:
/*24.De manera similar llamar al Formulario crear los manejadores de eventos
para el Editar y el Eliminar. Utilizando el constructor de
UsuarioDesktop que requiere enviar el ID y el Modo
Para obtener el ID podemos hacerlo de la siguiente forma
int ID = ((Business.Entities.Usuario)this.dgvUsuarios.SelectedRows[0].DataBoundItem).ID;
Para poder utilizar dicha línea de código debemos:
• Asegurarnos que haya una fila seleccionada (controlando que
this.dgvUsuarios.SelectedRows tenga elementos dentro)
• Permitir que se selecciones una única fila (Propiedad MultiSelect de
la grilla en false)
• Que al hacer clic en una celda se seleccione una fila entera
(Propiedad SelectionMode de la grilla en FullRowSelect)

    }
}