using System;
using System.Collections.Generic;
using System.Data;
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

namespace Automata3
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TxtNumEst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void BtnStep1_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txtNumCase.Text) || String.IsNullOrWhiteSpace(txtNumEst.Text))
                MessageBox.Show("Alerta: Favor de llenar los dos campos", "Alerta");
            else
                fnGenerateAFNDA();
            
        }

        private void fnGenerateAFNDA()
        {
            try
            {
                gridAdd.Visibility = Visibility.Visible;
                txtNumEst.IsEnabled = false;
                txtNumCase.IsEnabled = false;
                btnStep1.Click -= new RoutedEventHandler(BtnStep1_Click);
                btnStep1.Click += new RoutedEventHandler(BtnStep2_Click);
                btnStep1.Content = "Generar AFD";
                DataTable DT = new DataTable("AFND");
                //Se imprimen los estados
                lblStados.Visibility = Visibility.Visible;
                lblStados.Content = "Estados: ";
                cmbInit.Items.Clear();
                cmbEnd.Items.Clear();
                DT.Columns.Add("Estados");
                for (int i = 0; i < Convert.ToInt32(txtNumEst.Text); i++)
                {
                    lblStados.Content += "[" + char.ConvertFromUtf32(65 + i) + "]  ";
                    ComboBoxItem Itemcmb = new ComboBoxItem();
                    Itemcmb.Content = char.ConvertFromUtf32(65 + i);
                    Itemcmb.ToolTip = i.ToString();
                    Itemcmb.Name = "Item" + char.ConvertFromUtf32(65 + i);
                    cmbInit.Items.Add(Itemcmb);
                    Itemcmb = new ComboBoxItem();
                    Itemcmb.Content = char.ConvertFromUtf32(65 + i);
                    Itemcmb.ToolTip = i.ToString();
                    Itemcmb.Name = "Item" + char.ConvertFromUtf32(65 + i);
                    cmbEnd.Items.Add(Itemcmb);
                    DT.Rows.Add();
                    DT.Rows[i][0] = char.ConvertFromUtf32(65 + i);
                }
                //Se imprimen los casos
                lblStados.Content += "\nCasos: ";
                cmbCase.Items.Clear();
                for (int i = 0; i < Convert.ToInt32(txtNumCase.Text); i++)
                {
                    lblStados.Content += "[" + i + "]  ";
                    ComboBoxItem Itemcmb = new ComboBoxItem();
                    Itemcmb.Content = i.ToString();
                    Itemcmb.ToolTip = i.ToString();
                    Itemcmb.Name = "Item" + i;
                    cmbCase.Items.Add(Itemcmb);
                    DT.Columns.Add("Caso: " + i);
                }
                lblAFNDA.Visibility = Visibility.Visible;
                GridAFNDA.Visibility = Visibility.Visible;
                GridAFNDA.DataContext = DT.DefaultView;
            }
            catch (Exception error)
            {
                MessageBox.Show("Errro: " + error.Message + "\nEn: " + error.StackTrace);
            }
        }

        private void BtnStep2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                

            }
            catch (Exception error)
            {
                MessageBox.Show("Errro: " + error.Message +"\nEn: "+error.StackTrace);
            }
            
        }

            private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            gridAdd.Visibility = Visibility.Hidden;
            lblStados.Visibility = Visibility.Hidden;
            lblAFNDA.Visibility = Visibility.Hidden;
            GridAFNDA.Visibility = Visibility.Hidden;
            lblAFDA.Visibility = Visibility.Hidden;
            GridAFDA.Visibility = Visibility.Hidden;
            
            cmbInit.Items.Clear();
            cmbEnd.Items.Clear();
            txtNumEst.IsEnabled = true;
            txtNumEst.Text = "";
            txtNumCase.IsEnabled = true;
            txtNumCase.Text = "";
            btnStep1.Content = "Siguiente";
            btnStep1.Click += new RoutedEventHandler(BtnStep1_Click);
        }

        private void BtnAddParam_Click(object sender, RoutedEventArgs e)
        {
            if (cmbInit.SelectedIndex >= 0  && cmbCase.SelectedIndex >= 0 && cmbEnd.SelectedIndex >=0)
            {
                DataRowView view = (DataRowView)GridAFNDA.SelectedItem;
            }
            else
            {
                MessageBox.Show("Alerta: Favor de selecionar los tres campos","Alerta");
            }            
        }

        private void CmbInit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridAFNDA.SelectedIndex = cmbInit.SelectedIndex;
        }

        private void GridAFNDA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbInit.SelectedIndex = GridAFNDA.SelectedIndex;
        }
    }    
}
