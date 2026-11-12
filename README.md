# DuckFarm

DuckFarm adalah project baru Windows Forms .NET 8 yang dibuat dari kerangka UI DuckPayBoro. Project asli DuckPayBoro tidak diubah.

## Isi project

- `DuckMENU` sebagai form utama MDI.
- `DuckSidebar` untuk navigasi.
- `MdiManager` untuk pengelolaan form MDI.
- `ThemeManager` dan kontrol UI pendukung.
- Dashboard awal sederhana dengan identitas DuckFarm.
- Transaksi CWR (`frmTrnAbsensidanHasilCWR`).
- Form pencarian CWR (`frmTrnAbsensidanHasilCWRShow`), yang dipanggil dari tombol Find pada form transaksi.



## Teknologi

- C# Windows Forms
- .NET 8 (`net8.0-windows`)
- C# Language Version 8.0
- FontAwesome.Sharp 6.6.0

## Menjalankan

1. Buka `DuckFarm.sln` menggunakan Visual Studio 2022 dengan workload **.NET desktop development**.
2. Pastikan .NET 8 SDK dan Windows Forms targeting pack tersedia.
3. Restore NuGet packages.
4. Set `DuckFarm` sebagai Startup Project.
5. Build lalu jalankan.

Project ini saat ini merupakan kerangka UI. Form CWR dan form pencariannya dipertahankan dari sumber, tetapi koneksi database/API dan proses bisnis belum ditambahkan.
