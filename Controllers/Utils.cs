using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class Utils
    {
        public static Microsoft.Win32.OpenFileDialog GetFileDialogWithImageExtensions()
        {
            // Configure open file dialog box 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "";

            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }

            dlg.Filter = String.Format("{2} ({3})|{3}{1}{0}", dlg.Filter, sep, "All Files", "*.*");
            dlg.DefaultExt = ".png"; // Default file extension 

            return dlg;
        }

        public static bool AddDetailsToProgressNote(ProgressNote progressNote, Dictionary<string, string> noteDetials)
        {
            bool detailsAdded = true;
            foreach (var item in noteDetials)
            {
                Model.ProgressNoteDetail noteDetail = new Model.ProgressNoteDetail()
                {
                    ProgressNoteId = progressNote.ProgressNoteId,
                    DetailName = item.Key,
                    DetailDescription = item.Value
                };

                detailsAdded &= BusinessController.Instance.Add(noteDetail);
            }

            return detailsAdded;
        }
    }

    public interface IProcedureInfo
    {
        string GetProcedureContent();
    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public enum ProgressNoteType
    {
        GENERAL,
        BIOPSIA_DE_TEJIDOS_BLANDOS,
        CIRUGIA_BUCAL,
        DETARTRAJE_Y_CURETAJE,
        OBTURACION_DENTAL,
        ODONTECTOMIA,
        PROFILAXIS_DENTAL,
        PROTESIS_PARCIAL_FIJA
    }

    public enum ProgressNoteDetail
    {
        GENERAL_VITAL_SIGNS,
        GENERAL_DESCRIPTION,
        ESPECIFICA_PROCEDIMIENTO,
        ESPECIFICA_INDICACIONES,
        ESPECIFICA_MEDICAMENTO,
        ESPECIFICA_HALLAZGOS,
        ESPECIFICA_PRONOSTICOS,
        ESPECIFICA_DIAGNOSTICO,
        ESPECIFICA_TRATAMIENTO_PROX_CITA
    }

    public class CustomViewModel<T> where T : class
    {
        private ObservableCollection<T> _allData = new ObservableCollection<T>();

        public CustomViewModel(System.Linq.Expressions.Expression<Func<T, bool>> findBy, string sortBy, string sortDirection)
        {
            var param = Expression.Parameter(typeof(T), "item");
            var sortExpression = Expression.Lambda<Func<T, object>>
                                    (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            var allData = Controllers.BusinessController.Instance.FindBy<T>(findBy).ToList();

            switch (sortDirection.ToLower())
            {
                case "asc":
                    allData = allData.AsQueryable<T>().OrderBy<T, object>(sortExpression).ToList();
                    break;
                default:
                    allData = allData.AsQueryable<T>().OrderByDescending<T, object>(sortExpression).ToList();
                    break;
            }

            _allData = new ObservableCollection<T>(allData.ToList());
        }

        public CustomViewModel(List<T> data)
        {
            _allData = new ObservableCollection<T>(data);
        }

        public ObservableCollection<T> ObservableData
        {
            get { return _allData; }
        }
    }
}
