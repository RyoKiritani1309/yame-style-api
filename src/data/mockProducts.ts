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
  },
  {
    id: 5,
    title: "Quần Jean Slim Fit",
    slug: "quan-jean-slim-fit",
    shortDescription: "Quần jean ôm vừa phải, co giãn thoải mái",
    description: "Quần jean slim fit với chất liệu denim cao cấp, co giãn nhẹ, tạo form dáng thon gọn. Thiết kế hiện đại, phù hợp cho nhiều dịp khác nhau.",
    price: 499000,
    images: [product1],
    variants: [
      { variantId: 15, sku: "YM-QJ-005-30", size: "30", color: "Xám", stock: 15, price: 499000 },
      { variantId: 16, sku: "YM-QJ-005-32", size: "32", color: "Xám", stock: 20, price: 499000 },
      { variantId: 17, sku: "YM-QJ-005-34", size: "34", color: "Xám", stock: 12, price: 499000 },
    ],
    availability: true,
    tags: ["jean", "slim-fit", "casual"],
    primaryCategory: "Quần",
    specs: {
      material: "Denim co giãn",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.6,
      count: 95
    }
  },
  {
    id: 6,
    title: "Áo Polo Basic",
    slug: "ao-polo-basic",
    shortDescription: "Áo polo nam/nữ phong cách tối giản",
    description: "Áo polo basic với chất liệu cotton cao cấp, form dáng hiện đại, phù hợp cho mọi hoạt động hàng ngày.",
    price: 299000,
    salePrice: 249000,
    images: [product2],
    variants: [
      { variantId: 18, sku: "YM-PL-006-S", size: "S", color: "Trắng", stock: 25, price: 249000 },
      { variantId: 19, sku: "YM-PL-006-M", size: "M", color: "Trắng", stock: 30, price: 249000 },
      { variantId: 20, sku: "YM-PL-006-L", size: "L", color: "Trắng", stock: 20, price: 249000 },
      { variantId: 21, sku: "YM-PL-006-XL", size: "XL", color: "Trắng", stock: 15, price: 249000 },
    ],
    availability: true,
    tags: ["polo", "basic", "casual"],
    primaryCategory: "Áo",
    specs: {
      material: "100% Cotton",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.4,
      count: 110
    }
  },
  {
    id: 7,
    title: "Quần Short Kaki",
    slug: "quan-short-kaki",
    shortDescription: "Quần short kaki thoải mái cho mùa hè",
    description: "Quần short kaki với chất liệu thoáng mát, thiết kế đơn giản nhưng tinh tế. Lựa chọn hoàn hảo cho những ngày hè nóng bức.",
    price: 350000,
    salePrice: 280000,
    images: [product3],
    variants: [
      { variantId: 22, sku: "YM-QS-007-M", size: "M", color: "Đen", stock: 18, price: 280000 },
      { variantId: 23, sku: "YM-QS-007-L", size: "L", color: "Đen", stock: 22, price: 280000 },
      { variantId: 24, sku: "YM-QS-007-XL", size: "XL", color: "Đen", stock: 14, price: 280000 },
    ],
    availability: true,
    tags: ["short", "kaki", "summer"],
    primaryCategory: "Quần",
    specs: {
      material: "Kaki Premium",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.3,
      count: 73
    }
  },
  {
    id: 8,
    title: "Túi Tote Canvas",
    slug: "tui-tote-canvas",
    shortDescription: "Túi tote canvas đơn giản, tiện dụng",
    description: "Túi tote canvas thiết kế tối giản, chất liệu bền bỉ. Phù hợp cho đi làm, đi học hay đi chơi.",
    price: 150000,
    images: [product4],
    variants: [
      { variantId: 25, sku: "YM-TT-008-OS", size: "OS", color: "Trắng", stock: 40, price: 150000 },
      { variantId: 26, sku: "YM-TT-008-OS-B", size: "OS", color: "Đen", stock: 35, price: 150000 },
    ],
    availability: true,
    tags: ["tote", "canvas", "accessory"],
    primaryCategory: "Phụ kiện",
    specs: {
      material: "Canvas",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.5,
      count: 88
    }
  },
  {
    id: 9,
    title: "Áo Thun Oversized",
    slug: "ao-thun-oversized",
    shortDescription: "Áo thun form rộng phong cách streetwear",
    description: "Áo thun oversized chất liệu cotton 100%, form rộng thoải mái, phong cách streetwear hiện đại.",
    price: 199000,
    images: [product1],
    variants: [
      { variantId: 27, sku: "YM-TO-009-M", size: "M", color: "Xám", stock: 28, price: 199000 },
      { variantId: 28, sku: "YM-TO-009-L", size: "L", color: "Xám", stock: 32, price: 199000 },
      { variantId: 29, sku: "YM-TO-009-XL", size: "XL", color: "Xám", stock: 25, price: 199000 },
    ],
    availability: true,
    tags: ["oversized", "streetwear", "casual"],
    primaryCategory: "Áo",
    specs: {
      material: "100% Cotton",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.7,
      count: 145
    }
  },
  {
    id: 10,
    title: "Giày Sneaker Sport",
    slug: "giay-sneaker-sport",
    shortDescription: "Giày sneaker thể thao năng động",
    description: "Giày sneaker sport với thiết kế năng động, đế êm ái hỗ trợ tốt cho các hoạt động thể thao.",
    price: 750000,
    salePrice: 650000,
    images: [product2],
    variants: [
      { variantId: 30, sku: "YM-GS-010-39", size: "39", color: "Đen", stock: 10, price: 650000 },
      { variantId: 31, sku: "YM-GS-010-40", size: "40", color: "Đen", stock: 15, price: 650000 },
      { variantId: 32, sku: "YM-GS-010-41", size: "41", color: "Đen", stock: 12, price: 650000 },
      { variantId: 33, sku: "YM-GS-010-42", size: "42", color: "Đen", stock: 8, price: 650000 },
    ],
    availability: true,
    tags: ["sneaker", "sport", "active"],
    primaryCategory: "Giày",
    specs: {
      material: "Mesh + Rubber",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.8,
      count: 92
    }
  },
  {
    id: 11,
    title: "Mũ Lưỡi Trai Classic",
    slug: "mu-luoi-trai-classic",
    shortDescription: "Mũ lưỡi trai thiết kế cổ điển",
    description: "Mũ lưỡi trai với thiết kế cổ điển, chất liệu cotton thoáng mát. Phụ kiện hoàn hảo cho mọi outfit.",
    price: 120000,
    images: [product3],
    variants: [
      { variantId: 34, sku: "YM-ML-011-OS", size: "OS", color: "Đen", stock: 50, price: 120000 },
      { variantId: 35, sku: "YM-ML-011-OS-N", size: "OS", color: "Navy", stock: 45, price: 120000 },
    ],
    availability: true,
    tags: ["cap", "accessory", "classic"],
    primaryCategory: "Phụ kiện",
    specs: {
      material: "Cotton",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.6,
      count: 78
    }
  },
  {
    id: 12,
    title: "Áo Sơ Mi Linen",
    slug: "ao-so-mi-linen",
    shortDescription: "Áo sơ mi linen mát mẻ, thanh lịch",
    description: "Áo sơ mi linen cao cấp, thoáng mát và thanh lịch. Phù hợp cho môi trường công sở và dạo phố.",
    price: 580000,
    salePrice: 480000,
    images: [product4],
    variants: [
      { variantId: 36, sku: "YM-SM-012-S", size: "S", color: "Trắng", stock: 12, price: 480000 },
      { variantId: 37, sku: "YM-SM-012-M", size: "M", color: "Trắng", stock: 18, price: 480000 },
      { variantId: 38, sku: "YM-SM-012-L", size: "L", color: "Trắng", stock: 15, price: 480000 },
      { variantId: 39, sku: "YM-SM-012-XL", size: "XL", color: "Trắng", stock: 10, price: 480000 },
    ],
    availability: true,
    tags: ["shirt", "linen", "formal"],
    primaryCategory: "Áo",
    specs: {
      material: "Linen Premium",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.9,
      count: 65
    }
  },
  {
    id: 13,
    title: "Quần Jogger Thể Thao",
    slug: "quan-jogger-the-thao",
    shortDescription: "Quần jogger thoải mái cho mọi hoạt động",
    description: "Quần jogger thể thao với chất liệu co giãn 4 chiều, thoải mái cho mọi vận động. Thiết kế hiện đại với túi khóa kéo.",
    price: 420000,
    images: [product1],
    variants: [
      { variantId: 40, sku: "YM-QJ-013-M", size: "M", color: "Navy", stock: 20, price: 420000 },
      { variantId: 41, sku: "YM-QJ-013-L", size: "L", color: "Navy", stock: 25, price: 420000 },
      { variantId: 42, sku: "YM-QJ-013-XL", size: "XL", color: "Navy", stock: 18, price: 420000 },
      { variantId: 43, sku: "YM-QJ-013-XXL", size: "XXL", color: "Navy", stock: 12, price: 420000 },
    ],
    availability: true,
    tags: ["jogger", "sport", "active"],
    primaryCategory: "Quần",
    specs: {
      material: "Polyester co giãn",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.7,
      count: 102
    }
  },
  {
    id: 14,
    title: "Balo Du Lịch",
    slug: "balo-du-lich",
    shortDescription: "Balo du lịch đa năng, chống nước",
    description: "Balo du lịch với nhiều ngăn tiện dụng, chất liệu chống nước. Thiết kế ergonomic êm ái cho lưng và vai.",
    price: 1200000,
    salePrice: 980000,
    images: [product2],
    variants: [
      { variantId: 44, sku: "YM-BL-014-OS", size: "OS", color: "Đen", stock: 15, price: 980000 },
      { variantId: 45, sku: "YM-BL-014-OS-G", size: "OS", color: "Xám", stock: 12, price: 980000 },
    ],
    availability: true,
    tags: ["backpack", "travel", "waterproof"],
    primaryCategory: "Phụ kiện",
    specs: {
      material: "Polyester chống nước",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.8,
      count: 156
    }
  },
  {
    id: 15,
    title: "Áo Khoác Bomber",
    slug: "ao-khoac-bomber",
    shortDescription: "Áo khoác bomber phong cách vintage",
    description: "Áo khoác bomber với thiết kế vintage, chất liệu vải dù cao cấp. Form dáng chuẩn, dễ phối đồ.",
    price: 850000,
    salePrice: 720000,
    images: [product3],
    variants: [
      { variantId: 46, sku: "YM-BM-015-M", size: "M", color: "Xám", stock: 10, price: 720000 },
      { variantId: 47, sku: "YM-BM-015-L", size: "L", color: "Xám", stock: 14, price: 720000 },
      { variantId: 48, sku: "YM-BM-015-XL", size: "XL", color: "Xám", stock: 8, price: 720000 },
    ],
    availability: true,
    tags: ["bomber", "jacket", "vintage"],
    primaryCategory: "Áo",
    specs: {
      material: "Vải dù cao cấp",
      madeIn: "Việt Nam"
    },
    reviews: {
      average: 4.6,
      count: 81
    }
  }
];
