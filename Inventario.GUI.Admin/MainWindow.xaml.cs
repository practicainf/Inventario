using Inventario.BIZ;
using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using Inventario.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Inventario.GUI.Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        enum accion
        {
            New,
            Edit
        }

        IManageFuncionarios manageFuncionarios;
        IManageIP manageIPs;
        IManageEquipos manageEquipos;
        IManageTickets manageTickets;
        IManageUnidad manageUnidad;
        IManageDepartamentos manageDepartamentos;
        IManageEdificios manageEdificios;
        IManagePantallas managePantallas;
        IManageOrdenador manageOrdenadors;
        IManageFacturas manageFacturas;
        IManageProveedores manageProveedor;

        accion accionFunc;
        accion accionIP;
        accion accionEquip;
        accion accionFactura;
        accion accionTicket;
        accion accionUnidad;
        accion accionDepartamento;
        accion accionEdificio;
        accion accionPantalla;
        accion accionOrdenador;
        accion accionProveedor;

        Equipo equipo;
        Ordenador ordenador;
        Ticket ticket;
        Unidad unidad;
        Departamento departamento;
        Edificio edificio;
        IP ip;
        Proveedor proveedor;
        Factura factura;



        public MainWindow()
        {
            InitializeComponent();

            manageEquipos = new ManageEquipos(new REquipo());
            managePantallas = new ManagePantallas(new RPantalla());
            manageOrdenadors = new ManageOrdenador(new ROrdenador());
            manageFuncionarios = new ManageFuncionarios(new RFuncionario());
            manageTickets = new ManageTickets(new RTicket());
            manageUnidad = new ManageUnidads(new RUnidad());
            manageDepartamentos = new ManageDepartamentos(new RDepartamento());
            manageEdificios = new ManageEdificios(new REdificio());
            manageIPs = new ManageIP(new RIP());
            manageFacturas = new ManageFactura(new RFactura());
            manageProveedor = new ManageProveedor(new RProveedor());
                

            EditFuncBtns(false);
            CleanFuncValues();
            UpdateFuncGrid();

            EditProveedorBtns(false);
            CleanProveedorValues();
            UpdateProveedorGrid();
            
            EditEquipBtns(false);
            CleanEquipValues();
            UpdateEquipGrid();
            
            EditOrdenadorBtns(false);
            CleanOrdenadorValues();
            UpdateOrdenadorGrid();

            EditPantallaBtns(false);
            CleanPantallaValues();
            UpdatePantallaGrid();

            EditIPBtns(false);
            CleanIPValues();
            UpdateIPGrid();

            CleanFacturaValues();
            UpdateFacturaGrid();

            CleanUnidadValues();
            UpdateUnidadGrid();

            CleanDepartamentoValues();
            UpdateDepartamentoGrid();
            
            CleanEdificioValues();
            UpdateEdificioGrid();

            txbPantallasCategoria.IsEnabled = false;
            txbOrdenadorCategoria.IsEnabled = false;


            UpdateTicketGrid();
            gridDetalle.IsEnabled = false;

        }

        


        //Boton Guardar Ticket
        private void btnGuardarTicket_Click(object sender, RoutedEventArgs e)
        {
            if (accionTicket == accion.New)
            {
                if(ticket.FechaEntrega == null)
                {
                    ticket.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
                    
                }
                else
                {
                    ticket.FechaEntrega = DateTime.Now;
                }
                
                ticket.FechaIngreso = DateTime.Now;
                ticket.Empleado = cmbFuncionario.SelectedItem as Funcionario;
                if (manageTickets.Create(ticket))
                {
                    MessageBox.Show("Ticket guardado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    LimpiarCamposDeTicket();
                    gridDetalle.IsEnabled = false;
                    UpdateTicketGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar el ticket", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {

                ticket.FechaIngreso = DateTime.Now;
                ticket.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
                ticket.Empleado = cmbFuncionario.SelectedItem as Funcionario;

                if (manageTickets.Update(ticket.Id, ticket))
                {
                    MessageBox.Show("Ticket guardado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    LimpiarCamposDeTicket();
                    gridDetalle.IsEnabled = false;
                    UpdateTicketGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar el ticket", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Entregar Ticket
        private void btnEntregarTicket_Click(object sender, RoutedEventArgs e)
        {
            lblFechaRetiro.Content = DateTime.Now;
            ticket.FechaRetiro = DateTime.Parse(lblFechaRetiro.Content.ToString());
        }

        //Boton Cancelar Ticket
        private void btnCanelarTicket_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCamposDeTicket();
            gridDetalle.IsEnabled = false;
        }

        //Doble Click Ticket
        private void dtgTickets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Ticket v = dtgTickets.SelectedItem as Ticket;
            if (v != null)
            {
                gridDetalle.IsEnabled = true;
                ticket = v;
                ActualizarEquiposEnTicket();
                accionTicket = accion.Edit;

                lblFechaRecepcion.Content = ticket.FechaIngreso.ToString();
                lblFechaRetiro.Content = ticket.FechaRetiro.ToString();
                ActualizarCombosDetalle();
                cmbFuncionario.Text = ticket.Empleado.ToString();
                dtpFechaEntrega.SelectedDate = ticket.FechaEntrega;
            }
        }

        //Limpiar Campos Tickets
        private void LimpiarCamposDeTicket()
        {
            dtpFechaEntrega.SelectedDate = DateTime.Now;
            lblFechaRetiro.Content = "";
            lblFechaRecepcion.Content = "";
            dtgEquiposEnTicket.ItemsSource = null;
            cmbEquipos.ItemsSource = null;
            cmbFuncionario.ItemsSource = null;
        }

        //Boton Agregar Equipo a Ticket
        private void btnAgregarEquipo_Click(object sender, RoutedEventArgs e)
        {
            Equipo eq = cmbEquipos.SelectedItem as Equipo;
            
            if (eq != null)
            {
                //eq.Nombre = eq.Nombre;
                ticket.EquipoSolicitado.Add(eq);
                ActualizarEquiposEnTicket();
            }
        }

        //Boton Eliminar Equipo de Ticket
        private void btnEliminarEquipo_Click(object sender, RoutedEventArgs e)
        {
            Equipo eq = dtgEquiposEnTicket.SelectedItem as Equipo;
            if (eq != null)
            {
                ticket.EquipoSolicitado.Remove(eq);
                ActualizarEquiposEnTicket();
            }
        }

        //Boton Nuevo Ticket
        private void btnNuevoTicket_Click(object sender, RoutedEventArgs e)
        {
            gridDetalle.IsEnabled = true;
            ActualizarCombosDetalle();
            ticket = new Ticket();
            ticket.EquipoSolicitado = new List<Equipo>();
            ActualizarEquiposEnTicket();
            accionTicket = accion.New;
        }

        //Actualizar ComboBoxDetalle
        private void ActualizarCombosDetalle()
        {


            cmbEquipos.ItemsSource = null;
            cmbEquipos.ItemsSource = manageEquipos.List;

            cmbFuncionario.ItemsSource = null;
            cmbFuncionario.ItemsSource = manageFuncionarios.List;

        }

        //Actualizar Equipos en Ticket
        private void ActualizarEquiposEnTicket()
        {
            dtgEquiposEnTicket.ItemsSource = null;
            dtgEquiposEnTicket.ItemsSource = ticket.EquipoSolicitado;

        }

        //Boton Eliminar Ticket
        private void btnEliminarTicket_Click(object sender, RoutedEventArgs e)
        {
            Ticket t = dtgTickets.SelectedItem as Ticket;
            if (t != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar el Ticket?", "Almacén", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageTickets.Delete(t))
                    {
                        MessageBox.Show("Eliminado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        UpdateTicketGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal...", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //Actualizar Tabla Tickets
        private void UpdateTicketGrid()
        {
            dtgTickets.ItemsSource = null;
            dtgTickets.ItemsSource = manageTickets.List;

        }









       
        //Actualizar Tabla Funcionarios
        private void UpdateFuncGrid()
        {
            dtgFunc.ItemsSource = null;
            dtgFunc.ItemsSource = manageFuncionarios.List;

        }

        //Limpiar Valores Funcionario
        private void CleanFuncValues()
        {
            txtFuncId.Text = "";
            txtFuncRut.Clear();
            txtFuncName.Clear();
            txtFuncLname.Clear();
            txtFuncArea.Clear();

        }

        //Botones Edicion Funcionario
        private void EditFuncBtns(bool value)
        {
            btnCancelFunc.IsEnabled = value;
            btnEditFunc.IsEnabled = !value;
            btnDelFunc.IsEnabled = !value;
            btnSaveFunc.IsEnabled = value;
            btnNewFunc.IsEnabled = !value;


        }

        //Boton Nuevo Funcionario
        private void btnNewFunc_Click(object sender, RoutedEventArgs e)
        {
            CleanFuncValues();
            EditFuncBtns(true);
            accionFunc = accion.New;
        }

        //Boton Editar Funcionario
        private void btnEditFunc_Click(object sender, RoutedEventArgs e)
        {
            Funcionario emp = dtgFunc.SelectedItem as Funcionario;
            if (emp != null)
            {
                txtFuncLname.Text = emp.Apellido;
                txtFuncArea.Text = emp.Area;
                txtFuncName.Text = emp.Nombre;
                txtFuncRut.Text = emp.Rut;
                accionFunc = accion.Edit;
                EditFuncBtns(true);
            }

        }

        //Boton Guardar Funcionario
        private void btnSaveFunc_Click(object sender, RoutedEventArgs e)
        {
            if (accionFunc == accion.New)
            {
                Funcionario emp = new Funcionario()
                {
                    Rut = txtFuncRut.Text,
                    Nombre = txtFuncName.Text,
                    Apellido = txtFuncLname.Text,
                    Area = txtFuncArea.Text
                };
                if (manageFuncionarios.Create(emp))
                {
                    MessageBox.Show("Funcionario agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanFuncValues();
                    UpdateFuncGrid();
                    EditFuncBtns(false);
                }
                else
                {
                    MessageBox.Show("El Funcionario no se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Funcionario emp = dtgFunc.SelectedItem as Funcionario;
                emp.Rut = txtFuncRut.Text;
                emp.Nombre = txtFuncName.Text;
                emp.Apellido = txtFuncLname.Text;
                emp.Area = txtFuncArea.Text;
                if (manageFuncionarios.Update(emp.Id, emp))
                {
                    MessageBox.Show("Funcionario modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanFuncValues();
                    UpdateFuncGrid();
                    EditFuncBtns(false);
                }
                else
                {
                    MessageBox.Show("El Funcionario no se pudo actualizar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Funcionario
        private void btnCancelFunc_Click(object sender, RoutedEventArgs e)
        {
            CleanFuncValues();
            EditFuncBtns(false);
        }

        //Boton Eliminar Funcionario
        private void btnDelFunc_Click(object sender, RoutedEventArgs e)
        {
            {
                Funcionario emp = dtgFunc.SelectedItem as Funcionario;
                if (emp != null)
                {
                    if (MessageBox.Show("¿Realmente desea eliminar este Funcionario?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (manageFuncionarios.Delete(emp))
                        {
                            MessageBox.Show("Funcionario eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                            UpdateFuncGrid();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el Funcionario", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

            }

        }










        //Actualizar Tabla IP
        private void UpdateIPGrid()
        {
            dtgIP.ItemsSource = null;
            dtgIP.ItemsSource = manageIPs.List;

        }

        //Limpiar Valores IP
        private void CleanIPValues()
        {
            txbIPDireccion.Text = "";

        }

        //Botones Edicion IP
        private void EditIPBtns(bool value)
        {
            btnIPCancelar.IsEnabled = value;
            btnIPEditar.IsEnabled = !value;
            btnIPEliminar.IsEnabled = !value;
            btnIPGuardar.IsEnabled = value;
            btnIPNuevo.IsEnabled = !value;


        }

        //Boton Nuevo IP
        private void btnIPNuevo_Click(object sender, RoutedEventArgs e)
        {
            CleanIPValues();
            EditIPBtns(true);
            accionIP = accion.New;
        }

        //Boton Editar IP
        private void btnIPEditar_Click(object sender, RoutedEventArgs e)
        {
            IP ip = dtgIP.SelectedItem as IP;
            if (ip != null)
            {
                txbIPDireccion.Text = ip.DireccionIP;
            }
            ActualizarCombosIP();

        }

        //Boton Guardar IP
        private void btnIPGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionIP == accion.New)
            {
                IP ip = new IP()
                {
                    DireccionIP = txbIPDireccion.Text,
                    Estado = "DISPONIBLE"
                };
                if (manageIPs.Create(ip))
                {
                    MessageBox.Show("IP agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanIPValues();
                    UpdateIPGrid();
                    EditIPBtns(false);
                    ActualizarCombosIP();
                }
                else
                {
                    MessageBox.Show("El IP no se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                IP ip = dtgIP.SelectedItem as IP;
                ip.DireccionIP = txbIPDireccion.Text;
                if (manageIPs.Update(ip.Id, ip))
                {
                    MessageBox.Show("IP modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanIPValues();
                    UpdateIPGrid();
                    EditIPBtns(false);
                    ActualizarCombosIP();
                }
                else
                {
                    MessageBox.Show("El IP no se pudo actualizar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar IP
        private void btnIPCancelar_Click(object sender, RoutedEventArgs e)
        {
            CleanIPValues();
            EditIPBtns(false);
        }

        //Boton Eliminar IP
        private void btnIPEliminar_Click(object sender, RoutedEventArgs e)
        {
            {
                IP ip = dtgIP.SelectedItem as IP;
                if (ip != null)
                {
                    if (MessageBox.Show("¿Realmente desea eliminar este IP?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (manageIPs.Delete(ip))
                        {
                            MessageBox.Show("IP eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                            UpdateIPGrid();
                            ActualizarCombosIP();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el IP", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

            }

        }

        








        //Boton Nuevo Equipo
        private void btnEquiposNuevo_Click(object sender, RoutedEventArgs e)
        {
            ActualizarCombosIP();
            CleanEquipValues();
            accionEquip = accion.New;
            EditEquipBtns(true);


        }

        //Boton Editar Equipo
        private void btnEquiposEditar_Click(object sender, RoutedEventArgs e)
        {
            CleanEquipValues();
            accionEquip = accion.Edit;
            EditEquipBtns(true);
            Equipo eq = dtgEquipos.SelectedItem as Equipo;
            if (eq != null)
            {
                
                txbEquiposNombre.Text = eq.Nombre;
                txbEquiposCategoria.Text = eq.Tipo;
                txbEquiposMarca.Text = eq.Marca;
                txbEquiposEstado.Text = eq.Estado;
            }

        }

        //Boton Guardar Equipo
        private void btnEquiposGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionEquip == accion.New)
            {
                Equipo eq = new Equipo()
                {
                    Tipo = txbEquiposCategoria.Text,
                    Nombre = txbEquiposNombre.Text,
                    Marca = txbEquiposMarca.Text,
                    Estado = txbEquiposEstado.Text,
                };
                if (manageEquipos.Create(eq))
                {
                    MessageBox.Show("Equipo correctamente agregado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanEquipValues();
                    UpdateEquipGrid();
                    UpdatePantallaGrid();
                    UpdateOrdenadorGrid();
                    EditEquipBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Equipo eq = dtgPantallas.SelectedItem as Equipo;
                eq.Tipo = txbEquiposCategoria.Text;
                eq.Nombre = txbEquiposNombre.Text;
                eq.Marca = txbEquiposMarca.Text;
                eq.Estado = txbEquiposEstado.Text;
                if (manageEquipos.Update(eq.Id, eq))
                {
                    MessageBox.Show("Equipo correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanEquipValues();
                    UpdateEquipGrid();

                    UpdatePantallaGrid();
                    UpdateOrdenadorGrid();
                    EditEquipBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Equipo
        private void btnEquiposCancelar_Click(object sender, RoutedEventArgs e)
        {
            CleanEquipValues();
            EditEquipBtns(false);

        }

        //Boton Eliminar Equipo
        private void btnEquiposEliminar_Click(object sender, RoutedEventArgs e)
        {
            Equipo eq = dtgEquipos.SelectedItem as Equipo;
            if (eq != null)
            {
                if (MessageBox.Show("¿Realmente deseas eliminar este Equipo?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageEquipos.Delete(eq))
                    {
                        MessageBox.Show("Equipo Eliminado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateEquipGrid();
                        UpdateOrdenadorGrid();
                        UpdatePantallaGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        //Actualizar Tabla de Equipos
        private void UpdateEquipGrid()
        {
            dtgEquipos.ItemsSource = null;
            //dtgPantallas.ItemsSource = manageEquipos.ListarPantallas("Pantallas");
            dtgEquipos.ItemsSource = manageEquipos.List;
        }

        //Limpiar Valores Equipo
        private void CleanEquipValues()
        {
            txbEquiposCategoria.Clear();
            
            txbEquiposNombre.Clear();
            txbEquiposMarca.Clear();
            txbEquiposEstado.Clear();

        }

        //Botones Edicion Equipos
        private void EditEquipBtns(bool value)
        {
            
            btnEquiposCancelar.IsEnabled = value;
            btnEquiposEditar.IsEnabled = !value;
            btnEquiposEliminar.IsEnabled = !value;
            btnEquiposGuardar.IsEnabled = value;
            btnEquiposNuevo.IsEnabled = !value;

        }

        //Actualizar ComboBoxIP
        private void ActualizarCombosIP()
        {

            cmbOrdenadorIP.ItemsSource = null;
            cmbOrdenadorIP.ItemsSource = manageIPs.ListarIpDispo();

        }









        //Boton Nuevo Ordenador
        private void btnOrdenadorNuevo_Click(object sender, RoutedEventArgs e)
        {
            CleanOrdenadorValues();
            accionEquip = accion.New;
            EditOrdenadorBtns(true);
            ActualizarCombosIP();


        }

        //Boton Editar Ordenador
        private void btnOrdenadorEditar_Click(object sender, RoutedEventArgs e)
        {
            CleanOrdenadorValues();
            accionEquip = accion.Edit;
            ActualizarCombosIP();
            EditOrdenadorBtns(true);
            Ordenador eq = dtgOrdenador.SelectedItem as Ordenador;
            if (eq != null)
            {

                txbOrdenadorNombre.Text = eq.Nombre;
                txbOrdenadorCategoria.Text = eq.Tipo;
                txbOrdenadorMarca.Text = eq.Marca;
                txbOrdenadorEstado.Text = eq.Estado;
                txbOrdenadorHost.Text = eq.Host;
                txbTipoOrdenador.Text = eq.Tipo;
                txbOrdenadorProcesador.Text = eq.Procesador;
                txbOrdenadorNucleos.Text = eq.Nucleos;
                txbOrdenadorRam.Text = eq.Ram;
                txbOrdenadorAlmacenamiento.Text = eq.Almacenamiento;
                txbOrdenadorMACLAN.Text = eq.MACLAN;
                txbOrdenadorMACWIFI.Text = eq.MACWIFI;
                if(eq.DireccionIP != null)
                {
                    cmbOrdenadorIP.Text = eq.DireccionIP.ToString();
                }
                
            }

        }

        //Boton Guardar Ordenador
        private void btnOrdenadorGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionEquip == accion.New)
            {
                Ordenador eq = new Ordenador()
                {                                         
                    Tipo = "Ordenador",
                    Nombre = txbOrdenadorNombre.Text,
                    Marca = txbOrdenadorMarca.Text,
                    Estado = txbOrdenadorEstado.Text,
                    Host = txbOrdenadorHost.Text,
                    DireccionIP = cmbOrdenadorIP.SelectedItem as IP,
                    TipoOrdenador = txbTipoOrdenador.Text,
                    Procesador = txbOrdenadorProcesador.Text,
                    Nucleos = txbOrdenadorNucleos.Text,
                    Ram = txbOrdenadorRam.Text,
                    Almacenamiento = txbOrdenadorAlmacenamiento.Text,
                    MACLAN = txbOrdenadorMACLAN.Text,
                    MACWIFI = txbOrdenadorMACWIFI.Text                  

                };

                
                
                if (manageEquipos.Create(eq))
                {
                    MessageBox.Show("Ordenador correctamente agregado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanOrdenadorValues();
                    UpdateOrdenadorGrid();
                    UpdateEquipGrid();
                    EditOrdenadorBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Ordenador eq = dtgOrdenador.SelectedItem as Ordenador;
                eq.Tipo = txbOrdenadorCategoria.Text;
                eq.Nombre = txbOrdenadorNombre.Text;
                eq.Marca = txbOrdenadorMarca.Text;
                eq.Estado = txbOrdenadorEstado.Text;
                eq.Host = txbOrdenadorHost.Text;
                eq.TipoOrdenador = txbTipoOrdenador.Text;
                eq.Procesador = txbOrdenadorProcesador.Text;
                eq.Nucleos = txbOrdenadorNucleos.Text;
                eq.Ram = txbOrdenadorRam.Text;
                eq.Almacenamiento = txbOrdenadorAlmacenamiento.Text;
                eq.MACLAN = txbOrdenadorMACLAN.Text;
                eq.MACWIFI = txbOrdenadorMACWIFI.Text;
                eq.DireccionIP = cmbOrdenadorIP.SelectedItem as IP;
                if (manageEquipos.Update(eq.Id, eq))
                {
                    MessageBox.Show("Ordenador correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanOrdenadorValues();
                    UpdateEquipGrid();
                    UpdateOrdenadorGrid();
                    EditOrdenadorBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Ordenador
        private void btnOrdenadorCancelar_Click(object sender, RoutedEventArgs e)
        {
            CleanOrdenadorValues();
            EditOrdenadorBtns(false);

        }

        //Boton Eliminar Ordenador
        private void btnOrdenadorEliminar_Click(object sender, RoutedEventArgs e)
        {
            Ordenador eq = dtgOrdenador.SelectedItem as Ordenador;
            if (eq != null)
            {
                if (MessageBox.Show("¿Realmente deseas eliminar este Ordenador?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageEquipos.Delete(eq))
                    {
                        MessageBox.Show("Ordenador Eliminado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateOrdenadorGrid();
                        UpdateEquipGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        //Actualizar Tabla de Ordenador
        private void UpdateOrdenadorGrid()
        {
            dtgOrdenador.ItemsSource = null;
            dtgOrdenador.ItemsSource = manageEquipos.ListarOrdenador("Ordenador");
        }

        //Limpiar Valores Ordenador
        private void CleanOrdenadorValues()
        {
            txbOrdenadorCategoria.Text = "Ordenador";

            txbOrdenadorNombre.Clear();
            txbOrdenadorMarca.Clear();
            txbOrdenadorEstado.Clear();
            txbOrdenadorHost.Clear();
            txbTipoOrdenador.Clear();
            txbOrdenadorProcesador.Clear();
            txbOrdenadorNucleos.Clear();
            txbOrdenadorRam.Clear();
            txbOrdenadorAlmacenamiento.Clear();
            txbOrdenadorMACLAN.Clear();
            txbOrdenadorMACWIFI.Clear();


        }

        //Botones Edicion Ordenador
        private void EditOrdenadorBtns(bool value)
        {

            btnOrdenadorCancelar.IsEnabled = value;
            btnOrdenadorEditar.IsEnabled = !value;
            btnOrdenadorEliminar.IsEnabled = !value;
            btnOrdenadorGuardar.IsEnabled = value;
            btnOrdenadorNuevo.IsEnabled = !value;

        }










        //Boton Nuevo Pantalla
        private void btnPantallasNuevo_Click(object sender, RoutedEventArgs e)
        {
            CleanPantallaValues();
            accionPantalla = accion.New;
            EditPantallaBtns(true);


        }

        //Boton Editar Pantalla
        private void btnPantallasEditar_Click(object sender, RoutedEventArgs e)
        {
            CleanPantallaValues();
            accionPantalla = accion.Edit;
            EditPantallaBtns(true);
            Pantalla eq = dtgPantallas.SelectedItem as Pantalla;
            if (eq != null)
            {

                txbPantallasNombre.Text = eq.Nombre;
                txbPantallasCategoria.Text = eq.Tipo;
                txbPantallasMarca.Text = eq.Marca;
                txbPantallasEstado.Text = eq.Estado;
                txbPantallasPulgadas.Text = eq.Pulgadas;
            }

        }

        //Boton Guardar Pantalla
        private void btnPantallasGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (accionPantalla == accion.New)
            {
                Pantalla eq = new Pantalla()
                {
                    Tipo = "Pantalla",
                    Nombre = txbPantallasNombre.Text,
                    Marca = txbPantallasMarca.Text,
                    Estado = txbPantallasEstado.Text,
                    Pulgadas = txbPantallasPulgadas.Text,
                    SN = txbPantallasSN.Text
                };
                if (manageEquipos.Create(eq))
                {
                    MessageBox.Show("Pantalla correctamente agregado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanPantallaValues();
                    UpdatePantallaGrid();
                    UpdateEquipGrid();
                    EditPantallaBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Pantalla eq = dtgPantallas.SelectedItem as Pantalla;
                eq.Tipo = txbPantallasCategoria.Text;
                eq.Nombre = txbPantallasNombre.Text;
                eq.Marca = txbPantallasMarca.Text;
                eq.Estado = txbPantallasEstado.Text;
                eq.Pulgadas = txbPantallasPulgadas.Text;
                eq.SN = txbPantallasSN.Text;
                if (manageEquipos.Update(eq.Id, eq))
                {
                    MessageBox.Show("Pantalla correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanPantallaValues();
                    UpdatePantallaGrid();
                    UpdateEquipGrid();
                    EditPantallaBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Pantalla
        private void btnPantallasCancelar_Click(object sender, RoutedEventArgs e)
        {
            CleanPantallaValues();
            EditPantallaBtns(false);

        }

        //Boton Eliminar Pantalla
        private void btnPantallasEliminar_Click(object sender, RoutedEventArgs e)
        {
            Pantalla eq = dtgPantallas.SelectedItem as Pantalla;
            if (eq != null)
            {
                if (MessageBox.Show("¿Realmente deseas eliminar este Pantalla?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageEquipos.Delete(eq))
                    {
                        MessageBox.Show("Pantalla Eliminado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdatePantallaGrid();
                        UpdateEquipGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

        //Actualizar Tabla de Pantallas
        private void UpdatePantallaGrid()
        {
            dtgPantallas.ItemsSource = null;
            dtgPantallas.ItemsSource = manageEquipos.ListarPantallas("Pantallas");
            //dtgPantallas.ItemsSource = manageEquipos.List;
        }

        //Limpiar Valores Pantalla
        private void CleanPantallaValues()
        {
            txbPantallasCategoria.Text = "Pantalla";

            txbPantallasNombre.Clear();
            txbPantallasMarca.Clear();
            txbPantallasEstado.Clear();
            txbPantallasPulgadas.Clear();
            txbPantallasSN.Clear();

        }

        //Botones Edicion Pantallas
        private void EditPantallaBtns(bool value)
        {

            btnPantallasCancelar.IsEnabled = value;
            btnPantallasEditar.IsEnabled = !value;
            btnPantallasEliminar.IsEnabled = !value;
            btnPantallasGuardar.IsEnabled = value;
            btnPantallasNuevo.IsEnabled = !value;

        }











        //Boton Guardar Unidad
        private void btnGuardarUnidad_Click(object sender, RoutedEventArgs e)
        {
            if (accionUnidad == accion.New)
            {


                unidad.NombreUnidad = txtUnidadName.Text;
                if (manageUnidad.Create(unidad))
                {
                    MessageBox.Show("Unidad guardada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanUnidadValues();
                    
                    gridDetalleUnidad.IsEnabled = false;
                    UpdateUnidadGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Unidad", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                unidad.NombreUnidad = txbEquiposNombre.Text;
                

                if (manageUnidad.Update(unidad.Id, unidad))
                {
                    MessageBox.Show("Unidad guardada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanUnidadValues();
                    gridDetalle.IsEnabled = false;
                    UpdateUnidadGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Unidad", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Unidad
        private void btnCanelarUnidad_Click(object sender, RoutedEventArgs e)
        {
            CleanUnidadValues();
            gridDetalleUnidad.IsEnabled = false;
        }

        //Doble Click Unidad
        private void dtgUnidad_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Unidad u = dtgUnidad.SelectedItem as Unidad;
            if (u != null)
            {
                gridDetalleUnidad.IsEnabled = true;
                unidad = u;
                ActualizarFuncionariosEnUnidad();
                accionUnidad = accion.Edit;

                txtUnidadName.Text = u.NombreUnidad;

                ActualizarCombosUnidad();
            }

            
        }

        //Boton Agregar Funcionarios a Unidad
        private void btnAgregarFuncionario_Click(object sender, RoutedEventArgs e)
        {
            
            Funcionario f = cmbFuncionarios.SelectedItem as Funcionario;

            if (f != null)
            {
                unidad.FuncionariosEnUnidad.Add(f);
                ActualizarFuncionariosEnUnidad();
            }
            
        }

        //Boton Eliminar Equipo de Ticket
        private void btnEliminarFuncionario_Click(object sender, RoutedEventArgs e)
        {
            Funcionario f = dtgFuncionariosEnUnidad.SelectedItem as Funcionario;
            if (f != null)
            {
                unidad.FuncionariosEnUnidad.Remove(f);
                ActualizarFuncionariosEnUnidad();
            }
        }

        //Boton Nuevo Unidad
        private void btnNuevoUnidad_Click(object sender, RoutedEventArgs e)
        {
            CleanUnidadValues();
            gridDetalleUnidad.IsEnabled = true;
            ActualizarCombosUnidad();
            unidad = new Unidad();
            unidad.FuncionariosEnUnidad = new List<Funcionario>();
            ActualizarFuncionariosEnUnidad();
            accionUnidad = accion.New;
        }

        //Actualizar ComboBoxUnidad
        private void ActualizarCombosUnidad()
        {

            cmbFuncionarios.ItemsSource = null;
            cmbFuncionarios.ItemsSource = manageFuncionarios.List;

        }

        //Actualizar Funcionarios en Unidad
        private void ActualizarFuncionariosEnUnidad()
        {

            dtgFuncionariosEnUnidad.ItemsSource = null;
            dtgFuncionariosEnUnidad.ItemsSource = unidad.FuncionariosEnUnidad;

        }

        //Boton Eliminar Unidad
        private void btnEliminarUnidad_Click(object sender, RoutedEventArgs e)
        {
            Unidad u = dtgUnidad.SelectedItem as Unidad;
            if (u != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar esta Unidad?", "Almacén", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageUnidad.Delete(u))
                    {
                        MessageBox.Show("Eliminada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        UpdateUnidadGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal...", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        
        //Actualizar Tabla Unidad
        private void UpdateUnidadGrid()
        {
            dtgUnidad.ItemsSource = null;
            dtgUnidad.ItemsSource = manageUnidad.List;
        }

        //Limpiar Valores Unidad
        private void CleanUnidadValues()
        {
            txtUnidadName.Clear();
            dtgFuncionariosEnUnidad.ItemsSource = null;
            cmbFuncionarios.ItemsSource = null;
        }

        







        //Boton Guardar Departamento
        private void btnGuardarDepartamento_Click(object sender, RoutedEventArgs e)
        {
            if (accionDepartamento == accion.New)
            {


                departamento.NombreDepartamento = txbDepartamentoNombre.Text;
                if (manageDepartamentos.Create(departamento))
                {
                    MessageBox.Show("Departamento guardada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanDepartamentoValues();

                    gridDetalleDepartamento.IsEnabled = false;
                    UpdateDepartamentoGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Departamento", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                departamento.NombreDepartamento = txbDepartamentoNombre.Text;


                if (manageDepartamentos.Update(departamento.Id, departamento))
                {
                    MessageBox.Show("Departamento guardada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanDepartamentoValues();
                    gridDetalleDepartamento.IsEnabled = false;
                    UpdateDepartamentoGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Departamento", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Departamento
        private void btnCanelarDepartamento_Click(object sender, RoutedEventArgs e)
        {
            CleanDepartamentoValues();
            gridDetalleDepartamento.IsEnabled = false;
        }

        //Doble Click Departamento
        private void dtgDepartamento_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Departamento d = dtgDepartamento.SelectedItem as Departamento;
            if (d != null)
            {
                gridDetalleDepartamento.IsEnabled = true;
                departamento = d;
                ActualizarUnidadesEnDepartamento();
                accionDepartamento = accion.Edit;

                txbDepartamentoNombre.Text = d.NombreDepartamento;

                ActualizarCombosDepartamento();
            }


        }

        //Boton Agregar Unidades a Departamento
        private void btnAgregarUnidad_Click(object sender, RoutedEventArgs e)
        {

            Unidad u = cmbUnidades.SelectedItem as Unidad;

            if (u != null)
            {
                departamento.UnidadesEnDepartamento.Add(u);
                ActualizarUnidadesEnDepartamento();
            }

        }

        //Boton Eliminar Equipo de Departamento
        private void btnEliminarUnidadDepartamento_Click(object sender, RoutedEventArgs e)
        {
            Unidad u = dtgUnidadesEnDepartamento.SelectedItem as Unidad;
            if (u != null)
            {
                departamento.UnidadesEnDepartamento.Remove(u);
                ActualizarUnidadesEnDepartamento();
            }
        }

        //Boton Nuevo Departamento
        private void btnNuevoDepartamento_Click(object sender, RoutedEventArgs e)
        {
            CleanDepartamentoValues();
            gridDetalleDepartamento.IsEnabled = true;
            ActualizarCombosDepartamento();
            departamento = new Departamento();
            departamento.UnidadesEnDepartamento = new List<Unidad>();
            ActualizarUnidadesEnDepartamento();
            accionDepartamento = accion.New;

        }

        //Actualizar ComboBoxDepartamento
        private void ActualizarCombosDepartamento()
        {

            cmbUnidades.ItemsSource = null;
            cmbUnidades.ItemsSource = manageUnidad.List;

        }

        //Actualizar Unidades en Departamento
        private void ActualizarUnidadesEnDepartamento()
        {

            dtgUnidadesEnDepartamento.ItemsSource = null;
            dtgUnidadesEnDepartamento.ItemsSource = departamento.UnidadesEnDepartamento;

        }

        //Boton Eliminar Departamento
        private void btnEliminarDepartamento_Click(object sender, RoutedEventArgs e)
        {
            Departamento d = dtgDepartamento.SelectedItem as Departamento;
            if (d != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar esta Departamento?", "Almacén", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageDepartamentos.Delete(d))
                    {
                        MessageBox.Show("Eliminada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        UpdateDepartamentoGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal...", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //Actualizar Tabla Departamento
        private void UpdateDepartamentoGrid()
        {
            dtgDepartamento.ItemsSource = null;
            dtgDepartamento.ItemsSource = manageDepartamentos.List;
        }

        //Limpiar Valores Departamento
        private void CleanDepartamentoValues()
        {
            txbDepartamentoNombre.Clear();
            dtgUnidadesEnDepartamento.ItemsSource = null;
            cmbUnidades.ItemsSource = null;
        }
        private void cmbFuncionario_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        private void cmbDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }








        private void btnNuevoEdificio_Click(object sender, RoutedEventArgs e)
        {
            CleanEdificioValues();
            gridDetalleEdificio.IsEnabled = true;
            ActualizarCombosEdificio();
            edificio = new Edificio();
            edificio.DepartamentosEnEdificio = new List<Departamento>();
            
            ActualizarDepartamentosEnEdificio();
            accionEdificio = accion.New;
        }

        private void ActualizarDepartamentosEnEdificio()
        {
            dtgDepartamentoEnEdificio.ItemsSource = null;
            dtgDepartamentoEnEdificio.ItemsSource = edificio.DepartamentosEnEdificio;
        }

        private void ActualizarCombosEdificio()
        {
            cmbDepartamento.ItemsSource = null;
            cmbDepartamento.ItemsSource = manageDepartamentos.List;
        }

        private void btnEliminarEdificio_Click(object sender, RoutedEventArgs e)
        {
            Edificio ed = dtgEdificio.SelectedItem as Edificio;
            if (ed != null)
            {
                if (manageEdificios.Delete(ed))
                {
                    MessageBox.Show("Eliminado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    UpdateDepartamentoGrid();
                }
                else
                {
                    MessageBox.Show("Algo salio mal...", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

           
        }

        private void dtgEdificio_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edificio ed = dtgEdificio.SelectedItem as Edificio;
            if (ed != null)
            {
                gridDetalleEdificio.IsEnabled = true;
                edificio = ed;
                ActualizarDepartamentosEnEdificio();
                accionDepartamento = accion.Edit;

                txbEdificioNombre.Text = ed.NombreEdificio;
                txbEdificioDireccion.Text = ed.DireccionEdificio;

                ActualizarCombosDepartamento();
            }
        }

        private void btnEliminarDepartamentoEdificio_Click(object sender, RoutedEventArgs e)
        {
            Departamento de = dtgDepartamentoEnEdificio.SelectedItem as Departamento;
            if (de != null)
            {
                edificio.DepartamentosEnEdificio.Remove(de);
                ActualizarDepartamentosEnEdificio();
            }
        }

        private void btnGuardarEdificio_Click(object sender, RoutedEventArgs e)
        {
            if (accionEdificio == accion.New)
            {


                edificio.NombreEdificio = txbEdificioNombre.Text;
                edificio.DireccionEdificio = txbEdificioDireccion.Text;
                if (manageEdificios.Create(edificio))
                {
                    MessageBox.Show("Edificio guardado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanEdificioValues();

                    gridDetalleEdificio.IsEnabled = false;
                    UpdateEdificioGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Edificio", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                departamento.NombreDepartamento = txbDepartamentoNombre.Text;


                if (manageEdificios.Update(edificio.Id, edificio))
                {
                    MessageBox.Show("Edificio guardado con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanEdificioValues();
                    gridDetalleDepartamento.IsEnabled = false;
                    UpdateEdificioGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Edificio", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCanelarEdificio_Click(object sender, RoutedEventArgs e)
        {
            CleanEdificioValues();
            gridDetalleEdificio.IsEnabled = false;
        }

        private void btnAgregarDepartamentoEdificio_Click(object sender, RoutedEventArgs e)
        {
            Departamento de = cmbDepartamento.SelectedItem as Departamento;

            if (de != null)
            {
                edificio.DepartamentosEnEdificio.Add(de);
                ActualizarDepartamentosEnEdificio();
            }
        }

        private void UpdateEdificioGrid()
        {

            dtgEdificio.ItemsSource = null;
            dtgEdificio.ItemsSource = manageEdificios.List;
        }

        private void CleanEdificioValues()
        {
            txbEdificioNombre.Clear();
            txbEdificioDireccion.Clear();
            dtgDepartamentoEnEdificio.ItemsSource = null;
            cmbDepartamento.ItemsSource = null;
        }

        private void dtgOrdenador_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }











        //Boton Guardar Factura
        private void btnGuardarFactura_Click(object sender, RoutedEventArgs e)
        {
            if (accionFactura == accion.New)
            {


                factura.FacturaNumero = txbFacturaNumero.Text;
                factura.FacturaTipoAdquisicion = txbFacturaAdquisicion.Text;
                factura.FacturaProveedor = cmbFacturaProveedor.SelectedItem as Proveedor;
                if (manageFacturas.Create(factura))
                {
                    MessageBox.Show("Factura guardada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanFacturaValues();
                    //cmbFacturaProveedor.SelectedItem = proveedor.FacturasProveedores.Add(factura);

                    dtgDetalleFactura.IsEnabled = false;
                    UpdateFacturaGrid();

                    
                }
                else
                {
                    MessageBox.Show("Error al guardar Factura", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                factura.FacturaNumero = txbFacturaNumero.Text;
                factura.FacturaTipoAdquisicion = txbFacturaAdquisicion.Text;
                factura.FacturaProveedor = cmbFacturaProveedor.SelectedItem as Proveedor;

                if (manageFacturas.Update(factura.Id, factura))
                {
                    MessageBox.Show("Factura guardada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    CleanFacturaValues();
                    gridDetalle.IsEnabled = false;
                    UpdateFacturaGrid();
                }
                else
                {
                    MessageBox.Show("Error al guardar Factura", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Factura
        private void btnCanelarFactura_Click(object sender, RoutedEventArgs e)
        {
            CleanFacturaValues();
            gridDetalleFactura.IsEnabled = false;
        }

        //Doble Click Factura
        private void dtgFactura_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Factura u = dtgFactura.SelectedItem as Factura;
            if (u != null)
            {
                gridDetalleFactura.IsEnabled = true;
                factura = u;
                ActualizarEquiposEnFactura();
                accionFactura = accion.Edit;

                txbFacturaNumero.Text = u.FacturaNumero;


                ActualizarCombosFactura();
            }


        }

        //Boton Agregar Equipos a Factura
        private void btnAgregarEquipoFactura_Click(object sender, RoutedEventArgs e)
        {

            Equipo f = cmbFacturaEquipos.SelectedItem as Equipo;

            if (f != null)
            {
                factura.EquiposEnFactura.Add(f);
                ActualizarEquiposEnFactura();
            }

        }

        //Boton Eliminar Equipo de Ticket
        private void btnEliminarEquipoFactura_Click(object sender, RoutedEventArgs e)
        {
            Equipo f = dtgDetalleFactura.SelectedItem as Equipo;
            if (f != null)
            {
                factura.EquiposEnFactura.Remove(f);
                ActualizarEquiposEnFactura();
            }
        }

        //Boton Nuevo Factura
        private void btnNuevoFactura_Click(object sender, RoutedEventArgs e)
        {
            CleanFacturaValues();
            gridDetalleFactura.IsEnabled = true;
            ActualizarCombosFactura();
            factura = new Factura();
            factura.EquiposEnFactura = new List<Equipo>();
            ActualizarEquiposEnFactura();
            accionFactura = accion.New;
        }

        //Actualizar ComboBoxFactura
        private void ActualizarCombosFactura()
        {

            cmbFacturaProveedor.ItemsSource = null;
            cmbFacturaProveedor.ItemsSource = manageProveedor.List;
            
            cmbFacturaEquipos.ItemsSource = null;
            cmbFacturaEquipos.ItemsSource = manageEquipos.List;

        }
        private void ActualizarCombosFacturaProveedores()
        {

            cmbFacturaProveedor.ItemsSource = null;
            cmbFacturaProveedor.ItemsSource = manageProveedor.List;

        }

        //Actualizar Equipos en Factura
        private void ActualizarEquiposEnFactura()
        {

            dtgDetalleFactura.ItemsSource = null;
            dtgDetalleFactura.ItemsSource = factura.EquiposEnFactura;



        }

        //Boton Eliminar Factura
        private void btnEliminarFactura_Click(object sender, RoutedEventArgs e)
        {
            Factura u = dtgFactura.SelectedItem as Factura;
            if (u != null)
            {
                if (MessageBox.Show("Realmente deseas eliminar esta Factura?", "Almacén", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (manageFacturas.Delete(u))
                    {
                        MessageBox.Show("Eliminada con éxito", "Almacén", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        UpdateFacturaGrid();
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal...", "Almacén", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        //Actualizar Tabla Factura
        private void UpdateFacturaGrid()
        {
            dtgFactura.ItemsSource = null;
            dtgFactura.ItemsSource = manageFacturas.List;
        }

        //Limpiar Valores Factura
        private void CleanFacturaValues()
        {
            txbFacturaNumero.Clear();
            txbFacturaAdquisicion.Clear();
            dtgDetalleFactura.ItemsSource = null;
            cmbFacturaProveedor.ItemsSource = null;
            cmbFacturaEquipos.ItemsSource = null;
        }

        private void cmbFacturaEquipos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }









        //Actualizar Tabla Proveedor
        private void UpdateProveedorGrid()
        {
            dtgProveedor.ItemsSource = null;
            dtgProveedor.ItemsSource = manageProveedor.List;

        }

        //Limpiar Valores Proveedor
        private void CleanProveedorValues()
        {
            txbProveedorRut.Clear();
            txbProveedorName.Clear();
            txbProveedorFono.Clear();

        }

        //Botones Edicion Proveedor
        private void EditProveedorBtns(bool value)
        {
            btnCancelProveedor.IsEnabled = value;
            btnEditProveedor.IsEnabled = !value;
            btnDelProveedor.IsEnabled = !value;
            btnSaveProveedor.IsEnabled = value;
            btnNewProveedor.IsEnabled = !value;


        }

        //Boton Nuevo Proveedor
        private void btnNewProveedor_Click(object sender, RoutedEventArgs e)
        {
            CleanProveedorValues();
            EditProveedorBtns(true);
            accionProveedor = accion.New;
        }

        //Boton Editar Proveedor
        private void btnEditProveedor_Click(object sender, RoutedEventArgs e)
        {
            Proveedor prov = dtgProveedor.SelectedItem as Proveedor;
            if (prov != null)
            {
                txbProveedorFono.Text = prov.FonoProveedor;
                txbProveedorName.Text = prov.NombreProveedor;
                txbProveedorRut.Text = prov.Rut;
                accionProveedor = accion.Edit;
                EditProveedorBtns(true);
            }

        }

        //Boton Guardar Proveedor
        private void btnSaveProveedor_Click(object sender, RoutedEventArgs e)
        {
            if (accionProveedor == accion.New)
            {
                Proveedor prov = new Proveedor()
                {
                    Rut = txbProveedorRut.Text,
                    NombreProveedor = txbProveedorName.Text,
                    FonoProveedor = txbProveedorFono.Text
                };
                if (manageProveedor.Create(prov))
                {
                    MessageBox.Show("Proveedor agregado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanProveedorValues();
                    UpdateProveedorGrid();
                    EditProveedorBtns(false);
                }
                else
                {
                    MessageBox.Show("El Proveedor no se pudo agregar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Proveedor prov = dtgProveedor.SelectedItem as Proveedor;
                prov.Rut = txbProveedorRut.Text;
                prov.NombreProveedor = txbProveedorName.Text;
                prov.FonoProveedor = txbProveedorFono.Text;
                if (manageProveedor.Update(prov.Id, prov))
                {
                    MessageBox.Show("Proveedor modificado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanProveedorValues();
                    UpdateProveedorGrid();
                    EditProveedorBtns(false);
                }
                else
                {
                    MessageBox.Show("El Proveedor no se pudo actualizar", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //Boton Cancelar Proveedor
        private void btnCancelProveedor_Click(object sender, RoutedEventArgs e)
        {
            CleanProveedorValues();
            EditProveedorBtns(false);
        }

        //Boton Eliminar Proveedor
        private void btnDelProveedor_Click(object sender, RoutedEventArgs e)
        {
            {
                Proveedor prov = dtgProveedor.SelectedItem as Proveedor;
                if (prov != null)
                {
                    if (MessageBox.Show("¿Realmente desea eliminar este Proveedor?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        if (manageProveedor.Delete(prov))
                        {
                            MessageBox.Show("Proveedor eliminado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                            UpdateProveedorGrid();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el Proveedor", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

            }

        }



        //Boton Nuevo Pantalla
        /// <summary>
        /// /

        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnPantallasNuevo_Click(object sender, RoutedEventArgs e)
        //{
        //    CleanEquipValues();
        //    accionEquip = accion.New;
        //    EditEquipBtns(true);


        //}

        ////Boton Editar Pantalla
        //private void btnEquiposEditar_Click(object sender, RoutedEventArgs e)
        //{
        //    CleanEquipValues();
        //    accionEquip = accion.Edit;
        //    EditEquipBtns(true);
        //    Equipo eq = dtgEquipos.SelectedItem as Equipo;
        //    if (eq != null)
        //    {
        //        txbEquiposId.Text = eq.Id;
        //        txbEquiposNombre.Text = eq.Nombre;
        //        txbEquiposCategoria.Text = eq.Tipo;
        //        txbEquiposMarca.Text = eq.Marca;
        //        txbEquiposEstado.Text = eq.Estado;
        //    }

        //}

        ////Boton Guardar Pantalla
        //private void btnEquiposGuardar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (accionEquip == accion.New)
        //    {
        //        Equipo eq = new Equipo()
        //        {
        //            Tipo = txbEquiposCategoria.Text,
        //            Nombre = txbEquiposNombre.Text,
        //            Marca = txbEquiposMarca.Text,
        //            Estado = txbEquiposEstado.Text
        //        };
        //        if (manageEquipos.Create(eq))
        //        {
        //            MessageBox.Show("Equipo correctamente agregado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
        //            CleanEquipValues();
        //            UpdateEquipGrid();
        //            EditEquipBtns(false);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //    {
        //        Equipo eq = dtgEquipos.SelectedItem as Equipo;
        //        eq.Tipo = txbEquiposCategoria.Text;
        //        eq.Nombre = txbEquiposNombre.Text;
        //        eq.Marca = txbEquiposMarca.Text;
        //        eq.Estado = txbEquiposEstado.Text;
        //        if (manageEquipos.Update(eq.Id, eq))
        //        {
        //            MessageBox.Show("Equipo correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
        //            CleanEquipValues();
        //            UpdateEquipGrid();
        //            EditEquipBtns(false);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}

        ////Boton Cancelar Pantalla
        //private void btnEquiposCancelar_Click(object sender, RoutedEventArgs e)
        //{
        //    CleanEquipValues();
        //    EditEquipBtns(false);

        //}

        ////Boton Eliminar Pantalla
        //private void btnEquiposEliminar_Click(object sender, RoutedEventArgs e)
        //{
        //    Equipo eq = dtgEquipos.SelectedItem as Equipo;
        //    if (eq != null)
        //    {
        //        if (MessageBox.Show("¿Realmente deseas eliminar este Equipo?", "Inventarios", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //        {
        //            if (manageEquipos.Delete(eq))
        //            {
        //                MessageBox.Show("Equipo Eliminado correctamente", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
        //                UpdateEquipGrid();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }

        //}

        ////Actualizar Tabla de Pantalla
        //private void UpdateEquipGrid()
        //{
        //    dtgEquipos.ItemsSource = null;
        //    dtgEquipos.ItemsSource = manageEquipos.List;
        //}

        ////Limpiar Valores Pantalla
        //private void CleanEquipValues()
        //{
        //    txbEquiposCategoria.Clear();
        //    txbEquiposId.Text = "";
        //    txbEquiposNombre.Clear();
        //    txbEquiposMarca.Clear();
        //    txbEquiposEstado.Clear();

        //}

        ////Botones Edicion Pantalla
        //private void EditEquipBtns(bool value)
        //{
        //    btnEquiposCancelar.IsEnabled = value;
        //    btnEquiposEditar.IsEnabled = !value;
        //    btnEquiposEliminar.IsEnabled = !value;
        //    btnEquiposGuardar.IsEnabled = value;
        //    btnEquiposNuevo.IsEnabled = !value;

        //}
    }
}
