# What is URLshorterner

It's a simple service that generate short url to reduce the length of your URL.

- Short URL generated are kept in memory only
- They are kept for up to 30 minutes
- No security, it's public.

# Run the URLShortener service

## Build the docker image
Clone the repository and from the root folder execute the following command:

`docker build -t urlshortener .`

## Run the image
To start the service listening on port 8080:

`docker run -d -p 8080:80 --name urlshortener urlshortener`

# Use the service

## Create a short url
`curl --header 'Content-Type: application/json' --data '{"url": "https://google.ca"}' --request POST http://localhost:8080/shortener`

## Use the short url
In a browser, navigate to the url: `http://localhost:8080/<id>`
