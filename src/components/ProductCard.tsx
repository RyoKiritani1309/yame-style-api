import { Link } from "react-router-dom";
import { Product } from "@/types/product";
import { ShoppingCart } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";

interface ProductCardProps {
  product: Product;
}

export const ProductCard = ({ product }: ProductCardProps) => {
  const hasDiscount = product.salePrice && product.salePrice < product.price;
  const discountPercent = hasDiscount
    ? Math.round(((product.price - product.salePrice!) / product.price) * 100)
    : 0;

  return (
    <div className="group relative overflow-hidden rounded-lg bg-card transition-smooth hover:shadow-product-hover">
      <Link to={`/product/${product.slug}`} className="block">
        {/* Product Image */}
        <div className="relative aspect-square overflow-hidden bg-muted">
          <img
            src={product.images[0]}
            alt={product.title}
            className="h-full w-full object-cover transition-smooth group-hover:scale-105"
          />
          {hasDiscount && (
            <Badge className="absolute right-3 top-3 bg-destructive text-destructive-foreground">
              -{discountPercent}%
            </Badge>
          )}
          {!product.availability && (
            <div className="absolute inset-0 flex items-center justify-center bg-background/80">
              <Badge variant="secondary">Hết hàng</Badge>
            </div>
          )}
        </div>

        {/* Product Info */}
        <div className="p-4">
          <div className="mb-1 flex flex-wrap gap-1">
            {product.tags.slice(0, 2).map((tag) => (
              <Badge key={tag} variant="secondary" className="text-xs">
                {tag}
              </Badge>
            ))}
          </div>
          
          <h3 className="mb-2 font-semibold text-card-foreground line-clamp-2 group-hover:text-accent transition-smooth">
            {product.title}
          </h3>
          
          <p className="mb-3 text-sm text-muted-foreground line-clamp-2">
            {product.shortDescription}
          </p>

          <div className="flex items-center justify-between">
            <div className="flex items-baseline gap-2">
              <span className="text-lg font-bold text-foreground">
                {(product.salePrice || product.price).toLocaleString('vi-VN')}đ
              </span>
              {hasDiscount && (
                <span className="text-sm text-muted-foreground line-through">
                  {product.price.toLocaleString('vi-VN')}đ
                </span>
              )}
            </div>
          </div>
        </div>
      </Link>

      {/* Quick Add to Cart Button */}
      <div className="absolute bottom-4 right-4 opacity-0 transition-smooth group-hover:opacity-100">
        <Button
          size="icon"
          className="rounded-full bg-accent text-accent-foreground shadow-lg hover:bg-accent/90"
          onClick={(e) => {
            e.preventDefault();
            // Add to cart logic here
          }}
        >
          <ShoppingCart className="h-4 w-4" />
        </Button>
      </div>
    </div>
  );
};
