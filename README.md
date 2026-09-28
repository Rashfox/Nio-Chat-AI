# Nio AI Chat (Desktop Version)

Branch `programs` ini berisi source code murni untuk aplikasi **Nio Chat versi Desktop Native**.
Aplikasi ini dibangun menggunakan **Blazor Server** dan **Photino.NET** sehingga bisa berjalan sebagai aplikasi desktop lokal (seperti game offline) tanpa tampilan terminal/konsol sama sekali.

## Prasyarat (Prerequisites)
- .NET 10 SDK (jika ingin build sendiri dari source)
- Ollama terinstall di komputer dengan model `rash5679/nio`

## Cara Instalasi dari Rilis (Disarankan)
1. Buka halaman **[Releases](../../releases)** di repository ini.
2. Download file **NioChatSetup.exe** versi terbaru.
3. Jalankan installer dan ikuti petunjuknya.
4. Buka **Nio Chat** dari shortcut di Desktop.

## Cara Build Mandiri (Untuk Developer)
Jika ingin melakukan build installer dari source code ini:

1. Clone branch ini:
   ```bash
   git clone -b programs https://github.com/Rashfox/Nio.git
   ```
2. Lakukan publish menjadi executable standalone:
   ```bash
   dotnet publish -c Release -o app_publish
   ```
3. Gunakan **Inno Setup** (6.x) dan jalankan script `installer.iss` yang sudah disediakan untuk mem-build `NioChatSetup.exe`.

---
*Nio AI Chat - Smart & Local AI Assistant*
