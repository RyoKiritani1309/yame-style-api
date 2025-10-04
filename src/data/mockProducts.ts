import { Product } from "@/types/product";
import product1 from "@/assets/product-1.jpg";
import product2 from "@/assets/product-2.jpg";
import product3 from "@/assets/product-3.jpg";
import product4 from "@/assets/product-4.jpg";

export const mockProducts: Product[] = [
  {
    id: 1,
    title: "Áo Thun Thermo Mesh",
    slug: "ao-thun-thermo-mesh",
    shortDescription: "Áo thun cao cấp với công nghệ Thermo Mesh thoáng mát",
    description: "Áo thun Thermo Mesh được thiết kế đặc biệt với chất liệu cao cấp, mang lại cảm giác thoáng mát suốt cả ngày. Phù hợp cho mọi hoạt động từ đi làm đến dạo phố.",
    price: 196200,
    salePrice: 196200,
    images: [product1],
    variants: [
      { variantId: 1, sku: "YM-AT-001-S", size: "S", color: "Vàng", stock: 10, price: 196200 },
      { variantId: 2, sku: "YM-AT-001-M", size: "M", color: "Vàng", stock: 15, price: 196200 },
      { variantId: 3, sku: "YM-AT-001-L", size: "L", color: "Vàng", stock: 8, price: 196200 },
      { variantId: 4, sku: "YM-AT-001-XL", size: "XL", color: "Vàng", stock: 5, price: 196200 },
    ],
    availability: true,
    tags: ["casual", "thermo-mesh", "nam"],
    primaryCategory: "Áo",
    specs: {
      material: "Thermo Mesh",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.6,
      count: 120
    }
  },
  {
    id: 2,
    title: "Áo Hoodie Premium",
    slug: "ao-hoodie-premium",
    shortDescription: "Áo hoodie thời trang, chất liệu cotton cao cấp",
    description: "Áo hoodie được thiết kế hiện đại với chất liệu cotton cao cấp, giữ ấm tốt và thoải mái. Form dáng chuẩn Hàn Quốc, phù hợp cho mọi phong cách.",
    price: 450000,
    salePrice: 380000,
    images: [product2],
    variants: [
      { variantId: 5, sku: "YM-HD-002-M", size: "M", color: "Đen", stock: 12, price: 380000 },
      { variantId: 6, sku: "YM-HD-002-L", size: "L", color: "Đen", stock: 20, price: 380000 },
      { variantId: 7, sku: "YM-HD-002-XL", size: "XL", color: "Đen", stock: 7, price: 380000 },
    ],
    availability: true,
    tags: ["hoodie", "premium", "streetwear"],
    primaryCategory: "Áo",
    specs: {
      material: "Cotton Premium",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.8,
      count: 85
    }
  },
  {
    id: 3,
    title: "Giày Sneaker Classic",
    slug: "giay-sneaker-classic",
    shortDescription: "Giày sneaker trắng phối màu, êm ái và bền bỉ",
    description: "Giày sneaker thiết kế classic với đế cao su êm ái, chống trơn trượt. Màu trắng dễ phối đồ, phù hợp cho mọi lứa tuổi.",
    price: 550000,
    salePrice: 495000,
    images: [product3],
    variants: [
      { variantId: 8, sku: "YM-GY-003-39", size: "39", color: "Trắng", stock: 8, price: 495000 },
      { variantId: 9, sku: "YM-GY-003-40", size: "40", color: "Trắng", stock: 15, price: 495000 },
      { variantId: 10, sku: "YM-GY-003-41", size: "41", color: "Trắng", stock: 12, price: 495000 },
      { variantId: 11, sku: "YM-GY-003-42", size: "42", color: "Trắng", stock: 10, price: 495000 },
    ],
    availability: true,
    tags: ["sneaker", "giày", "classic"],
    primaryCategory: "Giày",
    specs: {
      material: "Canvas + Rubber",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.5,
      count: 67
    }
  },
  {
    id: 4,
    title: "Áo Khoác Gió Minimal",
    slug: "ao-khoac-gio-minimal",
    shortDescription: "Áo khoác gió nhẹ, chống nước hiệu quả",
    description: "Áo khoác gió với thiết kế tối giản, chất liệu chống nước tốt. Dễ dàng gấp gọn mang theo, là item không thể thiếu trong tủ đồ.",
    price: 680000,
    images: [product4],
    variants: [
      { variantId: 12, sku: "YM-AK-004-M", size: "M", color: "Navy", stock: 6, price: 680000 },
      { variantId: 13, sku: "YM-AK-004-L", size: "L", color: "Navy", stock: 10, price: 680000 },
      { variantId: 14, sku: "YM-AK-004-XL", size: "XL", color: "Navy", stock: 4, price: 680000 },
    ],
    availability: true,
    tags: ["jacket", "windbreaker", "minimal"],
    primaryCategory: "Áo",
    specs: {
      material: "Polyester chống nước",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.7,
      count: 52
    }
  }
];
