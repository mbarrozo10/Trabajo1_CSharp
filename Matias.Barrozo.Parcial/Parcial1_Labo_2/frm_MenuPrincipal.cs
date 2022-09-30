﻿using Biblioteca;
using Parcial1_Labo_2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcial1_Labo_2
{
    public partial class frm_MenuPrincipal : Form
    {

        private static int Index;
        private DateTime horaPrincipal;
        private float recaudacionCabotaje = 0;
        private float recaudacionInternacional = 0;
        private int indiceVendedor;
        private Vendedor vendedorActivo = new Vendedor();

        public frm_MenuPrincipal()
        {
            InitializeComponent();

        }

        public frm_MenuPrincipal(Vendedor vendedor) : this()
        {
            lbl_Bienvenido.Text = "Bienvenidx " + vendedor.ToString() ;
            vendedorActivo = vendedor;
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            
            CargarDatagrid();
            modoOscuroClaro();
            cmb_FiltroHistorico.Items.Add("Destinos");
            cmb_FiltroHistorico.Items.Add("Clientes Frecuentes");
            cmb_FiltroHistorico.Items.Add("Ganancias totales");
            cmb_FiltroHistorico.Items.Add("Vuelos finalizados");
            cmb_FiltroHistorico.Items.Add("Aviones");
            cmb_FiltroHistorico.Items.Add("Vendedores");
            tmr_Hora.Start();
            horaPrincipal = DateTime.Now;
            if(vendedorActivo.Tipo!= "administrador")
            {
                pic_Agregar.Visible = false;
                
            }
        }

        private void btn_Salir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void dgv_VuelosActivos_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Index = e.RowIndex;
            if (Aerolinea.vuelos[Index].Estado == Biblioteca.Estado.Disponible && Aerolinea.vuelos[Index].Pasajeros.Count < Aerolinea.vuelos[Index].Avion.CantidadAsientos)
            {
               pic_Vender.Visible = true;
               pic_Pasajeros.Visible = true;
            }
            else if (Aerolinea.vuelos[Index].Estado == Biblioteca.Estado.EnVuelo || (Aerolinea.vuelos[Index].Estado == Biblioteca.Estado.Lleno || Aerolinea.vuelos[Index].Estado== Biblioteca.Estado.EnVuelo))
            {
                pic_Pasajeros.Visible = true;
                pic_Vender.Visible = false;
            }
            else
            {
                pic_Vender.Visible = false;
                pic_Pasajeros.Visible = false;
            }

            pic_MasInfo.Enabled = true;

        }

        private void btn_Pasajeros_Click(object sender, EventArgs e)
        {
            frm_pasajeros pj = new frm_pasajeros(Index);
            pj.ShowDialog();
        }

        private void CargarDatagrid()
        {
            dgv_VuelosActivos.DataSource = null;
            dgv_VuelosActivos.Rows.Clear();
            for (int i = 0; i < Aerolinea.vuelos.Count; i++)
            {
                DataGridViewRow filas = new DataGridViewRow();
               
                filas.CreateCells(dgv_VuelosActivos);
              
                filas.Cells[0].Value = Aerolinea.vuelos[i].Avion.Matricula;
                filas.Cells[1].Value = Aerolinea.vuelos[i].Duracion;
                filas.Cells[2].Value = Aerolinea.vuelos[i].Origen;
                filas.Cells[3].Value = Aerolinea.vuelos[i].Destino;
                filas.Cells[4].Value = Aerolinea.vuelos[i].Salida;
                filas.Cells[5].Value = Aerolinea.vuelos[i].Llegada; 
                filas.Cells[6].Value = Aerolinea.vuelos[i].Estado;
                filas.Cells[7].Value = Aerolinea.vuelos[i].Avion.CantidadAsientos - Aerolinea.vuelos[i].Pasajeros.Count;
                filas.Cells[8].Value = Aerolinea.vuelos[i].Avion.Wifi;
                filas.Cells[9].Value = Aerolinea.vuelos[i].Avion.Comida;
               
                dgv_VuelosActivos.Rows.Add(filas);

            }
        }

        private void btn_AgregarVuelo_Click(object sender, EventArgs e)
        {
            frm_AgregarVuelo vuelo = new frm_AgregarVuelo();
            if (vuelo.ShowDialog() == DialogResult.OK)
            {
                CargarDatagrid();
            }
        }

        private void btn_Vender_Click(object sender, EventArgs e)
        {
            frm_Venta venta = new frm_Venta(Index);
            if (venta.ShowDialog() == DialogResult.OK)
            {
                if (Aerolinea.vuelos[Index].Pasajeros.Count == Aerolinea.vuelos[Index].Avion.CantidadAsientos)
                {
                    Aerolinea.vuelos[Index].Estado = Biblioteca.Estado.Lleno;
                }
                CargarDatagrid();
            }
        }

        private void pic_Modo_Click(object sender, EventArgs e)
        {
            modoOscuroClaro();
        }

        private void btn_MasInfo_Click(object sender, EventArgs e)
        {
            pnl_Informacion.Visible = true;
            pnl_Informacion.Enabled = true;
            CargarCerrarPaneles(false);
            dgv_VuelosActivos.Visible = true;

            rtx_InfoVuelo.Text = Aerolinea.vuelos[Index].ToString();
        }

        private void pic_SalirPanelInfo_Click(object sender, EventArgs e)
        {
            pnl_Informacion.Visible = false;
            CargarCerrarPaneles(true);
        }

        private void pic_CancelarVuelo_Click(object sender, EventArgs e)
        {
            Aerolinea.vuelos[Index].Estado = Biblioteca.Estado.Cancelado;
            Aerolinea.vuelos.Remove(Aerolinea.vuelos[Index]);
            CargarCerrarPaneles(true);
            CargarDatagrid();
            Index = 0;

        }

        private void CargarCerrarPaneles(bool estado)
        {
            pic_Agregar.Visible = estado;
            pic_Vender.Visible = estado;
            pic_Salir.Visible = estado;
            pic_MasInfo.Visible = estado;
            pic_Historico.Visible = estado;
            pic_Modo.Enabled = estado;
            dgv_VuelosActivos.Visible = estado;
            pic_AgregarCliente.Visible = estado;

        }

        private void btn_Historico_Click(object sender, EventArgs e)
        {
            pnl_Historico.Show();
            CargarCerrarPaneles(false);
      
            lbl_RecaudacionTotal.Text = "Recaudacion total: " + Aerolinea.CalcularRecaudacionTotal().ToString();
            pic_Pasajeros.Visible = false;
            rtx_InfoAvion.Visible = false;

        }

     
        private void pic_SalirHistorico_Click_1(object sender, EventArgs e)
        {
            pnl_Historico.Hide();
            if (vendedorActivo.Tipo == "administrador")
            {
                pic_Agregar.Visible = true;

            }
            CargarCerrarPaneles(true);
            cmb_FiltroHistorico.Text = String.Empty;
            dgv_Historico.DataSource = null;
            pic_AgregarCliente.Visible = true;
            LimpiarHistorico();
            dgv_Historico.DataSource = null;
            dgv_Historico.Visible = false;

        }

        private void LimpiarHistorico()
        {
            pic_ModificarAceptar.Visible = false;
            lbl_TituloABM.Visible = false;
            chk_Admin.Visible = false;
            pic_AgregarVendedor.Visible = false;
            txt_ContraseñaVendedor.Visible = false;
            txt_ApellidoVendedor.Visible = false;
            txt_NombreVendedor.Visible = false;
            txt_UsuarioVendedor.Visible = false;
            pic_Borrar.Visible = false;
            dgv_Vendedores.Visible = false;
            pic_Modificar.Visible = false;
            lbl_InfoVendedor.Visible = false;
            rtx_InfoAvion.Visible = false;     
        }

        private void cmb_FiltroHistorico_SelectedValueChanged(object sender, EventArgs e)
        {
            List <Cliente> listaClientesFrecuentes = new List<Cliente>();
            rtx_InfoAvion.Clear();
            lbl_RecaudacionInternacional.Text=String.Empty;
            lbl_RecaudacionCabotaje.Text = String.Empty;
            dgv_Historico.Visible = true;
            lbl_ErrorBorrar.Text = String.Empty;
            LimpiarHistorico();
       
            switch (cmb_FiltroHistorico.Text)
            {
                case "Destinos":
                    dgv_Historico.Columns.Clear();
                    dgv_Historico.DataSource = null;
                    dgv_Historico.Columns.Add("Destino", "Destino");
                    dgv_Historico.Columns.Add("CantidadViajes", "Cantidad de viajes");
                    dgv_Historico.Columns.Add("TotalRecaudado", "TotalRecaudado");
                    dgv_Historico.Columns.Add("CantidadPasajeros", "Cantidad de pasajeros");
                    RecorrerVuelos();
                    dgv_Historico.Sort(dgv_Historico.Columns[2], ListSortDirection.Descending);
                    break;
                case "Clientes Frecuentes":
                    foreach(Cliente cliente in Aerolinea.clientes)
                    {
                        if (cliente.CantPasajesComprados > 2)
                        {
                            listaClientesFrecuentes.Add(cliente);
                        }
                    }
                    dgv_Historico.Columns.Clear();
                    dgv_Historico.DataSource = null;
                   
                    dgv_Historico.DataSource = listaClientesFrecuentes;
                    break;
                case "Ganancias totales":
                    dgv_Historico.Columns.Clear();
                    dgv_Historico.DataSource = null;
                    dgv_Historico.Columns.Add("Codigo", "Codigo de vuelo");
                    dgv_Historico.Columns.Add("Destino", "Destino");
                    dgv_Historico.Columns.Add("Ganancia", "Ganancia del vuelo");
                    dgv_Historico.Columns.Add("TipoServicio", "Tipo de servicio");
                    RecorrerVuelos(Aerolinea.vuelos);
                    RecorrerVuelos(Aerolinea.vuelosFinalizados);                 
                    break;
                case "Vuelos finalizados":
                    dgv_Historico.DataSource = null;
                    dgv_Historico.Columns.Clear();
                    dgv_Historico.DataSource = null;
                    dgv_Historico.Columns.Add("Avion", "Avion");
                    dgv_Historico.Columns.Add("Codigo", "Codigo");
                    dgv_Historico.Columns.Add("Duracion", "Duracion");
                    dgv_Historico.Columns.Add("Origen", "Origen");
                    dgv_Historico.Columns.Add("Destino", "Destino");
                    dgv_Historico.Columns.Add("Salida", "Salida");
                    dgv_Historico.Columns.Add("Llegada", "Llegada");
                    dgv_Historico.Columns.Add("CantPasajeros", "Pasajeros");
                    dgv_Historico.Columns.Add("Recaudacion", "Recaudacion");
                    foreach( Vuelo vuelo in Aerolinea.vuelosFinalizados)
                    {
                        DataGridViewRow filas = new DataGridViewRow();
                        filas.CreateCells(dgv_Historico);

                        filas.Cells[0].Value = vuelo.Avion.Matricula;
                        filas.Cells[1].Value = vuelo.Codigo;
                        filas.Cells[2].Value = vuelo.Duracion;
                        filas.Cells[3].Value = vuelo.Origen;
                        filas.Cells[4].Value = vuelo.Destino;
                        filas.Cells[5].Value = vuelo.Salida;
                        filas.Cells[6].Value = vuelo.Llegada;
                        filas.Cells[7].Value = vuelo.Pasajeros.Count;
                        filas.Cells[8].Value = vuelo.Recaudacion;

                        dgv_Historico.Rows.Add(filas);
                    }
                    break;
                case "Aviones":
                    dgv_Historico.DataSource = null;
                    dgv_Historico.Visible = false;
                    rtx_InfoAvion.Visible = true;
                    foreach (Avion aviones in Aerolinea.aviones)
                    {
                        rtx_InfoAvion.Text+= aviones.ToString() + "\n";
                    }
                    break;
                case "Vendedores":
                    if (vendedorActivo.Tipo == "usuario")
                    {
                        dgv_Historico.Visible = false;
                        rtx_InfoAvion.Visible = true;
       
                        string texto="";
                        foreach(Vendedor ven in Aerolinea.Vendedores)
                        {
                            texto += ven.MostrarInformacion() + "\n\n";
                        }
                        rtx_InfoAvion.Text = texto;
                    }
                    else
                    {
                        dgv_Vendedores.Visible = true;
                        pic_Borrar.Visible = true;
                        dgv_Historico.Visible = false;
                        dgv_Vendedores.DataSource = Aerolinea.Vendedores;
                        lbl_ErrorBorrar.Visible = true;
                        pic_AgregarVendedor.Visible = true;
                        txt_ApellidoVendedor.Visible = true;
                        txt_ContraseñaVendedor.Visible = true;
                        txt_NombreVendedor.Visible = true;
                        txt_UsuarioVendedor.Visible = true;
                        chk_Admin.Visible = true;
                        lbl_TituloABM.Visible = true;
                        pic_Modificar.Visible = true;
                    }
                    break;

                default:
                    dgv_Historico.DataSource = null;
                    break;
            }
        }

      

        private void btn_AgregarCliente_Click(object sender, EventArgs e)
        {
            frm_AgregarCliente frm = new frm_AgregarCliente();
            frm.ShowDialog();
        }


        private void RecorrerVuelos(List<Vuelo> vuelos)
        {
            
            for (int i = 0; i < vuelos.Count; i++)
            {

                DataGridViewRow filas = new DataGridViewRow();

                filas.CreateCells(dgv_Historico);

                filas.Cells[0].Value = vuelos[i].Codigo;
                filas.Cells[1].Value = vuelos[i].Destino;
                filas.Cells[2].Value = vuelos[i].Recaudacion;


                if (vuelos[i].Destino.ToString() == "Recife" || vuelos[i].Destino.ToString() == "Roma"
                    || vuelos[i].Destino.ToString() == "Acapulco" || vuelos[i].Destino.ToString() == "Miami")
                {
                    filas.Cells[3].Value = "Internacional";
                    recaudacionInternacional += vuelos[i].Recaudacion;
                }
                else
                {
                    filas.Cells[3].Value = "Cabotaje";
                    recaudacionCabotaje += vuelos[i].Recaudacion;
                }

                dgv_Historico.Rows.Add(filas);

            }
            lbl_RecaudacionCabotaje.Text = "Recaudacion cabotaje: " + recaudacionCabotaje;
            lbl_RecaudacionInternacional.Text = "Recaudacion internacional: " + recaudacionInternacional;

        }

        private void RecorrerVuelos()
        {
            int count = Enum.GetValues(typeof(Destino)).Length;
            for (int i = 0; i < count; i++)
            {
                int contador = 0;
                float recaudacion = 0;
                int cantidadPasajeros=0;
                for (int j = 0; j < Aerolinea.vuelos.Count; j++)
                {
                    string? s = Enum.GetName(typeof(Destino), i);
                    if (Aerolinea.vuelos[j].Destino.ToString().Contains(s))
                    {
                        contador++;
                        recaudacion += Aerolinea.vuelos[j].Recaudacion;
                        cantidadPasajeros+=Aerolinea.vuelos[j].Pasajeros.Count;
                    }
                }
                for (int j = 0; j < Aerolinea.vuelosFinalizados.Count; j++)
                {
                    string? s = Enum.GetName(typeof(Destino), i);
                    if (Aerolinea.vuelosFinalizados[j].Destino.ToString().Contains(s))
                    {
                        contador++;
                        recaudacion += Aerolinea.vuelosFinalizados[j].Recaudacion;
                        cantidadPasajeros += Aerolinea.vuelosFinalizados[j].Pasajeros.Count;
                    }
                }
                DataGridViewRow filas = new DataGridViewRow();

                filas.CreateCells(dgv_Historico);

                filas.Cells[0].Value = Enum.GetName(typeof(Destino), i);
                filas.Cells[1].Value = contador;
                filas.Cells[2].Value = recaudacion;
                filas.Cells[3].Value =cantidadPasajeros;

                dgv_Historico.Rows.Add(filas);

            }
        }

        private void tmr_Hora_Tick(object sender, EventArgs e)
        {
            horaPrincipal = DateTime.Now;
            lbl_Fecha.Text = horaPrincipal.ToString("dd/MM/yyyy HH:mm:ss");

            RevisarVuelo();
        }


        private void RevisarVuelo()
        {
            foreach(Vuelo vuelo in Aerolinea.vuelos)
            {
                if (horaPrincipal.CompareTo(vuelo.Salida) > 0 && (vuelo.Estado== Biblioteca.Estado.Disponible || vuelo.Estado==Biblioteca.Estado.Lleno))

                {
                    vuelo.Estado = Biblioteca.Estado.EnVuelo;
                    CargarDatagrid();
                }
                 if(horaPrincipal.CompareTo(vuelo.Llegada) > 0 && vuelo.Estado== Biblioteca.Estado.EnVuelo)
                {
                    vuelo.Estado = Biblioteca.Estado.Finalizado;
                    vuelo.Avion.HorasVuelo += vuelo.Duracion;
                    vuelo.Avion.Disponible = true;
                    BorrarVuelo(vuelo);
                    CargarDatagrid();
                    break;
                }
            }
            
        }

        private void BorrarVuelo(Vuelo vuelo)
        {
            
            Aerolinea.vuelosFinalizados.Add(vuelo);
            Aerolinea.vuelos.Remove(vuelo);
        }

        private void dgv_Vendedores_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            indiceVendedor= e.RowIndex;
            
        }

        private void pic_Borrar_Click(object sender, EventArgs e)
        {
            if(vendedorActivo!= Aerolinea.Vendedores[indiceVendedor])
            {
                Aerolinea.Vendedores.Remove(Aerolinea.Vendedores[indiceVendedor]);
                dgv_Vendedores.DataSource = null;
                dgv_Vendedores.DataSource = Aerolinea.Vendedores;
                indiceVendedor = 0;
            }
            else
            {
                lbl_ErrorBorrar.Text = "Error, no puede borrar su usuario";
            }
        }

        private void pic_AgregarVendedor_Click(object sender, EventArgs e)
        {
            string admin;
            if(txt_ApellidoVendedor.Text!=String.Empty && txt_ContraseñaVendedor.Text!=String.Empty
                &&txt_NombreVendedor.Text!= String.Empty && txt_UsuarioVendedor.Text!= String.Empty)
            {
                if (chk_Admin.Checked)
                    admin = "administrador";
                else
                    admin = "usuario";

                Aerolinea.Vendedores.Add(new Vendedor(txt_NombreVendedor.Text,txt_ApellidoVendedor.Text,txt_UsuarioVendedor.Text,txt_ContraseñaVendedor.Text,admin));
                dgv_Vendedores.DataSource = null;
                dgv_Vendedores.DataSource = Aerolinea.Vendedores;
                LimpiarPanelVendedor();
            }
            else
            {
                lbl_ErrorBorrar.Text = "Falta algun dato";
            }

            
        }

        private void pic_Modificar_Click(object sender, EventArgs e)
        {
            if (indiceVendedor >= 0)
            {
                pic_AgregarVendedor.Visible = false;
                pic_ModificarAceptar.Visible = true;
                lbl_InfoVendedor.Visible = true;
                pic_Borrar.Visible = false;
                lbl_InfoVendedor.Text ="Vendedor a modificar: \n"+ Aerolinea.Vendedores[indiceVendedor].MostrarInformacion();
                lbl_TituloABM.Text = "Ingrese el campo que quiera modificar";
                if (Aerolinea.Vendedores[indiceVendedor]== vendedorActivo)
                {
                    chk_Admin.Checked = true;
                    chk_Admin.Enabled = false;
                }
            }
        }

        private void pic_ModificarAceptar_Click(object sender, EventArgs e)
        {
            
            string tipo;
            if (chk_Admin.Checked)
                tipo = "administrador";
            else 
                tipo = "usuario";
           
            Aerolinea.Vendedores[indiceVendedor].ModificarVendedor(txt_NombreVendedor.Text, txt_ApellidoVendedor.Text, txt_UsuarioVendedor.Text, txt_ContraseñaVendedor.Text, tipo);
            lbl_TituloABM.Text = "Agregar usuario";
            pic_AgregarVendedor.Visible = true;
            lbl_InfoVendedor.Visible = false;
            dgv_Vendedores.DataSource = null;
            dgv_Vendedores.DataSource = Aerolinea.Vendedores;
            chk_Admin.Enabled = true;
            pic_Borrar.Visible = true;
            pic_ModificarAceptar.Visible = false;
            LimpiarPanelVendedor();
        
        }

        private void LimpiarPanelVendedor()
        {
            txt_ApellidoVendedor.Clear();
            txt_ContraseñaVendedor.Clear();
            txt_NombreVendedor.Clear();
            txt_UsuarioVendedor.Clear();
            chk_Admin.Checked = false;

        }

        //Modo oscuro

        private void modoOscuroClaro()
        {
            if (Aerolinea.modoOscuro)
            {
                pic_Modo.Image = Resources.dia;
                this.BackColor = Color.FromArgb(34, 34, 34);
                Aerolinea.modoOscuro = false;
                pic_Pasajeros.Image = Resources.pasajeros_blanco_84x24;
                pic_Salir.Image = Resources.salir_blanco_84x24;
                pic_Agregar.Image = Resources.agregar_blanco;
                pic_Vender.Image = Resources.vender_blanco_84x24;
                lbl_Bienvenido.ForeColor = Color.White;
                dgv_VuelosActivos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 34, 34);
                dgv_VuelosActivos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_VuelosActivos.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 34, 34);
                dgv_VuelosActivos.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_VuelosActivos.RowsDefaultCellStyle.BackColor = Color.FromArgb(34, 34, 34);
                dgv_VuelosActivos.RowsDefaultCellStyle.ForeColor = Color.White;
                lbl_Fecha.ForeColor = Color.White;
                pic_SalirHistorico.Image = Resources.salir_blanco_84x24;
                pic_Cancelar.Image = Resources.cancelar_blanco_84x24;
                rtx_InfoVuelo.ForeColor = Color.White;
                rtx_InfoVuelo.BackColor = Color.FromArgb(34, 34, 34);
                pic_SalirHistorico.Image = Resources.salir_blanco_84x24;
                cmb_FiltroHistorico.ForeColor = Color.White;
                cmb_FiltroHistorico.BackColor = Color.FromArgb(34, 34, 34);
                lbl_RecaudacionCabotaje.ForeColor = Color.White;
                lbl_RecaudacionInternacional.ForeColor = Color.White;
                lbl_RecaudacionTotal.ForeColor = Color.White;
                pic_SalirPanelInfo.Image = Resources.salir_blanco_84x24;
                pic_AgregarCliente.Image = Resources.Agregar_cliente_blanco;
                pic_MasInfo.Image = Resources.info_blanco;
                pic_Historico.Image = Resources.historico_84x24_blanco;
                pic_Borrar.Image = Resources.borrar_blanco;
                lbl_ErrorBorrar.ForeColor = Color.White;
                lbl_TituloABM.ForeColor = Color.White;
                lbl_InfoVendedor.ForeColor=Color.White;
                pic_Modificar.Image = Resources.modificar_blanco3;
                pic_ModificarAceptar.Image = Resources.modificar_blanco3;
                txt_ApellidoVendedor.BackColor = Color.FromArgb(34,34,34);
                txt_ContraseñaVendedor.BackColor = Color.FromArgb(34, 34, 34);
                txt_NombreVendedor.BackColor = Color.FromArgb(34, 34, 34);
                txt_UsuarioVendedor.BackColor = Color.FromArgb(34, 34, 34);
                txt_UsuarioVendedor.ForeColor = Color.White;
                txt_ApellidoVendedor.ForeColor = Color.White;
                txt_NombreVendedor.ForeColor = Color.White;
                txt_ContraseñaVendedor.ForeColor = Color.White;
                pic_AgregarVendedor.Image = Resources.agregar_blanco_84x24;
                chk_Admin.ForeColor = Color.White;

            }
            else
            {
                dgv_VuelosActivos.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dgv_VuelosActivos.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv_VuelosActivos.RowHeadersDefaultCellStyle.BackColor = Color.White;
                dgv_VuelosActivos.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
                pic_Modo.Image = Resources.night_mode1;
                this.BackColor = Color.White;
                Aerolinea.modoOscuro = true;
                lbl_Bienvenido.ForeColor = Color.Black;
                pic_Pasajeros.Image = Resources.pasajeros_negro_84x24;
                pic_Salir.Image = Resources.salir_negro_84x24;
                pic_Agregar.Image = Resources.agregar_negro;
                pic_Vender.Image = Resources.vender_negro_84x24;
                dgv_VuelosActivos.RowsDefaultCellStyle.BackColor = Color.White;
                dgv_VuelosActivos.RowsDefaultCellStyle.ForeColor = Color.Black;
                lbl_Fecha.ForeColor = Color.Black;
                pic_SalirHistorico.Image = Resources.salir_negro_84x24;
                pic_Cancelar.Image = Resources.cancelar_negro_84x24;
                rtx_InfoVuelo.ForeColor = Color.Black;
                rtx_InfoVuelo.BackColor = Color.White;
                pic_SalirHistorico.Image = Resources.salir_negro_84x24;
                cmb_FiltroHistorico.ForeColor = Color.Black;
                cmb_FiltroHistorico.BackColor = Color.White;
                lbl_RecaudacionCabotaje.ForeColor = Color.Black;
                lbl_RecaudacionInternacional.ForeColor = Color.Black;
                lbl_RecaudacionTotal.ForeColor = Color.Black;
                pic_SalirPanelInfo.Image = Resources.salir_negro_84x24;
                pic_AgregarCliente.Image = Resources.agregar_cliente_negro;
                pic_MasInfo.Image = Resources.info_negro;
                pic_Historico.Image = Resources.historico_84x24_negro;
                pic_Borrar.Image = Resources.borrar_negro;
                lbl_ErrorBorrar.ForeColor = Color.Black;
                lbl_InfoVendedor.ForeColor = Color.Black;
                pic_Modificar.Image = Resources.modificar_negro3;
                pic_ModificarAceptar.Image = Resources.modificar_negro3;
                txt_ApellidoVendedor.BackColor = Color.White;
                txt_ContraseñaVendedor.BackColor = Color.White;
                txt_NombreVendedor.BackColor = Color.White;
                txt_UsuarioVendedor.BackColor = Color.White;
                txt_UsuarioVendedor.ForeColor = Color.Black;
                txt_ApellidoVendedor.ForeColor = Color.Black;
                txt_NombreVendedor.ForeColor = Color.Black;
                txt_ContraseñaVendedor.ForeColor = Color.Black;
                pic_AgregarVendedor.Image = Resources.agregar_negro_84x24;
                chk_Admin.ForeColor= Color.Black;

            }
        }

    }
}
