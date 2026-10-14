# Nio AI Chat (Web Version)

Branch `public` ini berisi source code murni untuk aplikasi **Nio Chat versi Web**.
Aplikasi ini dibangun menggunakan **ASP.NET Core (Blazor Web App)**. Berbeda dengan versi desktop, versi ini dirancang untuk diakses melalui browser (seperti web server biasa) dan tidak menggunakan library native-desktop.

## Prasyarat (Prerequisites)
- .NET 10 SDK
- Ollama dengan model `rash5679/nio` sudah terinstall dan berjalan (default di `http://localhost:11434`, jalankan perintah `ollama run rash5679/nio`)

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
3. Buka browser dan arahkan ke alamat URL yang tertera di terminal (contoh: `http://localhost:5000`).

## Konfigurasi
Aplikasi akan secara otomatis mencoba menghubungkan diri ke Ollama lokal di port standar dengan model `rash5679/nio`. Jika ingin mengubahnya, kamu bisa menyesuaikan konfigurasi di `appsettings.json`.

---
*Nio AI Chat - Smart & Local AI Assistant*
