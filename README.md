# Nio AI Chat (Web Version)

Branch `public` ini berisi source code murni untuk aplikasi **Nio Chat versi Web**.
Aplikasi ini dibangun menggunakan **ASP.NET Core (Blazor Web App)**. Berbeda dengan versi desktop, versi ini dirancang untuk diakses melalui browser (seperti web server biasa) dan tidak menggunakan library native-desktop.

## Prasyarat (Prerequisites)
- .NET 10 SDK
- Ollama dengan model `rash5679/nio` sudah terinstall dan berjalan (default di `http://localhost:11434`)

## Cara Menjalankan Aplikasi
1. Clone branch ini:
   ```bash
   git clone -b public https://github.com/Rashfox/Nio.git
   cd Nio
   ```
2. Jalankan aplikasi menggunakan .NET CLI:
   ```bash
   dotnet run
   ```
3. Buka browser dan arahkan ke alamat URL yang tertera di terminal (contoq: `http://localhost:5000`).

## Konfigurasi
Aplikasi akan secara otomatis mencoba menghubungkan diri ke Ollama lokal di port standar. Jika ingin mengubahnya, kamu bisa menyesuaikan URL Ollama di `Program.cs`.

---
*Nio AI Chat - Smart & Local AI Assistant*
