using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Sanction_Memo
{
    public partial class Form2 : MetroForm
    {
        double gridTotal = 0;
        string fileName = DateTime.Now.ToFileTime() + ".docx";
        Boolean fileLocationPromted = false;
        DocX doc;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (fileLocationPromted)
            {
                doc.Save();
                Process.Start("WINWORD.EXE", fileName);
            }
            else
            {
                showNotification("Alert !", "Save at least one page first !", Properties.Resources.error_48);
            }
        }

        private void metroTextBox3_Click(object sender, EventArgs e)
        {
           
        }

        public string words(double? numbers, Boolean paisaconversion = false)
        {
            var pointindex = numbers.ToString().IndexOf(".");
            var paisaamt = 0;
            if (pointindex > 0)
                paisaamt = Convert.ToInt32(numbers.ToString().Substring(pointindex + 1, 2));

            int number = Convert.ToInt32(numbers);

            if (number == 0) return "Zero";
            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append(" ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }

            if (paisaamt == 0 && paisaconversion == false)
            {
                sb.Append("ruppes only");
            }
            else if (paisaamt > 0)
            {
                var paisatext = words(paisaamt, true);
                sb.AppendFormat("rupees {0} paise only", paisatext);
            }
            return sb.ToString().TrimEnd();
        }
        private Boolean validateRequest()
        {

            gridTotal = 0;
            if (metroTextBox1.Text == "")
            {
                showNotification("Alert !", "Memo Number is required !", Properties.Resources.error_48);
                return false;
            }
            if (metroTextBox2.Text == "")
            {
                showNotification("Alert !", "Date is required !", Properties.Resources.error_48);
                return false;
            }
            if (metroTextBox3.Text == "")
            {
                showNotification("Alert !", "Amount is required !", Properties.Resources.error_48);
                return false;
            }

            if (metroTextBox4.Text == "")
            {
                showNotification("Alert !", "Head is required !", Properties.Resources.error_48);
                return false;
            }

            int i = 0;
            foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
            {

                if (i < bunifuCustomDataGrid1.Rows.Count - 1)
                {
                    if (row.Cells[0].Value is null)
                    {
                        row.Cells[0].ErrorText = "Serial Number Required";
                        showNotification("Alert !", "Fix issue(s) in grid !", Properties.Resources.error_48);
                        return false;
                    }
                    else
                    {
                        row.Cells[0].ErrorText = "";
                    }
                    if (row.Cells[1].Value is null)
                    {
                        row.Cells[1].ErrorText = "Office Name Required";
                        showNotification("Alert !", "Fix issue(s) in grid !", Properties.Resources.error_48);
                        return false;
                    }
                    else
                    {
                        row.Cells[1].ErrorText = "";
                    }
                    if (row.Cells[2].Value is null)
                    {
                        row.Cells[2].ErrorText = "Telephone Number Required";
                        showNotification("Alert !", "Fix issue(s) in grid !", Properties.Resources.error_48);
                        return false;
                    }
                    else
                    {
                        row.Cells[2].ErrorText = "";
                    }
                    if (row.Cells[3].Value is null)
                    {
                        row.Cells[3].ErrorText = "Amount Required";
                        showNotification("Alert !", "Fix issue(s) in grid !", Properties.Resources.error_48);
                        return false;
                    }
                    else
                    {
                        row.Cells[3].ErrorText = "";
                    }

                    i++;
                    gridTotal += Convert.ToDouble(row.Cells[3].Value);
                }
            }


            if (Convert.ToDouble(gridTotal) != Convert.ToDouble(metroTextBox3.Text))
            {
                showNotification("Alert !", "Amount miss-matching !", Properties.Resources.delete_sign_48);
                return false;
            }

            return true;
        }

        private void showNotification(string popupTitle, string popUpMsg, System.Drawing.Image image)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Image = image;
            popup.TitleText = popupTitle;
            popup.ContentFont = new System.Drawing.Font("Tahoma", 10);
            popup.ImagePadding = new Padding(20, 20, 20, 20);
            popup.ContentText = "\n" + popUpMsg;
            popup.Popup();



        }

        private void metroTextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as MetroTextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form1 = new Form1();
            form1.Closed += (s, args) => this.Close();
            form1.Show();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            if (!fileLocationPromted)
            {
                saveFileDialog1.FileName = fileName;
            }

            if (validateRequest() && !fileLocationPromted)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    fileLocationPromted = true;
                    doc = DocX.Create(saveFileDialog1.FileName);
                }

            }

            if (doc != null && validateRequest())
            {
                
                Formatting format = new Formatting();
                format.Size = 12;
                doc.InsertParagraph("FORMAL SANCTION").Bold().Alignment = Alignment.right;
                doc.InsertParagraph("Department of Posta: India").Alignment = Alignment.center;
                doc.InsertParagraph("O/o the Superintendent of Post Offices").Alignment = Alignment.center;
                doc.InsertParagraph("Rourkela Division, Rourkela- 769011").Alignment = Alignment.center;
                doc.InsertParagraph("");
                doc.InsertParagraph("");



                Table both = doc.AddTable(1, 2);
                both.SetBorder((TableBorderType)TableCellBorderType.Top, new Border((Xceed.Document.NET.BorderStyle)TableBorderType.Top, BorderSize.one, 1, Color.Transparent));
                both.SetBorder((TableBorderType)TableCellBorderType.Bottom, new Border((Xceed.Document.NET.BorderStyle)TableBorderType.Top, BorderSize.one, 1, Color.Transparent));
                both.SetBorder((TableBorderType)TableCellBorderType.Left, new Border((Xceed.Document.NET.BorderStyle)TableBorderType.Top, BorderSize.one, 1, Color.Transparent));
                both.SetBorder((TableBorderType)TableCellBorderType.Right, new Border((Xceed.Document.NET.BorderStyle)TableBorderType.Top, BorderSize.one, 1, Color.Transparent));

                both.Rows[0].Cells[0].SetBorder((TableCellBorderType)(TableBorderType)TableCellBorderType.Right, new Border((Xceed.Document.NET.BorderStyle)TableBorderType.Top, BorderSize.one, 1, Color.Transparent));
                both.Rows[0].Cells[1].SetBorder((TableCellBorderType)(TableBorderType)TableCellBorderType.Left, new Border((Xceed.Document.NET.BorderStyle)TableBorderType.Top, BorderSize.one, 1, Color.Transparent));

                both.Rows[0].Cells[0].Paragraphs.First().Append("Memo No.: " + metroTextBox1.Text);

                both.Rows[0].Cells[1].Paragraphs.First().Append("Dated at Rourkela the " + metroTextBox2.Text).Alignment = Alignment.right;
                doc.InsertTable(both);
                doc.InsertParagraph("");


                doc.InsertParagraph("                   Sanction of the Supdt. of Post offices, Rourkela Division, Rourkela is hereby accorded for payment of Rs. " +
                    metroTextBox3.Text + "/- (" + words(Convert.ToDouble(metroTextBox3.Text), true) + ") only begin the expenditure incurred towards the cost of payment of refueling" +
                    " and repairing of MMS vehicle.").Alignment = Alignment.both;

                doc.InsertParagraph("");


                doc.InsertParagraph("");
                Table t = doc.AddTable(bunifuCustomDataGrid1.Rows.Count + 1, 4);
                t.Alignment = Alignment.center;
                int i = 0;




                t.Rows[0].Cells[0].Width = 45.0;
                t.Rows[0].Cells[1].Width = 225.5;
                t.Rows[0].Cells[2].Width = 180.4;
                t.Rows[0].Cells[3].Width = 90.0;


                t.Rows[0].Cells[0].Paragraphs.First().Append("SL No").Bold();
                t.Rows[0].Cells[1].Paragraphs.First().Append("Particulars").Bold();
                t.Rows[0].Cells[2].Paragraphs.First().Append("Bill No/Date").Bold();
                t.Rows[0].Cells[3].Paragraphs.First().Append("Amount (in Rs.)").Bold();



                foreach (DataGridViewRow row in bunifuCustomDataGrid1.Rows)
                {


                    if (i < bunifuCustomDataGrid1.Rows.Count - 1)
                    {
                        t.Rows[i + 1].Cells[0].Paragraphs.First().Append(row.Cells[0].Value.ToString());
                        t.Rows[i + 1].Cells[1].Paragraphs.First().Append(row.Cells[1].Value.ToString());
                        t.Rows[i + 1].Cells[2].Paragraphs.First().Append(row.Cells[2].Value.ToString());
                        t.Rows[i + 1].Cells[3].Paragraphs.First().Append(row.Cells[3].Value.ToString());
                        i++;
                        //gridTotal += Convert.ToDouble(row.Cells[3].Value);
                    }
                }

                t.Rows[i + 1].Cells[2].Paragraphs.First().Append("Total").Alignment = Alignment.right;
                t.Rows[i + 1].Cells[3].Paragraphs.First().Append(Convert.ToDecimal(String.Format("{0:0.00}", gridTotal)).ToString());

                doc.InsertTable(t);
                doc.InsertParagraph("");
                doc.InsertParagraph("           The amount has already been paid and charged under the Head " +
                    metroTextBox4.Text + " .").Alignment = Alignment.both;
                doc.InsertParagraph("");
                doc.InsertParagraph("");

                doc.InsertParagraph("Supdt. Post Offices,").Alignment = Alignment.right;
                doc.InsertParagraph("Rourkela Division,").Alignment = Alignment.right;
                doc.InsertParagraph("Rourkela-769011").Alignment = Alignment.right;
                doc.InsertParagraph("");
                doc.InsertParagraph("");

                doc.InsertParagraph("Copy to:");
                doc.InsertParagraph("");
                string isAre = "";
                string bill = "";
                int actualItems = (bunifuCustomDataGrid1.Rows.Count - 1);
                if (actualItems > 1)
                {
                    isAre = "are";
                    bill = "bills";
                }
                else
                {
                    isAre = "is";
                    bill = "bill";
                }
                doc.InsertList(doc.AddList("The Postmaster, "+ metroTextBox6.Text+ " " + words(actualItems, true) + " " + bill + " in original " + isAre + " enclosed herewith for information and necessary action.", 0, ListItemType.Numbered, 1));
                doc.InsertList(doc.AddList("The Sub Postmaster, ROurkels-2 MDG, Rourkela-769002 for information.", 0, ListItemType.Numbered, 2)); 
                doc.InsertList(doc.AddList("The DA (P), Cuttack-753004.", 0, ListItemType.Numbered, 3));
                doc.InsertList(doc.AddList("Office copy.", 0, ListItemType.Numbered, 4));

                doc.InsertParagraph("");
                doc.InsertParagraph("");

                doc.InsertParagraph("Supdt. Post Offices,").Alignment = Alignment.right;
                doc.InsertParagraph("Rourkela Division,").Alignment = Alignment.right;
                doc.InsertParagraph("Rourkela-769011").Alignment = Alignment.right;
                doc.InsertParagraph("");
                doc.InsertParagraph("");
                doc.InsertSectionPageBreak();
                showNotification("Alert !", "Current page saved !", Properties.Resources.checked_checkbox_48);
                metroTextBox1.Select();

            }
        }

        private void bunifuCustomDataGrid1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == bunifuCustomDataGrid1.Columns["amount"].Index) //this is our numeric column
            {
                int i;
                if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
                {
                    e.Cancel = true;
                    showNotification("Alert !", "Invalid amount in grid !", Properties.Resources.error_48);
                }
            }
        }
    }
}
