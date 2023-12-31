﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ChestVault.Schemas;

namespace ChestVault
{
    public class DataGrid
    {
        public bool SelectAble = true;
        public bool SettingsDisplay;

        public int DisplayLimit;
        public int CurrentPage = 0;
        public int LastPage = 0;


        private int TextCount;

        public int Selected = -1;
        public int DoubleClick = -1;

        public Form Sender;

        public DataGridForm ThisForm = null;
        public int Precion = 2;
        public Color FormColor;

        public List<DataGridColumn> Column = new List<DataGridColumn>();

        // Panels Controls

        public Panel HeaderPanel;
        public Panel BodyPanel;
        public Panel SettingsPanel;

        // Header Info
        public Font HeaderFont;
        public Color HeaderFontColor;
        public Color HeaderBackGroundColor;

        // Labels Value
        public Font FontType;
        public Color FontColor;
        public Color backGroundColor;
        public Color Back2Color;

        public Color HoverForeColor;
        public Color HoverBackColor;

        public Color SelectedForeColor;
        public Color SelectedBackColor;

        /// <summary>
        /// Standard Values Setup For DataGrid
        /// </summary>
        public void StandardConfiguarations()
        {
            SelectAble = true;
            SettingsDisplay = true;
            Precion = 2;
            DisplayLimit = 10;
            CurrentPage = 0;
            LastPage = 1;

            FormColor = Color.FromArgb(31, 31, 31);

            HeaderFont = new Font("HSIshraq-Light", 20);
            FontType = new Font("HSIshraq-Light", 18);

            HeaderFontColor = Color.Black;
            HeaderBackGroundColor = Color.White;

            FontColor = Color.White;
            backGroundColor = Color.FromArgb(31, 31, 31);
            Back2Color = Color.FromArgb(39, 39, 39);

            HoverForeColor = Color.Black;
            HoverBackColor = Color.FromArgb(204, 204, 204);

            SelectedBackColor = Color.FromArgb(168, 70, 34);
            SelectedForeColor = Color.White;


        }
        public DataGridForm DisplayForm(Form send)
        {
            if(ThisForm == null) ThisForm = new DataGridForm();
            ThisForm.TopLevel = false;
            ThisForm.AutoScroll = true;
            ThisForm.Show();
            ThisForm.Dock = DockStyle.Fill;

            HeaderPanel = ThisForm.Controls.Find("HeaderPanel",false).First() as Panel;
            BodyPanel = ThisForm.Controls.Find("BodyPanel", false).First() as Panel;
            SettingsPanel = ThisForm.Controls.Find("SettingsPanel", false).First() as Panel;

            ThisForm.ThisDataGrid = this;
            Sender = send;

            if (Column.Count == 0) SettingsPanel.Visible = false;
            StandardConfiguarations();
            return ThisForm;
        }
        public void ReloadDataGrid()
        {
            if (SettingsDisplay)
            {
                LastPage = (int)(Column[0].Text.Count / DisplayLimit) + ((Column[0].Text.Count % DisplayLimit != 0) ? 1 : 0);
            
            }
            else
            {
                DisplayLimit = Column[0].Label.Count;
            }

            if (CurrentPage > LastPage - 1) CurrentPage = LastPage;

            #region Creating the Headers
            if (Column[0].Label.Count == 0 && Column[0].HeaderTitle != "")
                for (int i = 0; i < Column.Count; i++)
                {
                    CreateHeader(i);
                }
            #endregion

            #region Adding Controls and Fixing Their Values
            int GoingToSelect = 0;
            for (int i = 0; i < Column.Count; i++)
            {
                if (Column[i].dataType == DataGridColumn.DataType.Double) Column[i] = DoubleTrim(Column[i]);

                Column[i].Label[0].Text = Column[i].HeaderTitle;

                int Limit = DisplayLimit;

                if (CurrentPage == LastPage && SettingsDisplay)
                {
                    Limit = Column[i].Text.Count % DisplayLimit == 0 ? DisplayLimit : Column[i].Text.Count % DisplayLimit;
                }

                int x = (CurrentPage * DisplayLimit) + 1;
                int index = x % ((DisplayLimit == 0) ? 1 : DisplayLimit);

                for (; index <= Limit && Column[i].Label.Count != 1; index++, x++)
                {
                    if (index >= Column[i].Label.Count) break;
                    if (Column[i].Text.Count >= x)
                    {
                        Column[i].Label[index].Text = Column[i].Text[x - 1];
                        Column[i].Label[index].Visible = true;
                        GoingToSelect = x - 1;
                    }
                    else Column[i].Label[index].Visible = false;

                }
                for (x -= 1; x < Column[i].Text.Count; x++)
                {
                    if (DisplayLimit != 0)
                    {
                        if (x >= DisplayLimit) break;
                    }
                    CreateLabel(i, x);
                    GoingToSelect = x;
                }
            }
            #endregion

            if(SettingsDisplay) ThisForm.FixData();
            SettingsPanel.Visible = (SettingsDisplay && LastPage > 1);

            if (SelectAble)
            {
                if (TextCount != Column[0].Text.Count)
                {
                    TextCount = Column[0].Text.Count;
                    RemoveSelection(Selected);
                    Selected = (Column[0].Text.Count == 0) ? -1 : GoingToSelect;
                    SelectControl(Selected);
                }
            }
        }
        public void CreateHeader(int Row)
        {
            Label Header = new Label();
            Header.Text = Column[Row].HeaderTitle;
            int XPos = ThisForm.Width;
            for (int i = 0; i <= Row; i++)
            {
                XPos -= Column[i].LabelSize.Width;
            }
            Header.AutoSize = false;
            Header.Size = Column[Row].LabelSize;
            Header.Location = new Point(XPos, 0);

            Header.Font = HeaderFont;
            Header.ForeColor = HeaderFontColor;
            Header.BackColor = HeaderBackGroundColor;

            Header.Padding = new Padding(0);
            Header.TextAlign = ContentAlignment.MiddleCenter;

            HeaderPanel.Size = new Size(Column.Count * Column[Row].LabelSize.Width, Column[Row].LabelSize.Height);
            HeaderPanel.Controls.Add(Header);
            Column[Row].Label.Add(Header);
            Header.Click += new EventHandler(HeaderClick);
        }
        public void CreateLabel(int Row,int index)
        {
            Label Itemname = new Label();
            Itemname.Text = Column[Row].Text[index];

            int XPoint = ThisForm.Width;
            for(int i = 0; i <= Row; i++)
            {
                XPoint -= Column[i].LabelSize.Width;
            }
            Itemname.AutoSize = false;
            Itemname.Size = Column[Row].LabelSize;
            Itemname.Location = new Point(XPoint,Itemname.Size.Height * index);
            Itemname.Name = index.ToString();
            Itemname.Font = FontType;
            Itemname.ForeColor = FontColor;
            Itemname.BackColor = (index % 2 == 0) ? Back2Color : backGroundColor;

            Itemname.Padding = new Padding(0);
            Itemname.TextAlign = ContentAlignment.MiddleCenter;

            BodyPanel.Controls.Add(Itemname);
            Column[Row].Label.Add(Itemname);

            if (SelectAble)
            {
                Itemname.MouseHover += new EventHandler(LabelHoverOn);
                Itemname.MouseLeave += new EventHandler(LabelLeave);
                Itemname.Click += new EventHandler(LabelClick);
                Itemname.DoubleClick += new EventHandler(LabelDoubleClick);
            }
        }

