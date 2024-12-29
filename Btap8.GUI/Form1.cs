using BapTap8.BUS;
using Btap8.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Btap8.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private readonly LoaiSPService loaiSPService = new LoaiSPService();
        private readonly SanPhamService sanPhamService = new SanPhamService();
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                btLuu.Enabled = false;
                btKhongLuu.Enabled = false;
                List<Sanpham> listSanPham = sanPhamService.GetALL();
                List<LoaiSP> listLoaiSP = loaiSPService.GetAll();
                FillLoaiSPComboBox(listLoaiSP);

                BindGrid(listSanPham);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FillLoaiSPComboBox(List<LoaiSP> listLoaiSP)
        {
            this.cmbLoaiSP.DataSource = listLoaiSP;
            this.cmbLoaiSP.DisplayMember = "TenLoai";
            this.cmbLoaiSP.ValueMember = "MaLoai";
        }

        private void BindGrid(List<Sanpham> listSanPham)
        {
            dgvSanPham.Rows.Clear();
            foreach (var item in listSanPham)
            {
                int index = dgvSanPham.Rows.Add();
                dgvSanPham.Rows[index].Cells[0].Value = item.MaSP;
                dgvSanPham.Rows[index].Cells[1].Value = item.TenSP;
                dgvSanPham.Rows[index].Cells[2].Value = item.Ngaynhap;
                dgvSanPham.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai;
            }
        }
        private int GetIndexSelected(string id)
        {
            for (int i = 0; i < dgvSanPham.Rows.Count; i++)
            {
                var cellValue = dgvSanPham.Rows[i].Cells[0].Value;
                if (cellValue != null && cellValue.ToString() == id)
                {
                    return i;
                }
            }
            return -1;
        }
        private void btThem_Click(object sender, EventArgs e)
        {

            try
            {
                // Kiểm tra xem mã sản phẩm đã tồn tại chưa
                var existingProduct = sanPhamService.FindByID(txtMaSP.Text);
                if (existingProduct == null)
                {
                    Sanpham sp = new Sanpham
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        MaLoai = cmbLoaiSP.SelectedValue.ToString(),
                        Ngaynhap = dateTimePicker1.Value
                    };
                    if (dgvSanPham != null)
                    {
                        int newIndex = dgvSanPham.Rows.Add();
                        dgvSanPham.Rows[newIndex].Cells[0].Value = sp.MaSP;
                        dgvSanPham.Rows[newIndex].Cells[1].Value = sp.TenSP;
                        dgvSanPham.Rows[newIndex].Cells[2].Value = sp.Ngaynhap;
                        dgvSanPham.Rows[newIndex].Cells[3].Value = cmbLoaiSP.Text; 

                        StatusEnable(true); 
                        MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("DataGridView chưa được khởi tạo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm sản phẩm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {

            try
            {
                int index = GetIndexSelected(txtMaSP.Text);
                if (index != -1)
                {
                    dgvSanPham.Rows[index].Cells[1].Value = txtTenSP.Text;
                    dgvSanPham.Rows[index].Cells[2].Value = dateTimePicker1.Value;
                    dgvSanPham.Rows[index].Cells[3].Value = cmbLoaiSP.Text;

                    StatusEnable(true);

                    MessageBox.Show("Sửa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi sửa sản phẩm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StatusEnable(bool status)
        {
            btLuu.Enabled = status;
            btKhongLuu.Enabled = status;
        }
        private List<string> deletedProductIds = new List<string>();
        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                int index = GetIndexSelected(txtMaSP.Text);
                if (index != -1)
                {
                    deletedProductIds.Add(dgvSanPham.Rows[index].Cells[0].Value.ToString());

                    dgvSanPham.Rows.RemoveAt(index);
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    StatusEnable(true); 
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string productId in deletedProductIds)
                {
                    var productToDelete = sanPhamService.FindByID(productId);
                    if (productToDelete != null)
                    {
                        sanPhamService.Remove(productToDelete);
                    }
                }

                deletedProductIds.Clear();

                foreach (DataGridViewRow row in dgvSanPham.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        Sanpham sp = new Sanpham
                        {
                            MaSP = row.Cells[0].Value.ToString(),
                            TenSP = row.Cells[1].Value.ToString(),
                            Ngaynhap = DateTime.Parse(row.Cells[2].Value.ToString()),
                            MaLoai = loaiSPService.GetByName(row.Cells[3].Value.ToString())?.MaLoai
                        };

                        sanPhamService.InsertUpdate(sp);
                    }
                }

                MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StatusEnable(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btKhongLuu_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid(sanPhamService.GetALL());
                StatusEnable(false);
                MessageBox.Show("Thay đổi đã được hủy bỏ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hủy bỏ thay đổi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvSanPham.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    DataGridViewRow selectedRow = dgvSanPham.Rows[e.RowIndex];

                    txtMaSP.Text = selectedRow.Cells[0].Value.ToString();
                    txtTenSP.Text = selectedRow.Cells[1].Value.ToString();
                    dateTimePicker1.Value = DateTime.Parse(selectedRow.Cells[2].Value.ToString());
                    cmbLoaiSP.Text = selectedRow.Cells[3].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chọn dòng: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btTimTheoTen_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtTim.Text))
                {
                    string search = txtTim.Text.ToLower();
                    List<Sanpham> filteredList = sanPhamService.GetALL().FindAll(sp => sp.TenSP.ToLower().Contains(search));

                    if (filteredList.Count > 0)
                    {
                        BindGrid(filteredList);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    BindGrid(sanPhamService.GetALL());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTim_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTim.Text))
            {
                BindGrid(sanPhamService.GetALL());
            }
        }
    }
}
