This is a coding exercise for Jack Hnry & Associates.  I received a document describing the basics of the 
project in terms of what it should do, function, etc.

This ReadMe document is in part to capture my thought process as I started working on this project.  
To reiterate, this project is to consume a sample Twitter feed and save the data in memory while being able to report, in some fashion,
on that data.

Given these rquirements my thought process jumped to creating a web services project.

I have chosen to use the .NET Framework, v4.7, for my solution.  In support of SOLID principles and SOA 
service design principles and service patterns, I have chosen to arrange my project with an awareness of the
principle of Separation of Concerns.  Here I will decompose the solution into the following layers and
projects:
	
	API - api endpoints and http request life cycly management proviced by WebAPI
		TwitterFeed.Api
		TwitterFeed.Api.UnitTest
		TwitterFeed.DependencyManagement
	Business Logic - Core business logic
		TwitterFeed.BusinessLogic
		TwitterFeed.BusinessLogic.Interface
		TwitterFeed.BusinessLogic.UnitTest
	Contract - Contains Data Transformation Object (DTOs)
	Repository - Data Access Layer - the only layer where DB/Data Store access is permitted
		TwitterFeed.Repository.InMemory
		TwitterFeed.Repository.InMemory.UnitTest
		TwitterFeed.Repository.Interface
		TwitterFeed.Repository.SqlServer
		TwitterFeed.Repository.SqlServer.UnitTest

Initially I created a VS solution named JHA.TwitterFeed.  Then I created a second VS solution nammed JHA.WebServices and made Twitter a component
thereof.  I prefer the latter in that it allows multiple projects to be included in the same code base;  here you increase code resuse, service 
resuability, and so on.

Other libraries/frameworks used:
	MS WebAPI (framework for web service hosting and HTTP request life cycle management - VS solution type)
	NUnit and NUnit Framework for Unit Testing (added via NuGet)
	log4net for logging (added via NuGet, but not used as of yet)
	EmojiData (not used as of yet)
	TweetinviAPI (Twitter API)
	SimpleInjector (for Dependency Management)
	NewtonSoft.Json

Tasks: (Here I list the tasks I came up with Day One and the add "==>" below to describe its status)

Research Twitter sample stream endpoint
	identity inputs & outputs
	inputs: 
		oauth endpoint:
		stream endpoint: https://developer.twitter.com/en/docs/twitter-api/tweets/sampled-stream/api-reference/get-tweets-sample-stream
	Get/determine Twitter stream object definition
==>	turns out these questions were basically answered by the TweetinviAPI documentation.  I was getting ready to create a new HTTP Client class
	in order to listen to the stream but TweetinviAPI took care of that. I had originally spent upwards of a day researching the Twitter 
	Developer Portal and saw examples of connection explicitly to the sample stream endpoint.  It was at this point I relayed a message
	to Jack Henry & Associates that I was spending too much time on Twitter.  JHA responded that I look into this TwitterinviAPI project.
	Thanks for the tip!  That made life much easier.

Download emoji.json convinience file from GitHub and add to my solution
==> I have downloaded the file and added it to my project in several locations (BusinessLogic and BusinessLogic.UnitTest).  I have to admint I
	did not implment this. 

Add EmojiData NuGet package to my solution
==> added then remomved.  Not sure if it would help.

Research usage of the EmojiData library and review emoji.json file
Compare emoji.json file contents with Twitter sample stream to see how it can be used (for reporting primarily)
==> incomplete

Get Twitter API Access & token
==> done.  You can see my keys in any App.config (or Web.config) files located in various projects.  I've regenerate the various keys/tokens
	several times by now (using the Twitter Developer portal) and the config files are up-to-date.

Determine a viablew in-memory storage facility (should be deferred/persisted to DB long-term)
==> To maintain data in memory I created a class in the Repository.InMemory project called TwitterResults.cs.  Here I use the Singleton pattern
	so that the class in instanciated once, data is persisted to it via the stream enpoint (listed above).  Twitter statistics can then be 
	gathered (at the same time) on an on-demand basis.

Determine what API endpoints are needed
	read Twitter sample stream
	report statistics
		all (as listed in documenation + any additional info)
		individual ?
		multiple ?
		will need an ApiRequest DTO class ???
