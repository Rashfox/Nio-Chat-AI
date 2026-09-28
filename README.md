# Nio AI Chat

Nio Chat adalah aplikasi asisten AI pintar yang dirancang untuk berjalan sepenuhnya secara lokal di mesin pengguna, didukung oleh **Ollama** dengan model custom `rash5679/nio` (berbasis Qwen2.5 7B). Aplikasi ini difokuskan pada produktivitas dan privasi, memastikan tidak ada data yang dikirim ke cloud.

Repository ini menggunakan sistem multi-branch untuk versi aplikasi yang berbeda.

## 🗂️ Struktur Repository (Branching)

Source code Nio Chat dipisahkan ke dalam beberapa branch berdasarkan platform yang didukung:

### 1. [Branch `programs`](https://github.com/Rashfox/Nio/tree/programs) (Desktop App)
Berisi source code untuk aplikasi **Nio Chat Desktop Native**.
- Dibangun menggunakan **Blazor** dan **Photino.NET**.
- Berjalan layaknya program game offline/standalone (tanpa terminal/konsol yang mengganggu).
- Menggunakan local file system untuk menyimpan riwayat chat (persisten).

### 2. [Branch `public`](https://github.com/Rashfox/Nio/tree/public) (Web App)
Berisi source code murni untuk aplikasi **Nio Chat Web-based**.
- Dibangun menggunakan **ASP.NET Core (Blazor Web App)** standar.
- Berjalan layaknya aplikasi server biasa dan diakses melalui web browser.

---

## 📥 Cara Instalasi / Download (Untuk Pengguna Biasa)

Jika kamu hanya ingin menggunakan aplikasinya tanpa melakukan kompilasi kode:

1. Buka halaman **[Releases](../../releases)**.
2. Download file Installer terbaru (contoh: `NioChatSetup.exe`).
3. Jalankan file `.exe` tersebut dan ikuti petunjuk instalasinya.
4. Buka aplikasi dari Shortcut di Desktop atau Start Menu.

> **Catatan:** Pastikan kamu sudah menginstall Ollama dan mem-pull model Nio dengan menjalankan perintah `ollama pull rash5679/nio` di terminal sebelum menggunakan aplikasi ini.

---

## 🛠️ Untuk Developer

Pilih versi mana yang ingin kamu kembangkan, lalu pindah ke branch yang sesuai:

**Versi Desktop:**
```bash
git checkout programs
```

**Versi Web:**
```bash
git checkout public
```
Pilih panduan *Setup* dan *Build* selengkapnya pada `README.md` di masing-masing branch tersebut.
