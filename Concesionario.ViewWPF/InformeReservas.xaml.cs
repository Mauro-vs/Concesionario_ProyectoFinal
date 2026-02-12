using Concesionario.Controller.API;
using Concesionario.Model.Informes;
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

namespace Concesionario.ViewWPF
{
    /// <summary>
    /// Lógica de interacción para InformeReservas.xaml
    /// </summary>
    public partial class InformeReservas : Window
    {
        private readonly ReporteApi _reporteApi = new ReporteApi();
        public InformeReservas()
        {
            InitializeComponent();

            var ds = _reporteApi.GetReservasDataSet(); // tu método en ReporteApi
            var rpt = new crReservas(); // tu .rpt en Concesionario.Model

            rpt.SetDataSource(ds); 
            crViewer.ReportSource = rpt;
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
