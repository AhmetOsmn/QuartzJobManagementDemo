# QuartzJobManagementDemo

- Demo EF Core ile geliştirildi. Kullanmak istediğiniz database'i `appsettings.json` içerisinde alt kısımdaki alanları düzenleyerek belirleyebilirsiniz.
  
  ```json
  {
    "ConnectionStrings": {
        "SqlServer": "Server=localhost, 1433;Database=QuartzJobManagement;User Id=sa;{{Password}}=password;TrustServerCertificate=True", // password düzeltilmeli.
        "Postgres": "User ID=postgres;Password={{password}};Host=localhost;Port=5432;Database=QuartzJobManagement;" // password düzeltilmeli.
    },
    "Database": "Postgres" // SqlServer veya Postgres olabilir.
  }
  ```

- Sonrasında migration ekleyerek database'i oluşturabilirsiniz. 
  Son olarak, Quartz için gerekli tabloları oluşturmak için seçtiğiniz database'e uygun olan script'i [buradan](https://github.com/quartznet/quartznet/tree/main/database/tables) alarak çalıştırmalısınız.

## Job Tanımlama Alanı

![image](https://github.com/user-attachments/assets/da8605ef-3e03-4207-93a4-f6a99b83e52f)

## Job Schedule Alanı

![image](https://github.com/user-attachments/assets/02070fb5-7fd4-48e6-af84-48ed9a5e2253)

## Job Monitoring

![image](https://github.com/user-attachments/assets/31492f76-17ab-4dce-939a-a48ea1e5030e)
