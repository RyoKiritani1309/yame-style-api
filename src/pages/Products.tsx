import { useState } from "react";
import { ProductCard } from "@/components/ProductCard";
import { mockProducts } from "@/data/mockProducts";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Filter, X } from "lucide-react";

const Products = () => {
  const [showFilters, setShowFilters] = useState(false);
  const [products] = useState(mockProducts);

  return (
    <div className="min-h-screen bg-background">
      {/* Page Header */}
      <div className="border-b border-border bg-muted/30">
        <div className="container mx-auto px-4 py-8">
          <h1 className="text-4xl font-bold text-foreground">Sản phẩm</h1>
          <p className="mt-2 text-muted-foreground">
            Khám phá toàn bộ bộ sưu tập của chúng tôi
          </p>
        </div>
      </div>

      <div className="container mx-auto px-4 py-8">
        <div className="flex gap-8">
          {/* Filters Sidebar - Desktop */}
          <aside className="hidden w-64 shrink-0 lg:block">
            <div className="sticky top-24 space-y-6">
              <div>
                <h3 className="mb-4 text-lg font-semibold text-foreground">Bộ lọc</h3>
              </div>

              {/* Price Range */}
              <div className="space-y-3">
                <Label className="text-sm font-medium">Khoảng giá</Label>
                <div className="space-y-2">
                  <div className="flex items-center space-x-2">
                    <Checkbox id="price1" />
                    <label htmlFor="price1" className="text-sm text-muted-foreground cursor-pointer">
                      Dưới 200.000đ
                    </label>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Checkbox id="price2" />
                    <label htmlFor="price2" className="text-sm text-muted-foreground cursor-pointer">
                      200.000đ - 500.000đ
                    </label>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Checkbox id="price3" />
                    <label htmlFor="price3" className="text-sm text-muted-foreground cursor-pointer">
                      500.000đ - 1.000.000đ
                    </label>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Checkbox id="price4" />
                    <label htmlFor="price4" className="text-sm text-muted-foreground cursor-pointer">
                      Trên 1.000.000đ
                    </label>
                  </div>
                </div>
              </div>

              {/* Sizes */}
              <div className="space-y-3">
                <Label className="text-sm font-medium">Kích thước</Label>
                <div className="flex flex-wrap gap-2">
                  {["S", "M", "L", "XL", "XXL"].map((size) => (
                    <Button
                      key={size}
                      variant="outline"
                      size="sm"
                      className="h-10 w-10 p-0"
                    >
                      {size}
                    </Button>
                  ))}
                </div>
              </div>

              {/* Colors */}
              <div className="space-y-3">
                <Label className="text-sm font-medium">Màu sắc</Label>
                <div className="space-y-2">
                  {["Đen", "Trắng", "Xám", "Navy", "Vàng"].map((color) => (
                    <div key={color} className="flex items-center space-x-2">
                      <Checkbox id={`color-${color}`} />
                      <label
                        htmlFor={`color-${color}`}
                        className="text-sm text-muted-foreground cursor-pointer"
                      >
                        {color}
                      </label>
                    </div>
                  ))}
                </div>
              </div>

              {/* Category */}
              <div className="space-y-3">
                <Label className="text-sm font-medium">Danh mục</Label>
                <div className="space-y-2">
                  {["Áo", "Quần", "Giày", "Phụ kiện"].map((category) => (
                    <div key={category} className="flex items-center space-x-2">
                      <Checkbox id={`cat-${category}`} />
                      <label
                        htmlFor={`cat-${category}`}
                        className="text-sm text-muted-foreground cursor-pointer"
                      >
                        {category}
                      </label>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </aside>

          {/* Products Grid */}
          <div className="flex-1">
            {/* Toolbar */}
            <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
              <div className="flex items-center gap-2">
                <Button
                  variant="outline"
                  size="sm"
                  className="lg:hidden"
                  onClick={() => setShowFilters(!showFilters)}
                >
                  <Filter className="mr-2 h-4 w-4" />
                  Bộ lọc
                </Button>
                <p className="text-sm text-muted-foreground">
                  Hiển thị {products.length} sản phẩm
                </p>
              </div>

              <div className="flex items-center gap-2">
                <Label htmlFor="sort" className="text-sm">
                  Sắp xếp:
                </Label>
                <Select defaultValue="popularity">
                  <SelectTrigger id="sort" className="w-[180px]">
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="popularity">Phổ biến nhất</SelectItem>
                    <SelectItem value="price-asc">Giá: Thấp đến cao</SelectItem>
                    <SelectItem value="price-desc">Giá: Cao đến thấp</SelectItem>
                    <SelectItem value="newest">Mới nhất</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>

            {/* Mobile Filters */}
            {showFilters && (
              <div className="mb-6 rounded-lg border border-border bg-card p-4 lg:hidden">
                <div className="mb-4 flex items-center justify-between">
                  <h3 className="font-semibold">Bộ lọc</h3>
                  <Button
                    variant="ghost"
                    size="icon"
                    onClick={() => setShowFilters(false)}
                  >
                    <X className="h-4 w-4" />
                  </Button>
                </div>
                {/* Add mobile filter content here */}
              </div>
            )}

            {/* Products Grid */}
            <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
              {products.map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>

            {/* Pagination */}
            <div className="mt-12 flex justify-center">
              <div className="flex gap-2">
                <Button variant="outline" disabled>
                  Trước
                </Button>
                <Button variant="default">1</Button>
                <Button variant="outline">2</Button>
                <Button variant="outline">3</Button>
                <Button variant="outline">Sau</Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Products;
