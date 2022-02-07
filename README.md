# Console Photo Album

---
## This repo is my solution to the photo album code challenge from Lean Techniques

- I went through some iterations of trying to figure out how to set up the program parameters simply for the user.
  - I ultimately landed on passing an `action`, a `resource`, and optional `flags`.
  - This feels a lot like `kubectl` and other cli tools I've used, so it made sense to me.
- There are more endpoints and methods available in the demo api I was asked to work with.
  - I only implemented `get`ing `images` and `albums`, but I tried to write the code in a way that it could easily be augmented.

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

You can run this application directly with dotnet if your machine has the dotnet sdk available, or you can run with Docker.

- To run with dotnet on your machine
  - `dotnet run --project ConsolePhotoAlbum get albums [--albumId=3] [--searchText=abc]`
  - `dotnet run --project ConsolePhotoAlbum get images [--albumId=3] [--searchText=abc]`
- To run with Docker
  - `docker-compose build`
  - `docker-compose run consolephotoalbum get albums [--albumId=3] [--searchText=abc]`
  - `docker-compose run consolephotoalbum get images [--albumId=3] [--searchText=abc]`
