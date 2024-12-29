using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btap8.DAL;
namespace BapTap8.BUS
{
    public class LoaiSPService
    {

        ProductContextDB model = new ProductContextDB();
        public List<LoaiSP> GetAll()
        {
            return model.LoaiSP.ToList();
        }
        public LoaiSP GetByName(string tenLoai)
        {
            return model.LoaiSP.FirstOrDefault(l => l.TenLoai == tenLoai);
        }
    }
}

