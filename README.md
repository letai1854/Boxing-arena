Scene Chính: Để bắt đầu, vui lòng mở Scene chính của game tại đường dẫn:
Assets/EZ Assets/Scenes/Tai-Le_IB-Fight-Scene.unity

Link to file apk:
https://drive.google.com/file/d/1L0qmuh5EsVzmVJLAHMG3Nbm_A506cW31/view?usp=sharing

Link to file mp4:
https://drive.google.com/file/d/1dYTZ8RbffExmjJwmCaOP_OgkwesO12Pl/view?usp=sharing


Giải Thích Thuật Toán - Câu 2
1. Thuật Toán Sinh 10 Level (Yêu cầu #1)
Để đáp ứng yêu cầu tạo 10 level có độ khó tăng dần và sự thay đổi rõ rệt, tôi đã xây dựng một thuật toán để tự động sinh ra dữ liệu cho từng level.

Đầu tiên, tôi thiết lập các chỉ số gốc cho 3 loại nhân vật chính trong game: Player, Ally (đồng đội), và Enemy (kẻ địch). Dựa trên các chỉ số gốc này, thuật toán sẽ áp dụng các quy tắc sau cho mỗi level (lv):

A.Cấu Trúc Level và Số Lượng:

Tôi chia 10 level thành các cụm để tạo ra sự khác biệt lớn về gameplay, với số lượng được tính toán theo công thức để đảm bảo tính nhất quán:
Level 1-3: Là các màn 1 vs 1.
Level 4-6: Chuyển sang chế độ 1 vs Many. Số lượng kẻ địch sẽ tăng dần từ 2 lên 4, được tính bằng công thức: Số Enemy = lv - 2
Level 7-10: Là các màn Many vs Many. Người chơi sẽ có thêm các đồng đội để tham gia cùng. Số lượng Ally và Enemy được tính toán dựa trên số level để đảm bảo độ khó tăng dần. Cách tính:
Số lượng đồng đội: Số Ally = lv - 6.
Số lượng kẻ địch: Số Enemy = (lv - 6) * 2 + 3.
Các số được chọn nhầm đảm bảo enemy luôn có lợi, làm cho cơ chế cân bằng thấy được sự hữu ích.

B.Cách Tăng Độ Khó:

Độ khó được tăng cường qua mỗi level bằng hai cách chính:

1.Tăng Chỉ Số Cơ Bản Của Enemy:	
Mỗi level, các chỉ số của Enemy được cộng thêm một lượng bonus phần trăm so với chỉ số gốc. Cụ thể: Máu và Sát thương được cộng 11%, trong khi Tốc độ di chuyển được cộng 4% cho mỗi level.
Tốc độ tấn công của Enemy cũng được tăng lên. Cứ mỗi 3 level, thời gian hồi chiêu (cooldown) của chúng được giảm đi 10%.
2.Cơ Chế Cân Bằng Tự Động:
Để các trận đấu không quá chênh lệch, thuật toán sẽ kiểm tra số lượng của hai phe.
Nếu một phe bị áp đảo về số lượng, phe đó sẽ được nhận một lượng bonus nhỏ. Quy tắc là: cứ mỗi 2 thành viên chênh lệch, phe yếu hơn sẽ nhận một gói buff 5%.
Gói buff này sẽ được cộng chủ yếu vào Máu, và một phần nhỏ hơn vào Sát thương, Tốc độ, và giảm thời gian hồi chiêu (nếu là phe Player/Ally). Điều này giúp phe yếu thế hơn có cơ hội chống trả mà không làm game trở nên quá dễ.


Các Phương Pháp Tối Ưu Hóa Cho 50 Models (Yêu cầu #2)
GPU Instancing:
Kích hoạt tùy chọn Enable GPU Instancing trên các Material của nhân vật. Việc này giúp card đồ họa có thể vẽ nhiều nhân vật giống nhau chỉ trong một lần.
Object Pooling:
Xây dựng một hệ thống Object Pooler để quản lý việc tạo và hủy các nhân vật. Thay vì dùng Instantiate và Destroy liên tục, hệ thống sẽ tạo sẵn một lượng lớn các đối tượng lúc đầu và chỉ SetActive(true/false) để tái sử dụng chúng. 
Animator Culling:
Culling Mode của các component Animator trong asset được cung cấp đã được đặt sẵn là Cull Update Transforms. Thiết lập này đã được giữ nguyên vì nó giúp CPU không tốn tài nguyên tính toán animation cho các nhân vật đang ở ngoài khung hình camera.
Tối ưu hóa AI (Time Slicing):
Logic AI để tìm kiếm mục tiêu và ra quyết định được tôi đặt trong một Coroutine. Thay vì chạy mỗi frame, Coroutine này chỉ thực thi theo một chu kỳ nhất định (ví dụ: 4-5 lần mỗi giây). Việc này giúp phân chia việc xử lý của các AI ra nhiều frame, tránh được tình trạng tất cả cùng "suy nghĩ" một lúc gây quá tải cho CPU.
Tối ưu hóa Va chạm Vật lý:
Để giảm tải cho hệ thống vật lý, tôi không dùng Mesh Collider cho hàng rào võ đài. Thay vào đó, tôi đã dùng vài Box Collider đơn giản để tạo thành các bức tường chắn. Cách này giúp việc tính toán va chạm nhanh hơn nhiều, góp phần giữ FPS ổn định khi có nhiều nhân vật di chuyển.
