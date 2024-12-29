using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Btap8.DAL;

namespace BapTap8.BUS
{
    public class SanPhamService
    {
         ProductContextDB model = new ProductContextDB();
        public List<Sanpham> GetALL()
        {

            return model.Sanpham.ToList();
        }
        public Sanpham FindByID(string id)
        {
            return model.Sanpham.FirstOrDefault(p => p.MaSP == id);
        }

        public Sanpham FindByName(string name)
        {
            var product = model.Sanpham.FirstOrDefault(p => p.TenSP.ToLower() == name.ToLower());
            if (product == null)
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với tên: {name}");
            return product;
        }


        public void Remove(Sanpham sanpham)
        {
            using (var context = new ProductContextDB())
            {
                var studentToRemove = context.Sanpham.FirstOrDefault(s => s.MaSP == sanpham.MaSP);
                if (studentToRemove != null)
                {
                    context.Sanpham.Remove(studentToRemove);
                    context.SaveChanges();
                }
            }
        }
        public void InsertUpdate(Sanpham sanpham)
        {
            using (var context = new ProductContextDB())
            {
                context.Sanpham.AddOrUpdate(sanpham);
                context.SaveChanges();
            }
        }

    }
}
