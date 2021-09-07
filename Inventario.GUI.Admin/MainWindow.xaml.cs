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
        IManageEquipos manageEquipos;
        IManageTickets manageTickets;
        accion accionFunc;
        accion accionEquip;
        accion accionTicket;
        Ticket ticket;



        public MainWindow()
        {
            InitializeComponent();

            manageEquipos = new ManageEquipos(new REquipo());
            manageFuncionarios = new ManageFuncionarios(new RFuncionario());
            manageTickets = new ManageTickets(new RTicket());

            EditFuncBtns(false);
            CleanFuncValues();
            UpdateFuncGrid();

            EditEquipBtns(false);
            CleanEquipValues();
            UpdateEquipGrid();



            UpdateTicketGrid();
            gridDetalle.IsEnabled = false;

        }
        //Boton Guardar Ticket
        private void btnGuardarTicket_Click(object sender, RoutedEventArgs e)
        {
            if (accionTicket == accion.New)
            {

                ticket.FechaEntrega = dtpFechaEntrega.SelectedDate.Value;
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

        //Boton Agregar Equipo
        private void btnAgregarEquipo_Click(object sender, RoutedEventArgs e)
        {
            Equipo eq = cmbEquipos.SelectedItem as Equipo;
            if (eq != null)
            {
                ticket.EquipoSolicitado.Add(eq);
                ActualizarEquiposEnTicket();
            }
        }

        //Boton Eliminar Equipo
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

        //Actualizar Tabla de Equipos
        private void UpdateEquipGrid()
        {
            dtgEquipos.ItemsSource = null;
            dtgEquipos.ItemsSource = manageEquipos.List;
        }

        //Limpiar Valores Equipo
        private void CleanEquipValues()
        {
            txbEquiposCategoria.Clear();
            txbEquiposId.Text = "";
            txbEquiposNombre.Clear();

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
                txtFuncId.Text = emp.Id;
                txtFuncLname.Text = emp.Apellido;
                txtFuncArea.Text = emp.Area;
                txtFuncName.Text = emp.Nombre;
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

        //Boton Nuevo Equipo
        private void btnEquiposNuevo_Click(object sender, RoutedEventArgs e)
        {
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
                txbEquiposId.Text = eq.Id;
                txbEquiposNombre.Text = eq.Nombre;
                txbEquiposCategoria.Text = eq.Tipo;
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
                    Nombre = txbEquiposNombre.Text
                };
                if (manageEquipos.Create(eq))
                {
                    MessageBox.Show("Equipo correctamente agregado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanEquipValues();
                    UpdateEquipGrid();
                    EditEquipBtns(false);
                }
                else
                {
                    MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                Equipo eq = dtgEquipos.SelectedItem as Equipo;
                eq.Tipo = txbEquiposCategoria.Text;
                eq.Nombre = txbEquiposNombre.Text;
                if (manageEquipos.Update(eq.Id, eq))
                {
                    MessageBox.Show("Equipo correctamente modificado", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanEquipValues();
                    UpdateEquipGrid();
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
                    }
                    else
                    {
                        MessageBox.Show("Algo salio mal", "Inventarios", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
    }
}
