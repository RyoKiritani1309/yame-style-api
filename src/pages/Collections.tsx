import { Navbar } from "@/components/Navbar";
import { Footer } from "@/components/Footer";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { ArrowRight } from "lucide-react";
import { Link } from "react-router-dom";

const Collections = () => {
  const collections = [
    {
      id: 1,
      title: "Bộ sưu tập Mùa Xuân",
      description: "Tươi mới, năng động với những gam màu pastel nhẹ nhàng",
      imageUrl: "https://images.unsplash.com/photo-1490481651871-ab68de25d43d?w=800&q=80",
      productCount: 24
    },
    {
      id: 2,
      title: "Bộ sưu tập Mùa Hè",
      description: "Thoáng mát, trẻ trung cho những ngày hè rực rỡ",
      imageUrl: "https://images.unsplash.com/photo-1523381210434-271e8be1f52b?w=800&q=80",
      productCount: 32
    },
    {
      id: 3,
      title: "Bộ sưu tập Thu Đông",
      description: "Ấm áp, sang trọng với thiết kế tinh tế",
      imageUrl: "https://images.unsplash.com/photo-1483985988355-763728e1935b?w=800&q=80",
      productCount: 28
    },
    {
      id: 4,
      title: "Street Style",
      description: "Phong cách đường phố năng động, cá tính",
      imageUrl: "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?w=800&q=80",
      productCount: 18
    },
    {
      id: 5,
      title: "Minimalist",
      description: "Tối giản, tinh tế cho người yêu sự đơn giản",
      imageUrl: "https://images.unsplash.com/photo-1441984904996-e0b6ba687e04?w=800&q=80",
      productCount: 20
    },
    {
      id: 6,
      title: "Premium Collection",
      description: "Cao cấp, sang trọng với chất liệu đặc biệt",
      imageUrl: "https://images.unsplash.com/photo-1469334031218-e382a71b716b?w=800&q=80",
      productCount: 15
    }
  ];

  return (
    <div className="min-h-screen flex flex-col">
      <Navbar />
      
      <main className="flex-1">
        {/* Hero Section */}
        <section className="relative h-[400px] flex items-center justify-center bg-gradient-to-r from-primary/20 to-accent/20">
          <div className="container mx-auto px-4 text-center">
            <h1 className="text-5xl md:text-6xl font-bold mb-6">Bộ Sưu Tập</h1>
            <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
              Khám phá các bộ sưu tập thời trang độc đáo, được thiết kế dành riêng cho phong cách của bạn
            </p>
          </div>
        </section>

        {/* Collections Grid */}
        <section className="py-16">
          <div className="container mx-auto px-4">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
              {collections.map((collection) => (
                <Card key={collection.id} className="group overflow-hidden hover:shadow-lg transition-all">
                  <div className="relative h-80 overflow-hidden">
                    <img
                      src={collection.imageUrl}
                      alt={collection.title}
                      className="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
                    />
                    <div className="absolute inset-0 bg-gradient-to-t from-black/70 to-transparent" />
                    <div className="absolute bottom-0 left-0 right-0 p-6 text-white">
                      <h3 className="text-2xl font-bold mb-2">{collection.title}</h3>
                      <p className="text-sm text-white/90 mb-4">{collection.description}</p>
                      <div className="flex items-center justify-between">
                        <span className="text-sm">{collection.productCount} sản phẩm</span>
                        <Link to="/products">
                          <Button variant="secondary" size="sm" className="group-hover:translate-x-1 transition-transform">
                            Xem ngay
                            <ArrowRight className="ml-2 h-4 w-4" />
                          </Button>
                        </Link>
                      </div>
                    </div>
                  </div>
                </Card>
              ))}
            </div>
          </div>
        </section>

        {/* CTA Section */}
        <section className="py-16 bg-muted">
          <div className="container mx-auto px-4 text-center">
            <h2 className="text-3xl md:text-4xl font-bold mb-4">
              Không tìm thấy bộ sưu tập yêu thích?
            </h2>
            <p className="text-lg text-muted-foreground mb-8 max-w-2xl mx-auto">
              Khám phá toàn bộ sản phẩm của chúng tôi để tìm kiếm phong cách hoàn hảo cho bạn
            </p>
            <Link to="/products">
              <Button size="lg">
                Xem tất cả sản phẩm
                <ArrowRight className="ml-2 h-5 w-5" />
              </Button>
            </Link>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
};

export default Collections;
