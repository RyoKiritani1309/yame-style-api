import { Hero } from "@/components/Hero";
import { ProductCard } from "@/components/ProductCard";
import { mockProducts } from "@/data/mockProducts";
import { Button } from "@/components/ui/button";
import { Link } from "react-router-dom";
import { ArrowRight } from "lucide-react";

const Home = () => {
  const featuredProducts = mockProducts.slice(0, 4);

  return (
    <div className="min-h-screen">
      <Hero />

      {/* Featured Products Section */}
      <section className="container mx-auto px-4 py-16">
        <div className="mb-8 flex items-center justify-between">
          <div>
            <h2 className="text-3xl font-bold text-foreground">Sản phẩm nổi bật</h2>
            <p className="mt-2 text-muted-foreground">
              Khám phá những sản phẩm được yêu thích nhất
            </p>
          </div>
          <Button variant="outline" className="group" asChild>
            <Link to="/products">
              Xem tất cả
              <ArrowRight className="ml-2 h-4 w-4 transition-smooth group-hover:translate-x-1" />
            </Link>
          </Button>
        </div>

        <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
          {featuredProducts.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      </section>

      {/* Categories Section */}
      <section className="bg-muted/30 py-16">
        <div className="container mx-auto px-4">
          <div className="mb-8 text-center">
            <h2 className="text-3xl font-bold text-foreground">Danh mục sản phẩm</h2>
            <p className="mt-2 text-muted-foreground">
              Tìm kiếm theo danh mục yêu thích của bạn
            </p>
          </div>

          <div className="grid gap-6 md:grid-cols-3">
            <Link
              to="/products?category=ao"
              className="group relative overflow-hidden rounded-lg bg-card p-8 shadow-product transition-smooth hover:shadow-product-hover"
            >
              <div className="relative z-10">
                <h3 className="text-2xl font-bold text-foreground group-hover:text-accent transition-smooth">
                  Áo
                </h3>
                <p className="mt-2 text-sm text-muted-foreground">
                  Áo thun, hoodie, áo khoác và nhiều hơn nữa
                </p>
              </div>
              <div className="absolute bottom-0 right-0 h-32 w-32 rounded-full bg-accent/10 transition-smooth group-hover:scale-150" />
            </Link>

            <Link
              to="/products?category=quan"
              className="group relative overflow-hidden rounded-lg bg-card p-8 shadow-product transition-smooth hover:shadow-product-hover"
            >
              <div className="relative z-10">
                <h3 className="text-2xl font-bold text-foreground group-hover:text-accent transition-smooth">
                  Quần
                </h3>
                <p className="mt-2 text-sm text-muted-foreground">
                  Quần jean, kaki, short và nhiều kiểu dáng
                </p>
              </div>
              <div className="absolute bottom-0 right-0 h-32 w-32 rounded-full bg-accent/10 transition-smooth group-hover:scale-150" />
            </Link>

            <Link
              to="/products?category=giay"
              className="group relative overflow-hidden rounded-lg bg-card p-8 shadow-product transition-smooth hover:shadow-product-hover"
            >
              <div className="relative z-10">
                <h3 className="text-2xl font-bold text-foreground group-hover:text-accent transition-smooth">
                  Giày dép
                </h3>
                <p className="mt-2 text-sm text-muted-foreground">
                  Sneaker, sandal và phụ kiện thời trang
                </p>
              </div>
              <div className="absolute bottom-0 right-0 h-32 w-32 rounded-full bg-accent/10 transition-smooth group-hover:scale-150" />
            </Link>
          </div>
        </div>
      </section>

      {/* Why Choose Us Section */}
      <section className="container mx-auto px-4 py-16">
        <div className="mb-8 text-center">
          <h2 className="text-3xl font-bold text-foreground">Tại sao chọn YAME?</h2>
          <p className="mt-2 text-muted-foreground">
            Cam kết mang đến trải nghiệm mua sắm tốt nhất
          </p>
        </div>

        <div className="grid gap-8 md:grid-cols-3">
          <div className="text-center">
            <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-accent/10">
              <svg className="h-8 w-8 text-accent" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
            </div>
            <h3 className="mb-2 text-xl font-semibold text-foreground">Chất lượng cao cấp</h3>
            <p className="text-muted-foreground">
              Sản phẩm được làm từ chất liệu cao cấp, bền đẹp theo thời gian
            </p>
          </div>

          <div className="text-center">
            <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-accent/10">
              <svg className="h-8 w-8 text-accent" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <h3 className="mb-2 text-xl font-semibold text-foreground">Giao hàng nhanh</h3>
            <p className="text-muted-foreground">
              Miễn phí vận chuyển cho đơn hàng trên 500.000đ toàn quốc
            </p>
          </div>

          <div className="text-center">
            <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-accent/10">
              <svg className="h-8 w-8 text-accent" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
            </div>
            <h3 className="mb-2 text-xl font-semibold text-foreground">Đổi trả dễ dàng</h3>
            <p className="text-muted-foreground">
              Chính sách đổi trả trong vòng 30 ngày, hoàn tiền 100%
            </p>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Home;
