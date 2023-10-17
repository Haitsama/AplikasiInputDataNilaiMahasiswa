using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AplikasiInputDataMahasiswa
{
    public partial class Form1 : Form
    {
        private List<Mahasiswa> list = new List<Mahasiswa>();
        public Form1()
        {
            InitializeComponent();
            InisialisasiListView();
        }
        public class Mahasiswa
        {
            public string Kode;
            public string Matakuliah;
            public string Kelas;
            public int SKS;
            public int Nilai;
            public string Huruf;
            public int Bobot;
        }

        // atur kolom listview
        private void InisialisasiListView()
        {
            lvwMahasiswa.View = View.Details;
            lvwMahasiswa.FullRowSelect = true;
            lvwMahasiswa.GridLines = true;
            lvwMahasiswa.Columns.Add("No.", 30, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Kode", 91, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Matakuliah", 100, HorizontalAlignment.Left);
            lvwMahasiswa.Columns.Add("Kelas", 70, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("SKS", 50, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Nilai", 50, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Nilai Huruf", 80, HorizontalAlignment.Center);
            lvwMahasiswa.Columns.Add("Bobot nilai", 80, HorizontalAlignment.Center);
        }
        private void ResetForm()
        {
            txtKode.Clear();
            txtMatakuliah.Clear();
            txtKelas.Clear();
            txtSKS.Clear();
            txtNilai.Text = "0";
            txtKode.Focus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private bool NumericOnly(KeyPressEventArgs e)
        {
            var strValid = "0123456789";
            if (!(e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                // inputan selain angka
                if (strValid.IndexOf(e.KeyChar) < 0)
                {
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        private void txtNilai_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = NumericOnly(e);
        }

        private void btnTampilkanData_Click(object sender, EventArgs e)
        {
            TampilkanData();
        }


        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // membuat objek mahasiswa 
            Mahasiswa mhs = new Mahasiswa();
            // set nilai masing-masing propertynya
            // berdasarkan inputan yang ada di form
            mhs.Kode = txtKode.Text;
            mhs.Matakuliah = txtMatakuliah.Text;
            mhs.Kelas = txtKelas.Text;
            mhs.SKS = int.Parse(txtSKS.Text);
            mhs.Nilai = int.Parse(txtNilai.Text);

            if (mhs.Nilai <= 20 )
            {
                mhs.Huruf = "F";
            }else if (mhs.Nilai <= 40)
            {
                mhs.Huruf = "D";
            }
            else if (mhs.Nilai <= 60)
            {
                mhs.Huruf = "C";
            }
            else if (mhs.Nilai <= 80)
            {
                mhs.Huruf = "B";
            }
            else if (mhs.Nilai <= 100)
            {
                mhs.Huruf = "A";
            }
            else if (mhs.Nilai > 100)
            {
                mhs.Huruf = "agaklain";
            }

            switch (mhs.Huruf)
            {
                case "A":
                    mhs.Bobot = 4;
                    break;
                case "B":
                    mhs.Bobot = 3;
                    break;
                case "C":
                    mhs.Bobot = 2;
                    break;
                case "D":
                    mhs.Bobot = 1;
                    break;
                default:
                    mhs.Bobot = 0; // Jika nilai huruf tidak valid, beri bobot 0.
                    break;
            }


            // tambahkan objek mahasiswa ke dalam collection
            list.Add(mhs);
            var msg = "Data mahasiswa berhasil disimpan.";
            // tampilkan dialog informasi
            MessageBox.Show(msg, "Informasi", MessageBoxButtons.OK,
            MessageBoxIcon.Information);
            // reset form input
            ResetForm();
        }
        private void TampilkanData()
        {
            // kosongkan data listview
            lvwMahasiswa.Items.Clear();
            // lakukan perulangan untuk menampilkan data mahasiswa ke listview
            foreach (var mhs in list)
            {
                var noUrut = lvwMahasiswa.Items.Count + 1;
                var item = new ListViewItem(noUrut.ToString());
                item.SubItems.Add(mhs.Kode);
                item.SubItems.Add(mhs.Matakuliah);
                item.SubItems.Add(mhs.Kelas);
                item.SubItems.Add(mhs.SKS.ToString());
                item.SubItems.Add(mhs.Nilai.ToString());
                item.SubItems.Add(mhs.Huruf);
                item.SubItems.Add(mhs.Bobot.ToString()); 
                lvwMahasiswa.Items.Add(item);
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            
                // cek apakah data mahasiswa sudah dipilih
                if (lvwMahasiswa.SelectedItems.Count > 0)
                {
                    // tampilkan konfirmasi
                    var konfirmasi = MessageBox.Show("Apakah data mahasiswa ingin dihapus ? ", "Konfirmasi",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (konfirmasi == DialogResult.Yes)
                    {
                        // ambil index list yang di pilih
                        var index = lvwMahasiswa.SelectedIndices[0];
                        // hapus objek mahasiswa dari list
                        list.RemoveAt(index);
                        // refresh tampilan listivew
                        TampilkanData();
                    }
                }
                else // data belum dipilih
                {
                    MessageBox.Show("Data mahasiswa belum dipilih !!!", "Peringatan",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            
        }

        private void btnHitungIPK_Click(object sender, EventArgs e)
        {
            if (list.Count == 0)
            {
                MessageBox.Show("Belum ada data mahasiswa untuk menghitung IPK.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double totalBobotSKS = 0;
            int totalSKS = 0;

            foreach (var mhs in list)
            {
                totalBobotSKS += mhs.Bobot * mhs.SKS;
                totalSKS += mhs.SKS;
            }

            if (totalSKS == 0)
            {
                MessageBox.Show("Total SKS tidak boleh nol.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double ipk = totalBobotSKS / totalSKS;

            MessageBox.Show("IPK Mahasiswa: " + ipk.ToString("0.00"), "Hasil Perhitungan IPK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
