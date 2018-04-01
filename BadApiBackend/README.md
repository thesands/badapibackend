To run this app:

The badapi needs to also be downloaded.  You can get the code for the frontend here: https://github.com/thesands/badapi

Once both the back-end and front-end pieces are downloaded, run the back-end.
(You can make sure the back-end is properly running by going to http://localhost:8080/api/values, make sure
your IDE is hosting it on the appropriate port)

You can either run a dev instance of the front-end or a production build.  To run a dev instance, open the terminal in the
front-end directory and run: npm run start.  This will begin the front-end compilation.  You should be able to go to
http://localhost:3000/ and see it properly displayed.

To run a production version of the front-end, start Docker and run the following commands in the front-end directory:
docker build -t react-docker .
This will serve the api on http://localhost:5000/

My main C# file for the back-end is Program.cs.  Here, I create a webhost to place the Tweets dictionary I create in Tweets.cs.
This allows me the frontend to retrieve the data through http://localhost:8080/api/values (the ValuesController.cs handles the Get reqeuest
for when the front end calls the back-end.  It simply returns the dictionary.)  Before the webhost is created, I created a recursive GetAllTweets
function that starts with the beginning date (1/1/16) and final end date (1/1/18).  This function behaves similar to merge sort, using the limit of
100 tweets as an indicator where the date range needs to be cut.  If a response string returns with 100 tweets, the current start and end dates are halved
to get a value in between the two (for example, jan 15 would be new end date between jan 1 and jan 31) and then merge sort continues with the
two separate halves.  Once a response string is below 100 tweets, all of the tweets are created as a Tweet class and added to the dictionary.  Using the
Tweet class creates the tweets as separate objects to be placed in the table. 

If I had more time to work on the back-end piece, I would enable the ability to search for a specific tweet, add or a delete a tweet,
back-end pagination and/or filtering.