# Console Photo Album

### This is my solution to the photo album code challenge from Lean Techniques.

---

## Here is the challenge that was presented:

```
Create a console application that displays photo ids and titles in an album. The photos are available in this online web
service: https://jsonplaceholder.typicode.com/photos

Hint: Photos are filtered with a query string. This will return photos within albumId=3
https://jsonplaceholder.typicode.com/photos?albumId=3

• You can use any language
• Any open source libraries
• Unit tests are encouraged
• Post your solution on any of the free code repositories and send us a link:
• https://github.com/
• https://gitlab.com/
• https://bitbucket.org/

Provide a README that contains instructions on how to build and run your application.

Spend as much (or little) time as you’d like on this. Feel free to use any resources available.

Example:
>photo-album 2
[53] soluta et harum aliquid officiis ab omnis consequatur
[54] ut ex quibusdam dolore mollitia
...
>photo-album 3
[101] incidunt alias vel enim
[102] eaque iste corporis tempora vero distinctio consequuntur nisi nesciunt
```

---

## Running locally

You can run this application directly if your machine has the dotnet sdk available, or you can run in Docker.

- To run with dotnet on your machine
  - `dotnet run --project ConsolePhotoAlbum`
- To run with Docker
  - `docker-compose up`
