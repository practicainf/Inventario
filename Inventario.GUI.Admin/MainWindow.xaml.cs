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
        accion accionFunc;


        public MainWindow()
        {

            manageFuncionarios = new ManageFuncionarios(new RFuncionario());

            InitializeComponent();
            EditFuncBtns(false);
            CleanFuncValues();
            UpdateFuncGrid();
        }

        private void UpdateFuncGrid()
        {
            dtgFunc.ItemsSource = null;
            dtgFunc.ItemsSource = manageFuncionarios.List;

        }

        private void CleanFuncValues()
        {
            txtFuncId.Text = "";
            txtFuncName.Clear();
            txtFuncLname.Clear();
            txtFuncArea.Clear();

        }

        private void EditFuncBtns(bool value)
        {
            btnCancelFunc.IsEnabled = value;
            btnEditFunc.IsEnabled = !value;
            btnDelFunc.IsEnabled = !value;
            btnSaveFunc.IsEnabled = value;
            btnNewFunc.IsEnabled = !value;


        }

        private void btnNewFunc_Click(object sender, RoutedEventArgs e)
        {
            CleanFuncValues();
            EditFuncBtns(true);
            accionFunc = accion.New;
        }

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

        private void btnFuncionariosGuardar_Click(object sender, RoutedEventArgs e)
        {

        }


        private void btnCancelFunc_Click(object sender, RoutedEventArgs e)
        {
            CleanFuncValues();
            EditFuncBtns(false);
        }

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
    }
}
