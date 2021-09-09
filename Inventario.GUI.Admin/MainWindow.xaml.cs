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
        IManageUnidad manageUnidad;
        IManageDepartamentos manageDepartamentos;
        IManageEdificios manageEdificios;
        accion accionFunc;
        accion accionEquip;
        accion accionTicket;
        accion accionUnidad;
        accion accionDepartamento;
        accion accionEdificio;

        Ticket ticket;
        Unidad unidad;
        Departamento departamento;
        Edificio edificio;



        public MainWindow()
        {
            InitializeComponent();

            manageEquipos = new ManageEquipos(new REquipo());
            manageFuncionarios = new ManageFuncionarios(new RFuncionario());
            manageTickets = new ManageTickets(new RTicket());
            manageUnidad = new ManageUnidads(new RUnidad());
            manageDepartamentos = new ManageDepartamentos(new RDepartamento());
            manageEdificios = new ManageEdificios(new REdificio());

            EditFuncBtns(false);
            CleanFuncValues();
            UpdateFuncGrid();

            EditEquipBtns(false);
            CleanEquipValues();
            UpdateEquipGrid();

            CleanUnidadValues();
            UpdateUnidadGrid();

            CleanDepartamentoValues();
            UpdateDepartamentoGrid();
            
            CleanEdificioValues();
            UpdateEdificioGrid();




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
                    Estado = txbEquiposEstado.Text
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
                eq.Marca = txbEquiposMarca.Text;
                eq.Estado = txbEquiposEstado.Text;
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
    }
}
