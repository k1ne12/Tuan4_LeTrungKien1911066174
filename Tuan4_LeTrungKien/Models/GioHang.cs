using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tuan4_LeTrungKien.Models
{
    public class GioHang
    {
        NhaSachDataContext data = new NhaSachDataContext();
        public int masach { get; set; }

        [Display(Name ="Ten sach")]
        public string tensach { get; set; }

        [Display(Name = "Anh Bia")]
        public string hinh { get; set; }

        [Display(Name = "Gia ban")]
        public double giaban{ get; set; }

        [Display(Name = "So luong")]
        public int iSoluong { get; set; }

        [Display(Name = "Thanh Tien")]
        public Double dThanhTien
        {
            get { return iSoluong * giaban; }
        }
        public GioHang(int id)
        {
            masach = id;
            Sach sach = data.Saches.Single(n => n.masach == masach);
            tensach = sach.tensach;
            hinh = sach.hinh;
            giaban = double.Parse(sach.giaban.ToString());
            iSoluong = 1;
        }
    }
}