==> GET http://localhost:64274/api/twitter
	This endpoint is used to kick off the sample stream consumer.  This enpoint responds with 
		"Twitter Client has been created and is consuming sampe tweet stream ..."
==> GET http://localhost:64274/api/twitter/report
	This endpoint is used to request metrics/statistics while the stream is being consumed.  The endpoint can be "hit" at any point in time
	while the twitter stream is being consumed.  If it is hit when the stream is not being consumed, it returns a skeloton TwitterStatistics
	class this is all zeroes.

Establish Unit Tests
==> created for Api, BusinessLogic, Repository.InMemroy, Repository.SqlServer.  Normally these unit test projects are used extensively.  In this
	case I used them to help with the following:
	BusniessLogic - support the SOLID principle of single purpose by creating multiple methods to do a single purpose (e.g. CreateTwitterCredentials_Test).
	Repository.InMemory - also in support of SOLID single purpose for the same reason.  But in this case my reporting staticts were not quite right
							so I created a couple of tests using different data types (int, decimal, double, float) until I found the proper data
							type to use.  Then I updated the Contract.TwitterStatistics class with that type (float won out).  This code was then
							incorporated into the Repository.InMemory project.
Include error handling (no happy path here)
Apply SOLID principles to the design to:
	apply patterns to scale
	- an in-memory data store will not scale, long-term.  The long-term solution would be to persist the data to a durable data store, like
	  a SQLServer database.
	- I've chosen a web service implementation (vs stand-alone console app) for scalability.  This web service application can be deployed to 
	  one or more IIS deployments (or if you choose to use Azure/AWS to spin up instances as deemed necessary)
	form loosely coupled dependenicies 
		- use Dependency Injection and Interfaces to minimize dependencies.  Note that I have injected the Reposory.InMemory concrete implementation
		  via the InjectDependencies class.  This can be replaced with Repositoyry

Requirements:
 1. Create and consume the Twitter sample stream - saving Tweets in-memory
    - endpoint to kick off the create & consume feed: GET http://localhost:64274/api/twitter/
 2. Provide a means to report selected statistics to an end user while still consuming the Twitter sample stream 
    - endpoint GET http://localhost:64274/api/twitter/report


Summary:

	While I did not get everything to work I believe that I was successful in getting the main requirements to work (stated above).  
	- I used Postman to 
		- essentially start the sample stream process via a REST API endpoint
		- retrieve Tweet statistics via another REST API endpoint
	I also added a .NET Core Console application (DotNetCoreDemoApp) to the solution.  Sometimes this is how I initially work on projects;
	in particular, I used the console appp to get the TwitterinviAPI to work.  

	In terms of Agile dependencies, I would have created a Twitter Prototype story during Agile Planning to take place in an iteration/sprint
	before this functionality was really needed.  I would say the same for any new technology being proposed for a given project.  By using
	this time to study and prototype the targeted technology, developers can then proceed swiftly when this requirement actually becomes a
	reality.  

	I added the emoji.json file from the emoji-data project to the BusinessLogic and BusinessLogic.UnitTest projects but did not get around
	to implementing it.  In the Tweet streams that I was able to capture, it was unclear to me how to use the emoji.json file.

	Not everything is complete and you'll see some commented-out code.  That shows how my thought process works when striving to get the
	right solution.  Normaly I go back through and remove that commented code but thought I would leave it in for you with just for show.

	I've added 2 Postman files to the Solution Items folder:
		- JHA_postman_collection-EdWax.json
		- JHA_postman_environment-EdWax.json

	If you use Postman, please import these and use them to start and report statistics on the Twitter sample stream.

	To run this application:

	Open the VS solution and build and ensure there are no errors (use the default Debug configuration).  
	Press F5 or click the green arrow in fron of the text "IIS Expresss (Firefox)" in the VS File menu.  This will start the webservices.
	Start the stream process by using the provided Postman script "Start and consume Twitter sample stream"
	Periodically get the stream statistics by using the provided Postman script "GET Twitter Feed Statistical Results (on demand)"

	Thanks,

	Ed Wax