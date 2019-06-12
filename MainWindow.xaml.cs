using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Automata3.Entidades;

namespace Automata3
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int NumRow;
        private DataTable DT;
        private List<Transicion> ListAFDA;
        public MainWindow()
        {
            InitializeComponent();
            Label CopyRef = new Label();
            CopyRef.Content = "Creado por Jorge Perez, Mexico 2019, Github: https://github.com/Furiduri";
            FooterGrid.Children.Add(CopyRef);
            NumRow = 0;
            DT = null;
            ListAFDA = null;
            txtNumEst.Focus();
        }

        private void TxtNumEst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }
        private void TxtNumCase_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            switch (txt.Name)
            {
                case "txtNumEst":
                    txtNumCase.Focus();
                    break;
                case "txtNumCase":
                    btnStep1.Focus();
                    break;
                default:
                    btnClear.Focus();
                    break;
            }
        }
        private void BtnStep1_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txtNumCase.Text) || String.IsNullOrWhiteSpace(txtNumEst.Text))
                MessageBox.Show("Alerta: Favor de llenar los dos campos", "Alerta");
            else
            {
                fnGenerateAFNDA();
                gridAdd.Children[1].Focus();
            }


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
                DT = new DataTable("AFND");
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
                lblStados.Content += "\tCasos: ";
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
                GenerarAFDA();
                if (DT.Rows.Count > 0) {
                    GridAFDA.DataContext = DT.DefaultView;
                    lblAFDA.Visibility = Visibility.Visible;
                    GridAFDA.Visibility = Visibility.Visible;
                    MessageBox.Show("AFD Generado");
                }
                else
                {
                    lblAFDA.Visibility = Visibility.Hidden;
                    GridAFDA.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Errro: " + error.Message + "\nEn: " + error.StackTrace);
            }

        }

        /// <summary>
        /// Inico para genarar de AFNDA a AFDA
        /// </summary>
        /// <returns></returns>
        private void GenerarAFDA()
        {
            try
            {
                DT = new DataTable("AFDA");
                NumRow = 0;
                ListAFDA = new List<Transicion>();
                //Columnas
                DT.Columns.Add("Estados");
                for (int i = 0; i < Convert.ToInt32(txtNumCase.Text); i++)
                {
                    DT.Columns.Add("Caso: " + i);
                }
                //Mapear estados del AFNDA
                int FlagStart = -1;
                List<Entidades.Transicion> transicions = new List<Entidades.Transicion>();
                foreach (DataRowView row in GridAFNDA.Items)
                {
                    //Creamos un objetos de estado
                    Entidades.Transicion transicion = new Transicion();
                    //se guarda el caso inicial
                    string[] EstInit;
                    EstInit = row[0].ToString().Replace("->","").Replace("*","").Split(',');
                    bool isTeminal = IsTerminal(row[0].ToString());
                    bool isInicial = IsInicial(row[0].ToString());
                    transicion.EstIncio = EstInit;
                    transicion.flagStart = isInicial;
                    if (transicion.flagStart && FlagStart == -1)
                        FlagStart = transicions.Count();
                    
                    transicion.flagTerminal = isTeminal;
                    List<Caso> casos = new List<Caso>();
                    
                    for (int i = 0; i < Convert.ToInt32(txtNumCase.Text); i++)
                    {
                        //Creamos nuevo caso
                        Caso caso = new Caso();

                        string[] EstFin;
                        EstFin = row[i + 1].ToString().Split(',');
                        //Si se encuentra con mas de un estado final entonces es el incial.
                        if (EstFin.Length > 1)
                        {
                            //transicion.flagStart = true;
                            caso.newEst = true;
                        }
                        //Si tiene estado final entonces no es un estado terminal
                        if (!String.IsNullOrEmpty(EstFin[0]))
                        {
                            isTeminal = false;
                        }
                        //Guardamos el caso.
                        caso.CasoN = i;
                        caso.EstFin = EstFin;
                        casos.Add(caso);
                    }
                    //transicion.flagTerminal = isTeminal;
                    transicion.casos = casos;
                    transicions.Add(transicion);
                }
                //Se guarda el estado incial y se setean las variables de estado terminal
                if (FlagStart == -1)
                {
                    MessageBox.Show("Error: No existe estado inical valido!", "Error");
                    DT.Clear();
                    return;
                }
                else
                {
                    ListAFDA.Add(transicions[FlagStart]);
                    GenerarAFDALoop(transicions);
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show("Error: " + Error.Message + "\n" + Error.StackTrace, "Error");
            }
        }

        private bool IsInicial(string p)
        {
            if (p.IndexOf("->") >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsTerminal(string p)
        {
            if (p.IndexOf("*") >= 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        private void GenerarAFDALoop(List<Transicion> ListAFDNA)
        {
            //Guardamos el row actual
            DT.Rows.Add();
            int localRow = NumRow;
            //Se cuarda el estado generado
            if (ListAFDA[localRow].flagStart)
            {
                DT.Rows[localRow][0] = "->" + string.Join(",", ListAFDA[localRow].EstIncio);
                
            }
            else if (ListAFDA[localRow].flagTerminal)
            {
                DT.Rows[localRow][0] = "*" + string.Join(",", ListAFDA[localRow].EstIncio);
            }
            else if (!ListAFDA[localRow].flagTerminal && !ListAFDA[localRow].flagStart)
            {
                DT.Rows[localRow][0] = string.Join(",", ListAFDA[localRow].EstIncio);
            }
            //Se guardan los casos en la tabla            
            foreach (Caso caso in ListAFDA[localRow].casos)
            {
                string estFin = string.Join(",", caso.EstFin);
                DT.Rows[localRow][caso.CasoN + 1] = estFin;

                if (caso.newEst && !string.IsNullOrEmpty(estFin))
                {
                    if (localRow > Convert.ToInt32(txtNumEst.Text))
                    {
                        string ante = DT.Rows[localRow - 1][caso.CasoN + 1].ToString();
                        if (IsNotBucle(estFin, ante))
                            GenerarNuevoEstado(ListAFDNA, caso.EstFin);
                        else
                        {
                            MessageBox.Show("Error: Se genero un buble...", "ERROR");
                            NumRow = 0;
                            ListAFDA.Clear();
                            DT.Clear();
                            GridAFDA.DataContext = DT.DefaultView;
                            break;
                        }
                    } else
                        GenerarNuevoEstado(ListAFDNA, caso.EstFin);

                }
            }

        }

        private bool IsNotBucle(string actual, string pasado)
        {
            bool flag = false;
            int cont = 0;
            if (pasado.Length == actual.Length - 1 || pasado.Length == actual.Length - 2)
            {
                foreach (char c in pasado)
                {
                    if (c != actual[cont])
                        flag = true;
                    cont++;
                }
            if (pasado.IndexOf(actual[actual.Length - 1]) < 0)
                flag = true;

            }
            else
            {
                flag = true;
            }
            return flag;
        }

        private void GenerarNuevoEstado(List<Transicion> listAFNDA, string[] estFin)
        {
            //Eliminar estados repetidos          
            string[] B = estFin.Distinct().ToArray();
            string estadoNuevo = string.Join(",", B);
                
            //Verifico si el estado existe
            bool flagExist = false;
            foreach (Transicion itmen in ListAFDA)
            {
                string estadoAct = string.Join(",", itmen.EstIncio);
                if (estadoAct == estadoNuevo)
                {
                    //Si existe retorno la lista intacta
                    flagExist = true;
                }
            }
            if (flagExist)
                return;
            //Se comienzan a generar los nuevos estados si es que estos existen
            if (estFin.Length > 0)
            {
                //Se crea la nueva trasicion del nuevo caso
                Transicion transicion = new Transicion();
                transicion.EstIncio = estFin;
                //Se leen letra por letra del nuevo estado

                //Guardo las transiciones por letra del nuevo estado
                List<Transicion> transNew = new List<Transicion>();
                foreach (string estado in estFin)
                {
                    //Obtengo la transicion del estado
                    Transicion transEstado = listAFNDA.Find(x => x.EstIncio.Contains(estado));
                    if (transEstado.flagTerminal)
                        transicion.flagTerminal = true;
                    transNew.Add(transEstado);
                }
                //Guardo los nuevos casos
                List<Caso> newCasos = new List<Caso>();
                for (int i = 0; i < Convert.ToInt32(txtNumCase.Text); i++)
                {
                    Caso caso = new Caso();
                    caso.CasoN = i;
                    string stfinal = "";
                    for (int c = 0; c < transNew.Count; c++)
                    {
                        string estaFin = string.Join(",", transNew[c].casos[i].EstFin);
                        if (string.IsNullOrEmpty(stfinal) && !string.IsNullOrEmpty(estaFin))
                            stfinal += estaFin;
                        else if (!string.IsNullOrEmpty(estaFin))
                            stfinal += "," + estaFin;
                    }
                    string[] stFin = stfinal.Split(',').Distinct().ToArray();
                    caso.EstFin = stFin;
                    //Verifico si el estado existe
                    flagExist = false;
                    estadoNuevo = stfinal;
                    foreach (Transicion itmen in ListAFDA)
                    {
                        string estadoAct = string.Join(",", itmen.EstIncio);
                        if (estadoAct == estadoNuevo)
                        {
                            //Si existe retorno la lista intacta
                            flagExist = true;
                        }
                    }
                    if (flagExist)
                        caso.newEst = false;
                    else
                        caso.newEst = true;
                    newCasos.Add(caso);
                }
                transicion.casos = newCasos;
                ListAFDA.Add(transicion);
                NumRow++;
                GenerarAFDALoop(listAFNDA);
            }
            return;
        }

        /// <summary>
        /// Limpia el sistema para introducir un nuevo AFNDA
        /// </summary>
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {            
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Evento que registra nuevas transisciones o parametros en el Grid AFNDA
        /// </summary>
        private void BtnAddParam_Click(object sender, RoutedEventArgs e)
        {
            if (cmbInit.SelectedIndex >= 0 && cmbCase.SelectedIndex >= 0 && cmbEnd.SelectedIndex >= 0)
            {
                DataRowView view = (DataRowView)GridAFNDA.SelectedItem;
                ComboBoxItem item = (ComboBoxItem)cmbCase.SelectedValue;
                int caso = Convert.ToInt32(item.Content.ToString()) + 1;
                item = (ComboBoxItem)cmbEnd.SelectedValue;
                string estFin = item.Content.ToString();

                if (!String.IsNullOrWhiteSpace(view[caso].ToString()) && view[caso].ToString().IndexOf(estFin) == -1)
                    view[caso] += "," + estFin;
                else if (String.IsNullOrWhiteSpace(view[caso].ToString()))
                    view[caso] += estFin;
                else
                    MessageBox.Show("Alerta: Ya existe este flujo", "Alerta");
            }
            else
            {
                MessageBox.Show("Alerta: Favor de selecionar los tres campos", "Alerta");
            }
        }

        #region Selecionar estado en combobox y en el grid
        private void CmbInit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridAFNDA.SelectedIndex = cmbInit.SelectedIndex;
        }


        #endregion

        private void BtnEmpiParam_Click(object sender, RoutedEventArgs e)
        {
            int row = cmbInit.SelectedIndex;
            int collum = cmbCase.SelectedIndex;
            if (row >= 0 && collum >= 0)
            {
                DataRowView DR = (DataRowView)GridAFNDA.SelectedItem;

                DR[collum + 1] = "";
            }
        }

        private void GridAFNDA_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            cmbInit.SelectedIndex = GridAFNDA.SelectedIndex;
        }

        private void GridAFNDA_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender != btnEmpiParam)
                {
                    int collum = GridAFNDA.CurrentCell.Column.DisplayIndex;
                    cmbCase.SelectedIndex = collum - 1;
                }
            }
            catch (Exception)
            {
            }
        }

        private void GridAFDA_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {//Obtienes la fila
                DataRow row = item.Row;
                //Inspeccionas el row por el campo para validar el valor
                if (row[0].ToString().IndexOf('>') >= 0)
                {//Cambias el color
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                }
                else if (row[0].ToString().IndexOf('*') >= 0)
                {                    
                    //Cambias el color
                    e.Row.Background = new SolidColorBrush(Colors.LightSalmon);
                }
            }
        }

        private void btnIsChgParam_Click(object sender, RoutedEventArgs e)
        {
            int row = cmbInit.SelectedIndex;
            if (row >= 0)
            {
                DataRowView DR = (DataRowView)GridAFNDA.SelectedItem;
                switch (btnIsChgParam.Content.ToString())
                {
                    case "Inicial":
                        DR[0] = "->" + DR[0].ToString().Replace("->", "").Replace("*", "");
                        btnIsChgParam.Content = "Final";
                        break;
                    case "Final":
                        DR[0] = "*" + DR[0].ToString().Replace("->", "").Replace("*", "");
                        btnIsChgParam.Content = "Normal";
                        break;
                    default:
                        DR[0] = DR[0].ToString().Replace("->","").Replace("*","");
                        btnIsChgParam.Content = "Inicial";
                        break;
                }
            }
        }
    }
}
