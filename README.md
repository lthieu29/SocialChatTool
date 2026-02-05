# Social Chat Tool - Multi-Platform Messenger

WPF .NET 8 application để quản lý và gửi tin nhắn từ nhiều nền tảng social media.

## 🚀 Quick Start

### 1. Cấu hình Database
Cập nhật `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=your_db;User=root;Password=123456;"
  }
}
```

### 2. Run Application
```bash
dotnet build
dotnet run
```

## 📖 Hướng dẫn sử dụng

1. **Chọn Page** từ dropdown
2. **Chọn Conversation** từ danh sách
3. **Nhập tin nhắn** vào TextBox
4. **Click "Gửi tin nhắn"**

## 🔧 TODO: Implement SendMessage

File: `Services/MessageService.cs`

```csharp
public async Task SendMessage(int socialType, string pageId, string conversationId, string message)
{
    // TODO: Implement logic dựa trên socialType
    // 1 = Zalo OA
    // 2 = Facebook
    // 3 = Zalo Personal
    // 4 = Shopee Chat
    // 5 = Tiktok Business
}
```

## 📚 Documentation

Xem [walkthrough.md](file:///C:/Users/lehie/.gemini/antigravity/brain/0b912620-5944-4b9f-b6c9-901fb65b1522/walkthrough.md) để biết chi tiết đầy đủ về:
- Architecture & Design
- Database Schema
- Implementation Guide
- Troubleshooting

## 🛠️ Tech Stack

- .NET 8 WPF
- Entity Framework Core 8.0.11
- Pomelo MySQL Provider 8.0.2
- CommunityToolkit.Mvvm 8.3.2

## ✅ Status

**Build**: ✅ Success (0 warnings, 0 errors)  
**Ready**: ✅ Sẵn sàng để implement SendMessage logic