        #region Select and Hover Effects

        private void SelectControl(int Row)
        {
            if (Row < 0 || Column[0].Text.Count < 1) return;

            for (int i = 0; i < Column.Count; i++)
            {
                Column[i].Label[Row + 1].ForeColor = SelectedForeColor;
                Column[i].Label[Row + 1].BackColor = SelectedBackColor;
            }

            Sender.Text = "Click";
        }
        private void RemoveSelection(int Row)
        {
            if (Row < 0 || Column[0].Text.Count < 1) return;
            for (int i = 0; i < Column.Count; i++)
            {
                Column[i].Label[Row + 1].ForeColor = FontColor;
                Column[i].Label[Row + 1].BackColor = (Selected + 1 % 2 == 0) ? Back2Color : backGroundColor;
            }
        }

        private void HoverControl(int Row)
        {
            if (Row != Selected + 1)
            {
                for (int i = 0; i < Column.Count; i++)
                {
                    Column[i].Label[Row].ForeColor = HoverForeColor;
                    Column[i].Label[Row].BackColor = HoverBackColor;
                }
            }
        }

        private void RemoveHover(int Row)
        {
            if (Row != Selected + 1)
            {
                for(int i = 0; i < Column.Count; i++)
                {
                    Column[i].Label[Row].ForeColor = FontColor;
                    Column[i].Label[Row].BackColor = (Row % 2 == 0) ? Back2Color : backGroundColor;
                }
            }
        }

