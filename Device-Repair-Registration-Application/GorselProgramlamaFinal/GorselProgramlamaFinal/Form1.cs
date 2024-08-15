using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GorselProgramlamaFinal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string baglanma = "Data Source=DESKTOP-BAUKP8V;Initial Catalog=GorselProgramlama;Integrated Security=True";
        readonly SqlConnection connect = new SqlConnection(baglanma);

        void Temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            maskedTextBox1.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;


            textBox1.Focus();
        }



        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label8.Text = "True";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label8.Text = "false";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string baglanti;

            try
            {
                if (connect.State == ConnectionState.Closed)
                    connect.Open();
                baglanti = "SET IDENTITY_INSERT FinalProjesi ON insert into FinalProjesi (Musteri_No,Ad,Soyad,Sehir,Cihaz,Telefon_No,Garanti,Borc,Odenen,Kalan_Odeme) values (@Musteri_No,@Ad,@Soyad,@Sehir,@Cihaz,@Telefon_No,@Garanti,@Borc,@Odenen,@Kalan_Odeme) SET IDENTITY_INSERT FinalProjesi OFF";
                SqlCommand dagıtma = new SqlCommand(baglanti, connect);

                try
                {
                    // TextBox'lardan girilen değerleri al
                    double number1 = double.Parse(textBox4.Text);
                    double number2 = double.Parse(textBox5.Text);

                    // Çıkarma işlemini gerçekleştir
                    double result = number1 - number2;

                    // Sonucu Label'a yazdır
                    label10.Text = "Kalan: " + result.ToString();
                }
                catch (FormatException)
                {
                    // Kullanıcı hatalı giriş yaptıysa bir hata mesajı göster
                    MessageBox.Show("Lütfen geçerli sayılar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



                dagıtma.Parameters.AddWithValue("@Musteri_No", textBox1.Text);
                dagıtma.Parameters.AddWithValue("@Ad", textBox2.Text);
                dagıtma.Parameters.AddWithValue("@Soyad", textBox3.Text);
                dagıtma.Parameters.AddWithValue("@Sehir", comboBox1.Text);
                dagıtma.Parameters.AddWithValue("@Cihaz", comboBox2.Text);
                dagıtma.Parameters.AddWithValue("@Telefon_No", maskedTextBox1.Text);
                dagıtma.Parameters.AddWithValue("@Garanti", label8.Text);
                dagıtma.Parameters.AddWithValue("@Borc", textBox4.Text);
                dagıtma.Parameters.AddWithValue("@Odenen", textBox5.Text);
                dagıtma.Parameters.AddWithValue("@Kalan_Odeme", label10.Text);

                dagıtma.ExecuteNonQuery();

                MessageBox.Show("Kayıt Oluşturulmuştur.");
            }
            catch (Exception hata) //Rezervasyon oluşturma sırasında hata meydana gelirse bu kod ile karşılaşılıcak.
            {
                MessageBox.Show("Kayıt Oluşturulurken Hata Meydana Geldi" + hata.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM FinalProjesi", connect))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter veri = new SqlDataAdapter(cmd))
                    {
                        using (DataTable tablo = new DataTable())
                        {
                            veri.Fill(tablo);
                            dataGridView1.DataSource = tablo;
                        }
                    }
                }

                connect.Close();
            }
            catch (Exception hata)        // Eğer tabloya kayıt aşamasında bir hata olursa bu hod ile karşılaşılıcak
            {
                MessageBox.Show("Görüntüleme Yapılırken Meydana Geldi" + hata.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try                           //Seçilmiş olan rezervasyonu tablodan ve sql den siler.

            {
                if (connect.State == ConnectionState.Closed)
                    connect.Open();
                SqlCommand silgi = new SqlCommand("Delete From FinalProjesi where Musteri_No = '" + dataGridView1.SelectedCells[0].Value + "'", connect);
                SqlDataReader sil = silgi.ExecuteReader();
                sil.Read();

                MessageBox.Show("İstediğiniz Kayıt Başarıyla Silinmiştir");   //başarılı bir şekilde seçilen kayıt silinirse gelen bildirim 

                while (sil.Read())
                {

                }
                connect.Close();

                if (connect.State == ConnectionState.Closed) //Sildikten sonra tabloda direk gözükmesi için gereken kod. tabloyu yeniliyor gibi.
                    connect.Open();

                using (silgi = new SqlCommand("SELECT * From FinalProjesi", connect))
                {
                    silgi.CommandType = CommandType.Text;
                    using (SqlDataAdapter veri = new SqlDataAdapter(silgi))
                    {
                        using (DataTable tablo = new DataTable())
                        {
                            veri.Fill(tablo);
                            dataGridView1.DataSource = tablo;
                        }
                    }
                }

                connect.Close();
            }
            catch (Exception hata)          //Kayıt silinemezse alınacak hata kodu.
            {
                MessageBox.Show("Seçtiğiniz rezervasyon silinirken sorun oluştu lütfen baştan deneyiniz" + hata.Message);

            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == 8)         // yazma kısmına rakam harici bir yazı yazılmasını engelliyor.
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == 8)         // yazma kısmına rakam harici bir yazı yazılmasını engelliyor.
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

            textBox4.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*string guncelleme = "UPDATE FinalProjesi SET Museteri_No=@Musteri_No,Ad=@Ad,Soyad=@Soyad,Sehir=@Sehir,Cihaz=@Cihaz,Telefon_No=@Telefon_No,Garanti=@Garanti,Borc=@Borc,Odenen=@Odenen,Kalan=@Kalan WHERE Musteri_No=@Musteri_No";
            SqlCommand dagıtma = new SqlCommand(guncelleme, connect);
            //dagıtma.Parameters.AddWithValue("@Musteri_No", Convert.ToInt32(textBox1.Text));
            dagıtma.Parameters.AddWithValue("@Ad", Convert.ToInt32(textBox2.Text));
            dagıtma.Parameters.AddWithValue("@Soyad", Convert.ToInt32(textBox3.Text));                                 bu kod bölümü hata aldığı için başka bişi denedim
            dagıtma.Parameters.AddWithValue("@Sehir", Convert.ToInt32(comboBox1.Text));
            dagıtma.Parameters.AddWithValue("@Cihaz", Convert.ToInt32(comboBox2.Text));
            dagıtma.Parameters.AddWithValue("@Telefon_No", Convert.ToInt32(maskedTextBox1.Text));
            dagıtma.Parameters.AddWithValue("@Garanti", Convert.ToInt32(label8.Text));
            dagıtma.Parameters.AddWithValue("@Borc", Convert.ToInt32(textBox4.Text));
            dagıtma.Parameters.AddWithValue("@Odenen", Convert.ToInt32(textBox5.Text));
            dagıtma.Parameters.AddWithValue("@Kalan", Convert.ToInt32(label10.Text));
            connect.Open();
            dagıtma.ExecuteNonQuery();
            connect.Close();
            */
            // Güncellenecek kaydın UserID'sini alın


            int Musteri_No = int.Parse(textBox1.Text); // TextBox'tan UserID alınmalı

            // Güncellenmiş bilgileri TextBox'lardan alın
            string Ad = textBox2.Text;
            string Soyad = textBox3.Text;
            string Sehir = comboBox1.Text;
            string Cihaz = comboBox2.Text;
            string Telefon_No = maskedTextBox1.Text;
            string Garanti = label8.Text;
            string Borc = textBox4.Text;
            string Odenen = textBox5.Text;

            try
            {
                // TextBox'lardan girilen değerleri al
                double number1 = double.Parse(textBox4.Text);
                double number2 = double.Parse(textBox5.Text);

                // Çıkarma işlemini gerçekleştir
                double result = number1 - number2;

                // Sonucu Label'a yazdır
                label10.Text = "Kalan: " + result.ToString();
            }
            catch (FormatException)
            {
                // Kullanıcı hatalı giriş yaptıysa bir hata mesajı göster
                MessageBox.Show("Lütfen geçerli sayılar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            string Kalan_Odeme = label10.Text;


            // Güncelleme sorgusu
            string query = "UPDATE FinalProjesi SET Ad = @Ad, Soyad = @Soyad, Sehir = @Sehir, Cihaz = @Cihaz, Telefon_no = @Telefon_No, Garanti = @Garanti, Borc = @Borc, Odenen = @Odenen, Kalan_Odeme = @Kalan_Odeme WHERE Musteri_No = @Musteri_No";

            


            using (SqlConnection connection = new SqlConnection(baglanma))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Musteri_No", Musteri_No);
                command.Parameters.AddWithValue("@Ad", Ad);
                command.Parameters.AddWithValue("@Soyad", Soyad);
                command.Parameters.AddWithValue("@Sehir", Sehir);
                command.Parameters.AddWithValue("@Cihaz", Cihaz);
                command.Parameters.AddWithValue("@Telefon_No", Telefon_No);
                command.Parameters.AddWithValue("@Garanti", Garanti);
                command.Parameters.AddWithValue("@Borc", Borc);
                command.Parameters.AddWithValue("@Odenen", Odenen);
                command.Parameters.AddWithValue("@Kalan_Odeme", Kalan_Odeme);

                

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kayıt başarıyla güncellendi.");
                    }
                    else
                    {
                        MessageBox.Show("Güncellenecek kayıt bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
