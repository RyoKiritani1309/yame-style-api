export interface ProductVariant {
  variantId: number;
  sku: string;
  size: string;
  color: string;
  stock: number;
  price: number;
}

export interface Product {
  id: number;
  title: string;
  slug: string;
  shortDescription?: string;
  description?: string;
  price: number;
  salePrice?: number;
  images: string[];
  variants: ProductVariant[];
  availability: boolean;
  tags: string[];
  primaryCategory: string;
  specs?: {
    material?: string;
    madeIn?: string;
  };
  reviews?: {
    average: number;
    count: number;
  };
}

export interface CartItem {
  itemId: string;
  variantId: number;
  product: Product;
  variant: ProductVariant;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface Cart {
  cartId: string;
  items: CartItem[];
  subTotal: number;
  discount: number;
  shipping: number;
  tax: number;
  total: number;
}
