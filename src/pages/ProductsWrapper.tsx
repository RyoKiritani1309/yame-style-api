import { Navbar } from "@/components/Navbar";
import { Footer } from "@/components/Footer";
import Products from "./Products";

const ProductsWrapper = () => {
  return (
    <div className="min-h-screen">
      <Navbar cartItemsCount={0} />
      <Products />
      <Footer />
    </div>
  );
};

export default ProductsWrapper;
