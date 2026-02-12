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
using System.Windows.Shapes;
using Concesionario.Controller.API;
using Concesionario.Model.Informes;

namespace Concesionario.ViewWPF.Views
{
    /// <summary>
    /// Lógica de interacción para InformeClientesWindow.xaml
    /// </summary>
    public partial class InformeClientesWindow : Window
    {
        private readonly ReporteApi _reporteApi = new ReporteApi();

        public InformeClientesWindow()
        {
            InitializeComponent();

            var ds = _reporteApi.GetClientesDataSet(); // tu método en ReporteApi
            var rpt = new crClientes();               // tu .rpt en Concesionario.Model

            rpt.SetDataSource(ds);
            crViewer.ReportSource = rpt;
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
