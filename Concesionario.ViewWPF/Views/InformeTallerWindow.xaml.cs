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

namespace Concesionario.ViewWPF.Views
{
    /// <summary>
    /// Lógica de interacción para InformeTallerWindow.xaml
    /// </summary>
    public partial class InformeTallerWindow : Window
    {
        private readonly ReporteApi _reporteApi = new ReporteApi();
        public InformeTallerWindow()
        {
            InitializeComponent();

            var ds = _reporteApi.GetTallerDataSet();
            var rpt = new crTaller();

            rpt.SetDataSource(ds);
            crViewer.ReportSource = rpt;
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