        #endregion

        #region Unused Functions And Experiments Functions
        public DataGridColumn ReverseValues(DataGridColumn column)
        {

            DataGridColumn column2 = column;
            column2.Text.Clear();
            for (int i = column.Text.Count - 1; i >= 0; i--)
            {
                column2.Text.Add(column.Text[i]);
            }

            return column2;
        }
        #endregion

        #region Standard Label Functions
        private void LabelHoverOn(object sender, EventArgs e)
        {
            int hover = int.Parse(((Label)sender).Name) + 1;
            HoverControl(hover);
        }
        private void LabelLeave(object sender, EventArgs e)
        {
            int t = int.Parse(((Label)sender).Name) + 1;
            RemoveHover(t);
        }
        private void LabelClick(object sender, EventArgs e)
        {
            if (Selected >= 0) RemoveSelection(Selected);

            Selected = int.Parse(((Label)sender).Name);
            
            SelectControl(Selected);
        }
        private void LabelDoubleClick(object sender, EventArgs e)
        {
            DoubleClick = int.Parse(((Label)sender).Name);
            Sender.Text = "DoubleClick";
        }
        #endregion

        #region Header Functions
        private void HeaderHover(object sender, EventArgs e)
        {
            for (int i = 0; i < Column.Count; i++)
            {
                if (Column[i].Label[0] == sender)
                {

                }
            }
        }
        public void HeaderLeave(object sender, EventArgs e)
        {
            for (int i = 0; i < Column.Count; i++)
            {
                if (Column[i].Label[0] == sender)
                {

                }
            }
        }
        private void HeaderClick(object sender, EventArgs e)
        {
           for(int i = 0; i < Column.Count; i++)
            {
                if (Column[i].Label[0] == sender) Sort(i, 3);
            }
        }

        #endregion

