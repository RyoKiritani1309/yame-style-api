import { Button } from "@/components/ui/button";
import { ArrowRight } from "lucide-react";
import { Link } from "react-router-dom";
import heroImage from "@/assets/hero-fashion.jpg";

export const Hero = () => {
  return (
    <section className="relative h-[600px] overflow-hidden bg-gradient-to-br from-[hsl(var(--hero-gradient-start))] to-[hsl(var(--hero-gradient-end))]">
      {/* Background Image with Overlay */}
      <div className="absolute inset-0">
        <img
          src={heroImage}
          alt="Fashion collection"
          className="h-full w-full object-cover opacity-40"
        />
        <div className="absolute inset-0 bg-gradient-to-r from-primary/80 to-primary/40" />
      </div>

      {/* Content */}
      <div className="container relative mx-auto flex h-full items-center px-4">
        <div className="max-w-2xl space-y-6 text-primary-foreground">
          <h1 className="text-5xl font-bold leading-tight tracking-tight md:text-6xl lg:text-7xl">
            Thời Trang
            <br />
            <span className="text-accent">Hiện Đại</span>
          </h1>
          
          <p className="text-lg text-primary-foreground/90 md:text-xl">
            Khám phá bộ sưu tập mới nhất với thiết kế độc đáo và chất lượng cao cấp.
            Phong cách Việt, chất lượng quốc tế.
          </p>

          <div className="flex flex-wrap gap-4">
            <Button
              size="lg"
              className="bg-accent text-accent-foreground hover:bg-accent/90 transition-smooth group"
              asChild
            >
              <Link to="/products">
                Mua sắm ngay
                <ArrowRight className="ml-2 h-4 w-4 transition-smooth group-hover:translate-x-1" />
              </Link>
            </Button>
            
            <Button
              size="lg"
              variant="outline"
              className="border-primary-foreground/20 bg-primary-foreground/10 text-primary-foreground hover:bg-primary-foreground/20 backdrop-blur-sm"
              asChild
            >
              <Link to="/collections">
                Bộ sưu tập
              </Link>
            </Button>
          </div>
        </div>
      </div>

      {/* Decorative Elements */}
      <div className="absolute -bottom-1 left-0 right-0 h-20 bg-gradient-to-t from-background to-transparent" />
    </section>
  );
};
