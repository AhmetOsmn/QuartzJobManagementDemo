# Quartz Job Management Demo

## Gereksinimler

- SqlServer veya Postgres database.
- Elasticsearch
  - Elastic için bir adet network oluşturmak için:

        docker network create elastic-net
       
  - Elasticsearch  container'ı oluşturmak için alt kısımdaki komutu kullanabilirsiniz (password düzeltilmeli):
        
        docker run -p 127.0.0.1:9200:9200 -d --name elasticsearch --network elastic-net -e ELASTIC_PASSWORD=elasticPassword -e "discovery.type=single-node" -e "xpack.security.http.ssl.enabled=false" -e "xpack.license.self_generated.type=trial" docker.elastic.co/elasticsearch/elasticsearch:8.14.3

  - Kibana şifresini oluşturmak için alt kısımdaki komutu oluşturduğunuz elasticsearch container'ının içerisinde kullanabilirsiniz (password düzeltilmeli):

        curl -u elastic:elasticPassword -X POST http://localhost:9200/_security/user/kibana_system/_password -d '{"password":"kibanaPassword"}' -H 'Content-Type: application/json'

- Kibana
  - Kibana  container'ı oluşturmak için alt kısımdaki komutu kullanabilirsiniz (password düzeltilmeli):
   
        docker run -p 127.0.0.1:5601:5601 -d --name kibana --network elastic-net -e ELASTICSEARCH_URL=http://elasticsearch:9200 -e ELASTICSEARCH_HOSTS=http://elasticsearch:9200 -e ELASTICSEARCH_USERNAME=kibana_system -e ELASTICSEARCH_PASSWORD=elasticPassword -e "xpack.security.enabled=false" -e "xpack.license.self_generated.type=trial" docker.elastic.co/kibana/kibana:8.14.3

- Elasticsearch ile ilgili alt kısımdaki alanları güncelleyiniz:

  ```json
  {
     "Elasticsearch": {
      "Uri": "http://localhost:9200", //elasticsearch container'ı oluştururken tanımladığınız adres.
      "DefaultIndex": "quartz-job-management",
      "Username": "elastic", // varsayılan kullanıcı adı.
      "Password": "password" // elasticsearch container'ı oluştururken tanımladığınız password.
    }
  }
  ```

  `Not:` Kibana'da log'ları görebilmek için index pattern olarak yukarıda belirtilen ***DefaultIndex*** değerini şu şekilde vererek `quartz-job-management*` view data oluşturabilirsiniz.

- Kullanmak istediğiniz database'i `appsettings.json` içerisinde alt kısımdaki alanları düzenleyerek belirleyebilirsiniz.
  
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

## Mimari
![image](https://github.com/user-attachments/assets/8e55329c-1337-4348-a5a9-a49702accbf3)

## Job Tanımlama Alanı

![image](https://github.com/user-attachments/assets/da8605ef-3e03-4207-93a4-f6a99b83e52f)

## Job Schedule Alanı

![image](https://github.com/user-attachments/assets/02070fb5-7fd4-48e6-af84-48ed9a5e2253)

## Job Monitoring

![image](https://github.com/user-attachments/assets/31492f76-17ab-4dce-939a-a48ea1e5030e)
