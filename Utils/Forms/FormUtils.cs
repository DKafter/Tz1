using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tz.Utils
{
    public static class FormUtils
    {
        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        public static IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl))
                                      .Concat(controls);
        }

        public static bool EmptyRedWhite(Control control)
        {
            var controls = GetAll(control);
            var redBackground = Color.Red;
            var transparent = Color.White;
            var index = 0;
            foreach(var c in controls)
            {

                if(c is TextBox || c is ComboBox)
                {
                    if(string.IsNullOrEmpty(c.Text.Trim()))
                    {
                        c.BackColor = redBackground;
                        index++;
                    } 
                    else
                    {
                        c.BackColor = transparent;
                    }
                }
            }

            if(index > 0)
            {
                return false;
            }
            return true;
        }

        public static void ClearAllSubitem(ListView dtg)
        {
            foreach (ListViewItem listViewItem in dtg.Items)
            {
                listViewItem.Remove();
            }
        }

        public static void UpdateItemsToClick(Control control, object sender, MouseEventArgs e)
        {
            var s = ((ListView)sender);
            var lst = new List<Control>();
            var list = FormUtils.GetAll(control);
            var hitTest = ((ListView)sender).HitTest(e.Location);
            var itemIndex = hitTest.Item.Index;
            foreach (Control l in list)
            {
                if (l is TextBox
                    || l is ComboBox
                    || l is DateTimePicker)
                {
                    lst.Add(l);
                }
            }

            if (s.SelectedItems.Count > 0)
            {
                ArrayList selectedList = new ArrayList();
                
                for (int i = 0; i < lst.Count; ++i)
                { 
                    if (lst[i] is DateTimePicker)
                    {
                        //selectedList.Add(DateTime.Parse(s.Items[selectedIndex].SubItems[i].Text));
                    }
                    else
                    {
                        selectedList.Add(s.Items[itemIndex].SubItems[i].Text);
                        Console.WriteLine(s.Items[itemIndex].SubItems[i].Text);
                    }
                }

                for (int i = 0; i < selectedList.Count; ++i)
                {
                    if (lst[i] is DateTimePicker)
                    {
                        //((DateTimePicker)lst[i]).Value = (DateTime)selectedList[i];
                    }
                    else
                    {
                        Console.WriteLine(lst[i]);
                        lst[i].Text = (string)selectedList[i];
                    }
                }
                
            }

        }
        
        public static bool WriteToDataGrid(ComboBox dtg, IList ar)
        {
            dtg.Items.Clear();
            dtg.Text = string.Empty;
            foreach (var c in ar)
            {
                if (!dtg.Items.Contains(ar) && c != null)
                    dtg.Items.Add(c.ToString());
            }
            if ((dtg != null && string.IsNullOrEmpty(dtg.Text)) && dtg.Items.Count > 0)
            {
                dtg.Text = dtg.Items[0].ToString();
                return true;
            }
            return false;
        }

        public static void WriteToDataGrid(ListView dtg, List<List<String>> ar)
        {
            dtg.Items.Clear();
            dtg.Items.AddRange(
                ar.Select(
                    (row, index) => new ListViewItem(
                        new[] { index.ToString() }
                            .Concat(row)
                            .ToArray()
                        )
                    )
                    .ToArray()
                );
        }
    }
}
