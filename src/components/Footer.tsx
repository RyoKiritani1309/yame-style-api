import { Link } from "react-router-dom";
import { Facebook, Instagram, Youtube } from "lucide-react";

export const Footer = () => {
  return (
    <footer className="border-t border-border bg-muted/30">
      <div className="container mx-auto px-4 py-12">
        <div className="grid gap-8 md:grid-cols-2 lg:grid-cols-4">
          {/* Brand */}
          <div className="space-y-4">
            <h3 className="text-2xl font-bold text-foreground">YAME</h3>
            <p className="text-sm text-muted-foreground">
              Thời trang Việt Nam hiện đại, chất lượng cao cấp với thiết kế độc đáo và phong cách trẻ trung.
            </p>
            <div className="flex gap-4">
              <Link
                to="#"
                className="text-muted-foreground hover:text-accent transition-smooth"
              >
                <Facebook className="h-5 w-5" />
              </Link>
              <Link
                to="#"
                className="text-muted-foreground hover:text-accent transition-smooth"
              >
                <Instagram className="h-5 w-5" />
              </Link>
              <Link
                to="#"
                className="text-muted-foreground hover:text-accent transition-smooth"
              >
                <Youtube className="h-5 w-5" />
              </Link>
            </div>
          </div>

          {/* Products */}
          <div className="space-y-4">
            <h4 className="font-semibold text-foreground">Sản phẩm</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link to="/products" className="text-muted-foreground hover:text-accent transition-smooth">
                  Áo thun
                </Link>
              </li>
              <li>
                <Link to="/products" className="text-muted-foreground hover:text-accent transition-smooth">
                  Áo hoodie
                </Link>
              </li>
              <li>
                <Link to="/products" className="text-muted-foreground hover:text-accent transition-smooth">
                  Áo khoác
                </Link>
              </li>
              <li>
                <Link to="/products" className="text-muted-foreground hover:text-accent transition-smooth">
                  Giày dép
                </Link>
              </li>
            </ul>
          </div>

          {/* Support */}
          <div className="space-y-4">
            <h4 className="font-semibold text-foreground">Hỗ trợ</h4>
            <ul className="space-y-2 text-sm">
              <li>
                <Link to="/about" className="text-muted-foreground hover:text-accent transition-smooth">
                  Về chúng tôi
                </Link>
              </li>
              <li>
                <Link to="/contact" className="text-muted-foreground hover:text-accent transition-smooth">
                  Liên hệ
                </Link>
              </li>
              <li>
                <Link to="/shipping" className="text-muted-foreground hover:text-accent transition-smooth">
                  Chính sách vận chuyển
                </Link>
              </li>
              <li>
                <Link to="/returns" className="text-muted-foreground hover:text-accent transition-smooth">
                  Đổi trả hàng
                </Link>
              </li>
            </ul>
          </div>

          {/* Contact */}
          <div className="space-y-4">
            <h4 className="font-semibold text-foreground">Liên hệ</h4>
            <ul className="space-y-2 text-sm text-muted-foreground">
              <li>Hotline: 1900 xxxx</li>
              <li>Email: contact@yame.vn</li>
              <li>Địa chỉ: Hà Nội, Việt Nam</li>
              <li>Giờ làm việc: 8:00 - 22:00</li>
            </ul>
          </div>
        </div>

        <div className="mt-8 border-t border-border pt-8 text-center text-sm text-muted-foreground">
          <p>&copy; 2025 YAME Vietnam. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
};
