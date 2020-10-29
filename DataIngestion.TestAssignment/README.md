# DataIngestion.TestAssignment

### Task desciption:
- The task is to store music data in Searchable database - ElasticSearch - 
- In google drive folder https://drive.google.com/drive/folders/1RkUWkw9W0bijf7GOgV4ceiFppEpeXWGv, there are 4 compressed files representing 4 database tables extracted from relational database.
- The files are (Artist - ArtistCollection - Collection - CollectionMatch)
- We would like the solution to download, extract and read the files then inject Collection object into ElasticSearch index named albums
- Album Object should be like this

```
{
  "id": "1255407551",
  "name": "Nishana - Single",
  "url": "http://ms.com/album/nishana-single/1255407551?uo=5",
  "upc": "191061793557", // found in CollectionMatch file
  "releaseDate": "2017-06-10T00:00:00",
  "isCompilation": false,
  "label": "Aark Records",
  "imageUrl": "http://img.com/image/thumb/Music117/v4/92/b8/51/92b85100-13c8-8fa4-0856-bb27276fdf87/191061793557.jpg/170x170bb.jpg",
  "artists": [
    {
      "id": "935585671",
      "name": "Anmol Dhaliwal"
    }
  ]
}
```

![Test Image 1](https://github.com/GetLinkfire/DataIngestion.TestAssignment/blob/main/diagram.png)

### Prerequisites:
- Docker
- ElasticSearch http://localhost:9200/. it can be configured using docker image

### Requirments:
- Use dotnet core Console application
- Extract the files in your solution. Don't use external tools.
- You are allowed to use any technology you would like to use (AzureEventhub, EMQ, SqlServer, etc..)
- Feel free to design your solution in one or more microservices/console apps
- Solution should be testable, we are not looking for 100% code coverage but show examples of how you make parts unit-testable.
- You are free to use third-party libraries

We want to get a better understanding of:
- The code you produce
- How you go about architecting an extensible solution!

### Notes:
- Please don't make it more complex than necessary.
- We would like to see a piece of code as you would do on your normal working day.
- Please don't fork this project and create your own repository.
- Please send us whatever you have done before the deadline even if it is an incompleted task.
- Don't hesitate to contact us for questions and support while working on the task.
