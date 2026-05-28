# ToDoListApp

C# ile yazılmış Windows Forms görev takip uygulaması.
Collegium Da Vinci — Nesne Yönelimli Programlama final projesi.

## Ne yapıyor?
- Tarih ve öncelikli (Düşük / Orta / Yüksek) görev ekle, düzenle, sil
- Görevleri tamamlandı olarak işaretle / yeniden aç
- Süresi geçmiş görevleri görüntüle
- Tarihe veya önceliğe göre sırala
- Dosyaya otomatik kaydeder, tüm işlemleri loglar

## Kullanılan OOP Tasarım Desenleri
- Singleton (TaskManager, Logger)
- Decorator (UrgentTaskDecorator, OverdueTaskDecorator)
- Interface (ITask)
- Delegate & Lambda (SortStrategy)
- LINQ (filtreleme, sıralama)

> Yalnızca Windows'ta çalışır (.NET, WinForms)
