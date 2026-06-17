using mvc._01.Models;
using System.Security.Cryptography.X509Certificates;

namespace mvc._01.Services
{
    public class PlanetService : List<PlanetModel>
    {
        public PlanetService () 
        { 
            Add(new PlanetModel
            {
                Id=1, 
                Name= "Mercury",
                VnName= "Sao Thủy",
                Content= "Sao Thủy hay Thủy Tinh (chữ Hán: 水星; tiếng Anh: Mercury) là hành tinh nhỏ nhất và gần Mặt Trời nhất trong tám hành tinh thuộc hệ Mặt Trời,[a] với chu kỳ quỹ đạo bằng khoảng 88 ngày Trái Đất. Nhìn từ Trái Đất, hành tinh hiện lên với chu kỳ giao hội trên quỹ đạo bằng xấp xỉ 116 ngày, và nhanh hơn hẳn những hành tinh khác. Tốc độ chuyển động nhanh này đã khiến người La Mã đặt tên hành tinh là Mercurius, vị thần liên lạc và đưa tin một cách nhanh chóng. Trong thần thoại Hy Lạp tên của vị thần này là Hermes (Ερμής). Tên tiếng Việt của hành tinh này dựa theo tên do Trung Quốc đặt, chọn theo hành thủy trong ngũ hành."
            });

            Add(new PlanetModel
            {
                Id = 2,
                Name = "Venus",
                VnName = "Sao Kim",
                Content = "Sao Kim hay Kim Tinh (chữ Hán: 金星), còn gọi là sao Thái Bạch (太白), Thái Bạch Kim Tinh (太白金星) (tiếng Anh: Venus) là hành tinh thứ 2 trong Hệ Mặt Trời, tự quay quanh nó với chu kỳ khoảng 243 ngày Trái Đất.[7] Xếp sau Mặt Trăng, nó là thiên thể tự nhiên sáng nhất trong bầu trời tối, với cấp sao biểu kiến bằng −4.6, đủ sáng để tạo nên bóng trên mặt nước.[12] Bởi vì Sao Kim là hành tinh phía trong tính từ Trái Đất, nó không bao giờ xuất hiện trên bầu trời mà quá xa Mặt Trời: góc ly giác đạt cực đại bằng 47,8°. Sao Kim đạt độ sáng lớn nhất ngay sát thời điểm hoàng hôn hoặc bình minh, do vậy mà dân gian còn gọi là sao Hôm, khi hành tinh này hiện lên lúc hoàng hôn, và sao Mai, khi hành tinh này hiện lên lúc bình minh."
            });

            Add(new PlanetModel
            {
                Id = 3,
                Name = "Earth",
                VnName = "Trái Đất",
                Content = "Trái Đất, còn được gọi là Địa Cầu (: Earth), là , đồng thời cũng là hành tinh lớn nhất trong các của xét về , và . Trái Đất còn được biết tên với các tên gọi \"hành tinh xanh\", là nhà của hàng triệu loài , trong đó có và cho đến nay nó là nơi duy nhất trong được biết đến là có . này được hình thành cách đây khoảng và sự sống xuất hiện trên bề mặt của nó khoảng 3,7 tỷ năm trước. Kể từ đó, , của Trái Đất và các điều kiện vô cơ khác đã thay đổi đáng kể, tạo điều kiện thuận lợi cho sự phổ biến của các cũng như sự hình thành của -lớp bảo vệ quan trọng, cùng với từ trường của Trái Đất, đã ngăn chặn các có hại và chở che cho sự sống. Các đặc điểm của Trái Đất cũng như hay , cho phép sự sống tồn tại trong qua. Người ta ước tính rằng Trái Đất chỉ còn có thể hỗ trợ sự sống thêm 1,5 tỷ năm nữa, trước khi kích thước của tăng lên (trở thành ) khiến bề mặt hành tinh nóng lên và tiêu diệt hết mọi , có thể gọi ngày đấy là ."
            });

            Add(new PlanetModel
            {
                Id = 4,
                Name = "Mars",
                VnName = "Sao Hỏa",
                Content = "Sao Hỏa hay Hỏa Tinh (: 火星; : Mars) là thứ tư ở và là ở xa Mặt Trời nhất, với bán kính bé thứ hai so với các hành tinh khác. Sao Hoả có màu cam đỏ do bề mặt của hành tinh được bao phủ bởi lớp vụn , do đó còn có tên gọi khác là \"hành tinh đỏ\". Vì bán cầu Bắc của Sao Hoả có chiếm đến 40% diện tích hành tinh, so bán cầu Nam thì bán cầu Bắc phẳng hơn và ít hơn. khá mỏng với thành phần chính là . Ở hai cực Sao Hoả là lớp băng được làm bằng "
            });

            Add(new PlanetModel
            {
                Id = 5,
                Name = "Jupiter",
                VnName = "Sao Mộc",
                Content = "Sao Mộc (: Jupiter) hay Mộc Tinh (: 木星) là tính từ và là hành tinh trong . Nó là với bằng một phần nghìn của Mặt Trời nhưng bằng hai lần rưỡi tổng khối lượng của tất cả các hành tinh khác trong cộng lại. Sao Mộc được xếp vào nhóm hành tinh khí khổng lồ cùng với ( và được xếp vào ). Hai hành tinh này đôi khi được gọi là hoặc hành tinh vòng ngoài. Các nhà thiên văn học cổ đại đã biết đến hành tinh này, và gắn với thần thoại và niềm tin tôn giáo trong nhiều nền văn hóa. đặt tên hành tinh theo tên của , vị thần quan trọng nhất trong số các vị thần. Tên gọi trong tiếng Trung Quốc, tiếng Triều Tiên, tiếng Nhật và tiếng Việt của hành tinh này được đặt dựa vào hành \"mộc\" trong . Khi nhìn từ , Sao Mộc có −2,94, đủ sáng để tạo bóng; và là thiên thể sáng thứ ba trên bầu trời đêm sau và . ( hầu như sáng bằng Sao Mộc khi Sao Hỏa ở những vị trí xung đối trên quỹ đạo của nó với )."
            });

            Add(new PlanetModel
            {
                Id = 6,
                Name = "Saturn",
                VnName = "Sao Thổ",
                Content = "Sao Thổ (: Saturn), hay Thổ Tinh (土星) là thứ sáu tính theo khoảng cách trung bình từ và là , sau trong . Tên của hành tinh mang tên thần trong , ký hiệu thiên văn của hành tinh là (♄) thể hiện của thần. Sao Thổ là với bán kính trung bình bằng 9 lần của . Tuy khối lượng của hành tinh cao gấp 95 lần khối lượng của Trái Đất nhưng với thể tích lớn hơn 763 lần, khối lượng riêng trung bình của Sao Thổ chỉ bằng một phần tám so với của Trái Đất."
            });

            Add(new PlanetModel
            {
                Id = 7,
                Name = "Uranus",
                VnName = "Sao Thiên Vương",
                Content = "Sao Thiên Vương (: Uranus) hay Thiên Vương Tinh (: 天王星) là thứ bảy tính từ , là trong . Sao Thiên Vương có thành phần tương tự như . Cả hai có thành phần hóa học khác so với hai lớn hơn là và . Vì vậy, các nhà thiên văn thỉnh thoảng đưa các hành tinh này vào danh sách. Khí quyển của Sao Thiên Vương tương tự như của Sao Mộc và Sao Thổ về thành phần cơ bản như và . Khác là chúng chứa nhiều hợp chất dễ bay hơi như , và cùng với lượng nhỏ . Hành tinh này có bầu lạnh nhất trong số các hành tinh trong , với nhiệt độ cực tiểu bằng 49 (−224 ). Nó có cấu trúc tầng mây phức tạp. Khả năng những đám mây thấp nhất chứa chủ yếu nước trong khi methan lại chiếm chủ yếu trong những tầng mây phía trên. Ngược lại, cấu trúc bên trong Sao Thiên Vương chỉ chứa chủ yếu một lõi băng và đá."
            });

            Add(new PlanetModel
            {
                Id = 8,
                Name = "Neptune",
                VnName = "Sao Hải Vương",
                Content = "Sao Hải Vương (: Neptune), hay Hải Vương Tinh (: 海王星) là thứ tám và xa nhất tính từ trong . Nó là hành tinh lớn thứ tư về và lớn thứ ba về . Sao Hải Vương có khối lượng riêng lớn nhất trong số các hành tinh khí trong hệ Mặt trời. Sao Hải Vương có khối lượng gấp 17 lần khối lượng của và hơi lớn hơn khối lượng của (xấp xỉ bằng 15 lần của Trái Đất). Sao Hải Vương quay trên quỹ đạo quanh Mặt Trời ở khoảng cách trung bình 30,1 , bằng khoảng 30 lần khoảng cách Trái Đất - Mặt Trời. Sao Hải Vương được đặt tên theo biển cả của người La Mã (Neptune). Nó có ký hiệu thiên văn là ♆, là biểu tượng cách điệu cây của hoặc của Hy Lạp."
            });
        }
    }
}
