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
            txtNumEst.MaxLength = 1;
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
            try
            {            
                txtNumEst.IsEnabled = false;
                txtNumCase.IsEnabled = false;
                btnStep1.Click -= new RoutedEventHandler(BtnStep1_Click);
                btnStep1.Click += new RoutedEventHandler(BtnStep2_Click);
                //Se imprimen los estados
                lblStados.Visibility = Visibility.Visible;
                lblStados.Content = "Estados: ";
                ListBox comboBox1 = (ListBox)gridAdd.Children[1];
                List<Estado> items = new List<Estado>();
                for (int i = 0; i < Convert.ToInt32(txtNumEst.Text); i++)
                {
                    lblStados.Content += "[" + char.ConvertFromUtf32(65+i) + "]  ";
                    items.Add(new Estado(Convert.ToString(65 + i), i));
                }
                comboBox1.ItemsSource = items;
                //Se generan las opciones
                for (int cont = 0; cont < Convert.ToInt32(txtNumCase.Text); cont++)
                {                   
                    // TxtBox in grid
                    TextBox txt1 = new TextBox();
                    Label lbl = new Label();
                    PanelCase.Children.Add(lbl);
                    PanelCase.Children.Add(txt1);
                    ///Label
                    lbl.Name = "Case" + cont;
                    lbl.Content = "Valor " + cont + ":";
                    lbl.HorizontalAlignment = HorizontalAlignment.Left;
                    lbl.VerticalAlignment = VerticalAlignment.Top;
                    ///Textbox
                    txt1.Name = "txtCase"+cont;
                    txt1.Text = "";
                    txt1.Width = 50;                
                    txt1.Height = 30;
                    txt1.HorizontalAlignment = HorizontalAlignment.Left;
                    txt1.VerticalAlignment = VerticalAlignment.Top;
                }
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

                //Boqueamos los parametros
                for (int i = 1; i < PanelCase.Children.Count; i+=2)
                {
                    try
                    {
                        TextBox txt = (TextBox)PanelCase.Children[(i)];
                        txt.IsEnabled = false;
                    }
                    catch { }
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Errro: " + error.Message +"\nEn: "+error.StackTrace);
            }
            
        }

            private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            PanelCase.Children.Clear();
            txtNumEst.IsEnabled = true;
            txtNumEst.Text = "";
            txtNumCase.IsEnabled = true;
            txtNumCase.Text = "";
            btnStep1.Click += new RoutedEventHandler(BtnStep1_Click);
        }
    }    
}
