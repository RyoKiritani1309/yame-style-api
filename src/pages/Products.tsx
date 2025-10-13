import { useState, useMemo } from "react";
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
import { Filter, X, Search } from "lucide-react";
import { Product } from "@/types/product";

const Products = () => {
  const [showFilters, setShowFilters] = useState(false);
  const [searchQuery, setSearchQuery] = useState("");
  const [selectedPriceRanges, setSelectedPriceRanges] = useState<string[]>([]);
  const [selectedSizes, setSelectedSizes] = useState<string[]>([]);
  const [selectedColors, setSelectedColors] = useState<string[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
  const [sortBy, setSortBy] = useState("popularity");

  const togglePriceRange = (range: string) => {
    setSelectedPriceRanges(prev =>
      prev.includes(range) ? prev.filter(r => r !== range) : [...prev, range]
    );
  };

  const toggleSize = (size: string) => {
    setSelectedSizes(prev =>
      prev.includes(size) ? prev.filter(s => s !== size) : [...prev, size]
    );
  };

  const toggleColor = (color: string) => {
    setSelectedColors(prev =>
      prev.includes(color) ? prev.filter(c => c !== color) : [...prev, color]
    );
  };

  const toggleCategory = (category: string) => {
    setSelectedCategories(prev =>
      prev.includes(category) ? prev.filter(c => c !== category) : [...prev, category]
    );
  };

  const filteredAndSortedProducts = useMemo(() => {
    let filtered = mockProducts.filter((product) => {
      // Search filter
      if (searchQuery) {
        const query = searchQuery.toLowerCase();
        const matchesSearch = 
          product.title.toLowerCase().includes(query) ||
          product.shortDescription.toLowerCase().includes(query) ||
          product.description.toLowerCase().includes(query) ||
          product.tags.some(tag => tag.toLowerCase().includes(query));
        if (!matchesSearch) return false;
      }

      // Price range filter
      if (selectedPriceRanges.length > 0) {
        const price = product.salePrice || product.price;
        const inRange = selectedPriceRanges.some(range => {
          if (range === "under-200k") return price < 200000;
          if (range === "200k-500k") return price >= 200000 && price <= 500000;
          if (range === "500k-1m") return price >= 500000 && price <= 1000000;
          if (range === "over-1m") return price > 1000000;
          return false;
        });
        if (!inRange) return false;
      }

      // Size filter
      if (selectedSizes.length > 0) {
        const hasSize = product.variants.some(v => 
          selectedSizes.includes(v.size)
        );
        if (!hasSize) return false;
      }

      // Color filter
      if (selectedColors.length > 0) {
        const hasColor = product.variants.some(v => 
          selectedColors.includes(v.color)
        );
        if (!hasColor) return false;
      }

      // Category filter
      if (selectedCategories.length > 0) {
        if (!selectedCategories.includes(product.primaryCategory)) return false;
      }

      return true;
    });

    // Sorting
    filtered.sort((a, b) => {
      const priceA = a.salePrice || a.price;
      const priceB = b.salePrice || b.price;

      switch (sortBy) {
        case "price-asc":
          return priceA - priceB;
        case "price-desc":
          return priceB - priceA;
        case "newest":
          return b.id - a.id;
        case "popularity":
        default:
          return (b.reviews?.count || 0) - (a.reviews?.count || 0);
      }
    });

    return filtered;
  }, [searchQuery, selectedPriceRanges, selectedSizes, selectedColors, selectedCategories, sortBy]);

  return (
    <div className="min-h-screen bg-background">
      {/* Page Header */}
      <div className="border-b border-border bg-muted/30">
        <div className="container mx-auto px-4 py-8">
          <h1 className="text-4xl font-bold text-foreground">Sản phẩm</h1>
          <p className="mt-2 text-muted-foreground">
            Khám phá toàn bộ bộ sưu tập của chúng tôi
          </p>
          
          {/* Search Bar */}
          <div className="mt-6 relative max-w-md">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 h-4 w-4 text-muted-foreground" />
            <Input
              type="text"
              placeholder="Tìm kiếm sản phẩm..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="pl-10"
            />
          </div>
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
                    <Checkbox 
                      id="price1" 
                      checked={selectedPriceRanges.includes("under-200k")}
                      onCheckedChange={() => togglePriceRange("under-200k")}
                    />
                    <label htmlFor="price1" className="text-sm text-muted-foreground cursor-pointer">
                      Dưới 200.000đ
                    </label>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Checkbox 
                      id="price2" 
                      checked={selectedPriceRanges.includes("200k-500k")}
                      onCheckedChange={() => togglePriceRange("200k-500k")}
                    />
                    <label htmlFor="price2" className="text-sm text-muted-foreground cursor-pointer">
                      200.000đ - 500.000đ
                    </label>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Checkbox 
                      id="price3" 
                      checked={selectedPriceRanges.includes("500k-1m")}
                      onCheckedChange={() => togglePriceRange("500k-1m")}
                    />
                    <label htmlFor="price3" className="text-sm text-muted-foreground cursor-pointer">
                      500.000đ - 1.000.000đ
                    </label>
                  </div>
                  <div className="flex items-center space-x-2">
                    <Checkbox 
                      id="price4" 
                      checked={selectedPriceRanges.includes("over-1m")}
                      onCheckedChange={() => togglePriceRange("over-1m")}
                    />
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
                  {["S", "M", "L", "XL", "XXL", "30", "32", "34", "39", "40", "41", "42", "OS"].map((size) => (
                    <Button
                      key={size}
                      variant={selectedSizes.includes(size) ? "default" : "outline"}
                      size="sm"
                      className="h-10 min-w-10 px-3"
                      onClick={() => toggleSize(size)}
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
                      <Checkbox 
                        id={`color-${color}`} 
                        checked={selectedColors.includes(color)}
                        onCheckedChange={() => toggleColor(color)}
                      />
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
                      <Checkbox 
                        id={`cat-${category}`} 
                        checked={selectedCategories.includes(category)}
                        onCheckedChange={() => toggleCategory(category)}
                      />
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
                  Hiển thị {filteredAndSortedProducts.length} sản phẩm
                </p>
              </div>

              <div className="flex items-center gap-2">
                <Label htmlFor="sort" className="text-sm">
                  Sắp xếp:
                </Label>
                <Select value={sortBy} onValueChange={setSortBy}>
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
            {filteredAndSortedProducts.length > 0 ? (
              <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
                {filteredAndSortedProducts.map((product) => (
                  <ProductCard key={product.id} product={product} />
                ))}
              </div>
            ) : (
              <div className="py-12 text-center">
                <p className="text-lg text-muted-foreground">
                  Không tìm thấy sản phẩm nào phù hợp với bộ lọc của bạn.
                </p>
              </div>
            )}

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
