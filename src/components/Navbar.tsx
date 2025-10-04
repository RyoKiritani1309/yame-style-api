import { useState } from "react";
import { Link } from "react-router-dom";
import { ShoppingCart, Search, Menu, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface NavbarProps {
  cartItemsCount?: number;
}

export const Navbar = ({ cartItemsCount = 0 }: NavbarProps) => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <nav className="sticky top-0 z-50 w-full border-b border-border bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60">
      <div className="container mx-auto px-4">
        <div className="flex h-16 items-center justify-between">
          {/* Logo */}
          <Link to="/" className="text-2xl font-bold tracking-tight text-foreground hover:text-accent transition-smooth">
            YAME
          </Link>

          {/* Desktop Navigation */}
          <div className="hidden md:flex items-center space-x-8">
            <Link to="/" className="text-sm font-medium text-foreground hover:text-accent transition-smooth">
              Trang chủ
            </Link>
            <Link to="/products" className="text-sm font-medium text-foreground hover:text-accent transition-smooth">
              Sản phẩm
            </Link>
            <Link to="/collections" className="text-sm font-medium text-foreground hover:text-accent transition-smooth">
              Bộ sưu tập
            </Link>
            <Link to="/about" className="text-sm font-medium text-foreground hover:text-accent transition-smooth">
              Về chúng tôi
            </Link>
          </div>

          {/* Search & Cart */}
          <div className="flex items-center space-x-4">
            <div className="hidden lg:flex items-center">
              <div className="relative">
                <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  type="search"
                  placeholder="Tìm kiếm sản phẩm..."
                  className="w-64 pl-10"
                />
              </div>
            </div>
            
            <Button variant="ghost" size="icon" className="relative">
              <ShoppingCart className="h-5 w-5" />
              {cartItemsCount > 0 && (
                <span className="absolute -right-1 -top-1 flex h-5 w-5 items-center justify-center rounded-full bg-accent text-xs font-bold text-accent-foreground">
                  {cartItemsCount}
                </span>
              )}
            </Button>

            {/* Mobile menu button */}
            <Button
              variant="ghost"
              size="icon"
              className="md:hidden"
              onClick={() => setIsMenuOpen(!isMenuOpen)}
            >
              {isMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
            </Button>
          </div>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <div className="md:hidden py-4 space-y-4 border-t border-border">
            <div className="px-2">
              <div className="relative">
                <Search className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground" />
                <Input
                  type="search"
                  placeholder="Tìm kiếm..."
                  className="w-full pl-10"
                />
              </div>
            </div>
            <Link
              to="/"
              className="block px-2 py-2 text-sm font-medium text-foreground hover:text-accent transition-smooth"
              onClick={() => setIsMenuOpen(false)}
            >
              Trang chủ
            </Link>
            <Link
              to="/products"
              className="block px-2 py-2 text-sm font-medium text-foreground hover:text-accent transition-smooth"
              onClick={() => setIsMenuOpen(false)}
            >
              Sản phẩm
            </Link>
            <Link
              to="/collections"
              className="block px-2 py-2 text-sm font-medium text-foreground hover:text-accent transition-smooth"
              onClick={() => setIsMenuOpen(false)}
            >
              Bộ sưu tập
            </Link>
            <Link
              to="/about"
              className="block px-2 py-2 text-sm font-medium text-foreground hover:text-accent transition-smooth"
              onClick={() => setIsMenuOpen(false)}
            >
              Về chúng tôi
            </Link>
          </div>
        )}
      </div>
    </nav>
  );
};
