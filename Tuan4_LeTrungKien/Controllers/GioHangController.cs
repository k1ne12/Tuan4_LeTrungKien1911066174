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
        NhaSachDataContext db = new NhaSachDataContext();
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
            
    }

}