using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CashRegister
{
    public partial class Form1 : Form
    {
        int count = 0;
        private Dictionary<string, double> totalCount = new Dictionary<string, double>();
        List<Product> products = new List<Product>() {
            new Product("Select",0,0),
            new Product("Apple",100,0),
            new Product("Banana",30,10),
            new Product("Grapes",70,20),
            new Product("Pinaple",60,0),
            new Product("Almonds",300,10),
            };
        public Form1()
        {
            
            InitializeComponent();
         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (count < 4)
            {
                ComboBox combo = new ComboBox();
                ListtoDataTable lsttodt = new ListtoDataTable();
                DataTable dt = lsttodt.ToDataTable(products);

                combo.DataSource = new DataView(dt);
                combo.Name = "item" + count;
                combo.DisplayMember = "Name";
                combo.Location = new Point(50, 20 * (5 + count * 5));
                combo.SelectedIndexChanged+= new System.EventHandler(cmb_SelectedValueChanged);
                this.Controls.Add(combo);
            }
            count++;
        }

        private void cmb_SelectedValueChanged(object sender, EventArgs e)
        {
            var nameOfPro = ((System.Windows.Forms.Control)sender).Text;

            var cost = from x in products
                       where x.Name == ((System.Windows.Forms.Control)sender).Text
                       select x.Price;
            var discount= from x in products
                          where x.Name == ((System.Windows.Forms.Control)sender).Text
                          select x.Discount;
            var labelToRemove = this.Controls["cost" + ((System.Windows.Forms.Control)sender).Name];
            this.Controls.Remove(labelToRemove);
            var labelToRemove1 = this.Controls["final" + ((System.Windows.Forms.Control)sender).Name];
            this.Controls.Remove(labelToRemove1);
            var labelToRemove2 = this.Controls["itemsize" + ((System.Windows.Forms.Control)sender).Name];
            this.Controls.Remove(labelToRemove2);

            if (nameOfPro != "Select")
            {
                Label lb = new Label();
                var price = Convert.ToInt32(cost.First()) - Convert.ToDouble(cost.First()) * (Convert.ToDouble(discount.First()) / 100);
                //lb.rText = "";
                lb.Text = "Product Cost:" + cost.First() + " Discount Price:" + discount.First() + "%";
                lb.Name = "cost" + ((System.Windows.Forms.Control)sender).Name;
                lb.Size = new System.Drawing.Size(180, 23);
                lb.Location = new Point(((System.Windows.Forms.Control)sender).Location.X + 150, ((System.Windows.Forms.Control)sender).Location.Y);
                this.Controls.Add(lb);

                Label lbl = new Label();
                //lb.Text = "";
                lbl.Text = "Quantity in Kg's:";
                lbl.Name = "cost" + ((System.Windows.Forms.Control)sender).Name;
                lbl.Size = new System.Drawing.Size(90, 23);
                lbl.Location = new Point(((System.Windows.Forms.Control)sender).Location.X, ((System.Windows.Forms.Control)sender).Location.Y + 35);
                this.Controls.Add(lbl);
                TextBox itemSize = new TextBox();
                itemSize.Text = "1";
                itemSize.Name = "itemsize" + ((System.Windows.Forms.Control)sender).Name;
                itemSize.Size = new System.Drawing.Size(100, 23);
                itemSize.Location = new Point(((System.Windows.Forms.Control)sender).Location.X + 90, ((System.Windows.Forms.Control)sender).Location.Y + 30);

                //itemSize.TextChanged += new System.EventHandler(item_TextChanged);
                itemSize.TextChanged += delegate (object senders, EventArgs es) { item_TextChanged(senders, es, price, "final" + ((System.Windows.Forms.Control)sender).Name); };

                this.Controls.Add(itemSize);
                Label fprice = new Label();
                //lb.Text = "";
                fprice.Text = "Final Price:" + price;
                fprice.Name = "final" + ((System.Windows.Forms.Control)sender).Name;
                fprice.Size = new System.Drawing.Size(90, 23);
                fprice.Location = new Point(((System.Windows.Forms.Control)sender).Location.X, ((System.Windows.Forms.Control)sender).Location.Y + 65);
                this.Controls.Add(fprice);
                totalCount["final" + ((System.Windows.Forms.Control)sender).Name] = price;
            }
            //totalCount = totalCount + price;
        }

        private void item_TextChanged(object senders, EventArgs e, double price, string v)
        {
            if (((System.Windows.Forms.TextBox)senders).Text != "")
            {
          //      var labelToRemove = this.Controls[v];
            //    this.Controls.Remove(labelToRemove);
                var cost = Convert.ToInt32(((System.Windows.Forms.TextBox)senders).Text) * price;
                this.Controls[v].Text = "Final Price:"+Convert.ToString(cost);
                totalCount[v] = cost;
                //totalCount = totalCount + Convert.ToInt32(((System.Windows.Forms.TextBox)senders).Text) * price;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double sum = 0;

            foreach(var item in totalCount)
            {
                sum = sum + item.Value;
            }
            label2.Text = "Total Cost to Pay:" + sum;

        }
    }
    public class ListtoDataTable
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
    class Product
    {
        public string Name
        {
            get;
            set;
        }
        public float Price
        {
            get;
            set;
        }
        public float Discount
        {
            get;
            set;
        }
        public Product(string  name, float price, float discount)
        {
            Name = name;
            Price = price;
            Discount = discount;
        }
    }
}
