import { Navbar } from "@/components/Navbar";
import { Footer } from "@/components/Footer";
import { Card } from "@/components/ui/card";
import { Heart, Sparkles, Users, TrendingUp } from "lucide-react";

const About = () => {
  const values = [
    {
      icon: Heart,
      title: "Đam mê",
      description: "Chúng tôi yêu thời trang và tận tâm với từng sản phẩm mang đến cho khách hàng"
    },
    {
      icon: Sparkles,
      title: "Chất lượng",
      description: "Cam kết chất lượng cao cấp trong từng chi tiết, từ vải vóc đến đường may"
    },
    {
      icon: Users,
      title: "Cộng đồng",
      description: "Xây dựng cộng đồng yêu thời trang, kết nối những người có cùng phong cách"
    },
    {
      icon: TrendingUp,
      title: "Đổi mới",
      description: "Luôn cập nhật xu hướng mới nhất, thiết kế độc đáo và sáng tạo"
    }
  ];

  const team = [
    {
      name: "Nguyễn Văn A",
      role: "CEO & Founder",
      imageUrl: "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&q=80"
    },
    {
      name: "Trần Thị B",
      role: "Creative Director",
      imageUrl: "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400&q=80"
    },
    {
      name: "Lê Văn C",
      role: "Head of Design",
      imageUrl: "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400&q=80"
    },
    {
      name: "Phạm Thị D",
      role: "Marketing Manager",
      imageUrl: "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400&q=80"
    }
  ];

  return (
    <div className="min-h-screen flex flex-col">
      <Navbar />
      
      <main className="flex-1">
        {/* Hero Section */}
        <section className="relative h-[500px] flex items-center justify-center bg-gradient-to-r from-primary/20 to-accent/20">
          <div className="container mx-auto px-4 text-center">
            <h1 className="text-5xl md:text-6xl font-bold mb-6">Về YAME</h1>
            <p className="text-xl text-muted-foreground max-w-3xl mx-auto">
              Thương hiệu thời trang Việt Nam hiện đại, mang đến phong cách trẻ trung, 
              năng động với chất lượng cao cấp và thiết kế độc đáo
            </p>
          </div>
        </section>

        {/* Story Section */}
        <section className="py-16">
          <div className="container mx-auto px-4">
            <div className="grid md:grid-cols-2 gap-12 items-center">
              <div>
                <h2 className="text-4xl font-bold mb-6">Câu chuyện của chúng tôi</h2>
                <div className="space-y-4 text-muted-foreground">
                  <p>
                    YAME được thành lập vào năm 2020 với mục tiêu mang đến những sản phẩm 
                    thời trang chất lượng cao, thiết kế hiện đại và phù hợp với phong cách 
                    giới trẻ Việt Nam.
                  </p>
                  <p>
                    Chúng tôi tin rằng thời trang không chỉ là trang phục, mà là cách thể hiện 
                    bản thân, là nghệ thuật và là niềm đam mê. Mỗi sản phẩm của YAME đều được 
                    thiết kế tỉ mỉ, chọn lựa chất liệu kỹ càng để đảm bảo sự hài lòng tuyệt đối 
                    cho khách hàng.
                  </p>
                  <p>
                    Với đội ngũ thiết kế trẻ trung, sáng tạo, YAME không ngừng đổi mới và 
                    cập nhật những xu hướng thời trang mới nhất từ khắp nơi trên thế giới, 
                    kết hợp với nét văn hóa đặc trưng của Việt Nam.
                  </p>
                </div>
              </div>
              <div className="relative h-[500px]">
                <img
                  src="https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=800&q=80"
                  alt="YAME Store"
                  className="w-full h-full object-cover rounded-lg shadow-lg"
                />
              </div>
            </div>
          </div>
        </section>

        {/* Values Section */}
        <section className="py-16 bg-muted">
          <div className="container mx-auto px-4">
            <div className="text-center mb-12">
              <h2 className="text-4xl font-bold mb-4">Giá trị cốt lõi</h2>
              <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
                Những giá trị định hướng mọi hoạt động của YAME
              </p>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
              {values.map((value, index) => (
                <Card key={index} className="p-6 text-center hover:shadow-lg transition-shadow">
                  <div className="inline-flex items-center justify-center w-16 h-16 rounded-full bg-primary/10 mb-4">
                    <value.icon className="h-8 w-8 text-primary" />
                  </div>
                  <h3 className="text-xl font-bold mb-3">{value.title}</h3>
                  <p className="text-muted-foreground">{value.description}</p>
                </Card>
              ))}
            </div>
          </div>
        </section>

        {/* Team Section */}
        <section className="py-16">
          <div className="container mx-auto px-4">
            <div className="text-center mb-12">
              <h2 className="text-4xl font-bold mb-4">Đội ngũ của chúng tôi</h2>
              <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
                Những con người tài năng đứng sau thành công của YAME
              </p>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
              {team.map((member, index) => (
                <Card key={index} className="overflow-hidden hover:shadow-lg transition-shadow">
                  <div className="relative h-80">
                    <img
                      src={member.imageUrl}
                      alt={member.name}
                      className="w-full h-full object-cover"
                    />
                  </div>
                  <div className="p-6 text-center">
                    <h3 className="text-xl font-bold mb-1">{member.name}</h3>
                    <p className="text-muted-foreground">{member.role}</p>
                  </div>
                </Card>
              ))}
            </div>
          </div>
        </section>

        {/* Stats Section */}
        <section className="py-16 bg-primary text-primary-foreground">
          <div className="container mx-auto px-4">
            <div className="grid grid-cols-2 md:grid-cols-4 gap-8 text-center">
              <div>
                <div className="text-5xl font-bold mb-2">5+</div>
                <div className="text-lg opacity-90">Năm kinh nghiệm</div>
              </div>
              <div>
                <div className="text-5xl font-bold mb-2">50K+</div>
                <div className="text-lg opacity-90">Khách hàng</div>
              </div>
              <div>
                <div className="text-5xl font-bold mb-2">200+</div>
                <div className="text-lg opacity-90">Sản phẩm</div>
              </div>
              <div>
                <div className="text-5xl font-bold mb-2">10+</div>
                <div className="text-lg opacity-90">Cửa hàng</div>
              </div>
            </div>
          </div>
        </section>
      </main>

      <Footer />
    </div>
  );
};

export default About;