        #region Sorting Functions
        public Int64 GetAsci(string text, int acu)
        {
            long sum = 0;
            long power = (long) Math.Pow(10, acu);
            var asci =  text.ToUpper().ToCharArray();

            for (int i = 0; i < acu && i < asci.Count(); i++)
            {
                sum += asci[i] * (long) (power / Math.Pow(10, i));
            }
            return (sum);
        }
        public void Sort(int Row, int acu)
        {
            List<string> copy = Column[Row].Text.ToList();
            if (Column[Row].Higher)
            {
                for (int l = 0; l < Column.Count; l++)
                {
                    Column[l].Text.Reverse();
                }
            }
            else 
            {
                copy.Sort();
                int i = 0, j = 0;
                while (true)
                {
                    while (copy[i] != Column[Row].Text[j])
                    {
                        j++;
                        if (j == copy.Count())
                            j = 0;
                    }
                    for (int l = 0; l < Column.Count; l++)
                    {
                        string tmp = Column[l].Text[i];
                        Column[l].Text[i] = Column[l].Text[j];
                        Column[l].Text[j] = tmp;
                    }
                    i++;
                    if (i == copy.Count())
                        break;
                }
            }
            for (int l = 0; l < Column.Count; l++)
            {
                Column[Row].Higher = false;
            }
            Column[Row].Higher = true;
            ReloadDataGrid();
            return;
            //for (int i = 0; i < Column[Row].Text.Count; i++)
            //{
            //    for (int x = 0; x < Column[Row].Text.Count; x++)
            //    {
            //        if (Column[Row].Higher)
            //        {
            //            double s1;
            //            double s2;
            //            if (Column[Row].dataType == 0)
            //            {
            //                s1 = double.Parse(Column[Row].Text[i]);
            //                s2 = double.Parse(Column[Row].Text[x]);
            //            }
            //            else
            //            {
            //                var l = Column[Row].Text[i].Length > Column[Row].Text[x].Length ? Column[Row].Text[i].Length : Column[Row].Text[x].Length;
            //                if (Column[Row].dataType == DataGridColumn.DataType.Text)
            //                    l = acu;
            //                s1 = GetAsci(Column[Row].Text[i], l);
            //                s2 = GetAsci(Column[Row].Text[x], l);
            //            }
            //            if (s1 >= s2)
            //            {
            //                for (int l = 0; l < Column.Count; l++)
            //                {
            //                    string tmp = Column[l].Text[i];
            //                    Column[l].Text[i] = Column[l].Text[x];
            //                    Column[l].Text[x] = tmp;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            double s1;
            //            double s2;
            //            if (Column[Row].dataType == 0)
            //            {
            //                s1 = double.Parse(Column[Row].Text[i]);
            //                s2 = double.Parse(Column[Row].Text[x]);
            //            }
            //            else
            //            {
            //                var l = Column[Row].Text[i].Length > Column[Row].Text[x].Length ? Column[Row].Text[i].Length : Column[Row].Text[x].Length;
            //                if (Column[Row].dataType == DataGridColumn.DataType.Text)
            //                    l = acu;
            //                s1 = GetAsci(Column[Row].Text[i], l);
            //                s2 = GetAsci(Column[Row].Text[x], l);
            //            }
            //            if (s1 <= s2)
            //            {
            //                for (int l = 0; l < Column.Count; l++)
            //                {
            //                    string tmp = Column[l].Text[i];
            //                    Column[l].Text[i] = Column[l].Text[x];
            //                    Column[l].Text[x] = tmp;
            //                }
            //            }
            //        }
            //    }
            //}
            //Column[Row].Higher = !Column[Row].Higher;
            //ReloadDataGrid();
        }

        #endregion

        #region Trimming Functions
        private DataGridColumn DoubleTrim(DataGridColumn column)
        {
            double pow = 1.0;
            for (int i = 0; i < Precion; i++) pow *= 10.0;

            for (int i = 0; i < column.Text.Count; i++)
            {
                column.Text[i] = (((int) (double.Parse(column.Text[i]) * pow)) / pow).ToString();

            }
            return column;
        }

        #endregion

        #region DataGridFunctions

        public void RemoveRow(int row)
        {
            for (int i = 0; i < Column.Count;i++)
            {
                Column[i].Text.RemoveAt(row);
            }
            ReloadDataGrid();
        }
        #endregion
    }
    public class DataGridColumn
    {
        public enum DataType
        {
            Double,
            Text,
            Date
        }
        public bool Higher = false;
        public DataType dataType;
        public string HeaderTitle;
        public List<string> Text = new List<string>();
        public List<Label> Label = new List<Label>();
        public Size LabelSize;
    }

    public class DisplayedItemsSchemas
    {
        public List<ItemsSchema> ItemSchema;
        public List<PurchaseSchema> PurchaseSchema;
        public List<RecitesSchema> RecitesSchema;
    }
}
