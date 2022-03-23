using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tuan4_LeTrungKien.Models;

namespace Tuan4_LeTrungKien.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        DataClasses1DataContext db = new DataClasses1DataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();
                Session["GioHang"] = listGiohang;
            }
            return listGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)

        {
            List<GioHang> listGioHang = Laygiohang();
            GioHang sanpham = listGioHang.Find(n => n.masach == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                listGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }

        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tsl = listGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang != null)
            {
                tt = listGiohang.Sum(n => n.dThanhTien);
            }
            return tt;
        }
        public ActionResult GioHang()
        {
            List<GioHang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(listGioHang);

        }
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> listGioHang = Laygiohang();
            GioHang sanpham = listGioHang.SingleOrDefault(n => n.masach == id);
            if( sanpham !=null)
            {
                listGioHang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int id,FormCollection collection)
        {

            List<GioHang> listGioHang = Laygiohang();
            GioHang sanpham = listGioHang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                Sach sach = db.Saches.FirstOrDefault(n => n.masach == id);
                int solg = int.Parse(collection["txtSoLg"].ToString());
                if(solg > sach.soluongton)
                {
                    Session["Messenge"] = "Khong du so luong";
                    Session["AlertStatus"] = "danger";
                    return RedirectToAction("GioHang");
                }
                sanpham.iSoluong = solg;
            }
            
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> ListGioHang = Laygiohang();
            ListGioHang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap","NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> listGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();

            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy }", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            db.DonHangs.InsertOnSubmit(dh);
            db.SubmitChanges();
            foreach(var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.iSoluong;
                ctdh.gia =(decimal)item.giaban;
                s = db.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                db.SubmitChanges();
                db.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");

        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
            
    }

}