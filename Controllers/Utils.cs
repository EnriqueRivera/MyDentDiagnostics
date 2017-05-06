using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public static void AddEmptyItemToComboBoxes(Grid gdContent)
        {
            var controls = gdContent.Children.OfType<ComboBox>().ToList();

            foreach (var item in controls)
            {
                item.Items.Insert(0, string.Empty);
                item.SelectedIndex = 0;
            }
        }

        public static string GetGridControlsContent(Grid gdContent)
        {
            StringBuilder content = new StringBuilder();
            foreach (var item in gdContent.Children)
            {
                if (item is Label)
                    content.Append((item as Label).Content);
                else if (item is TextBox)
                    content.Append((item as TextBox).Text);
                else if (item is ComboBox)
                    content.Append((item as ComboBox).SelectedValue.ToString());
            }
            return content.ToString();
        }

        public static string GetNoteDetail(Model.ProgressNote note, ProgressNoteDetail detail)
        {
            Model.ProgressNoteDetail noteDetail = note.ProgressNoteDetails.Where(d => d.DetailName == detail.ToString()).FirstOrDefault();
            return noteDetail == null ? "" : noteDetail.DetailDescription;
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
